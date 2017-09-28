using System;
using System.Collections;

namespace NetgameProj
{
	/// <summary>
	/// ルールとデータを含むゲームオブジェクトです。
	/// </summary>
	[Serializable()]
	public class Game
	{
		private ArrayList combatStack = new ArrayList();		// トループカードのスタック
		private ArrayList tacticsStack = new ArrayList();		//　タクティクスカードのスタック
		private ArrayList graveyardStack = new ArrayList();		//　墓地
		private SortedList [] hand = {new SortedList(), new SortedList()};
		private Flag [] flag = new Flag[Defines.NumOfFlag];
		private int [] numTCard = new int[2];	//タクティクスカードを使った枚数
		private bool [] useLeader = new bool[2];
		private int turn;				//現在のターン
		private Defines.Phase phase;	//フェイズ
		private Card pCard;	//選択されているカード
		
		public Game()
		{
			// 旗の作成と初期化
			for(int i=0; i<Defines.NumOfFlag; i++)
			{
				flag[i] = new Flag(i);
			}
			// ゲームコンポーネントの初期化
			InitializeObjects();
		}

		private void InitializeObjects()
		{
			//カード作成
			for(int c=0; c < 6; c++)
			{
				for(int n=1; n<11; n++)
				{
					string tail=n.ToString()+"_"+c.ToString();
					combatStack.Add(new CCard(n, c, "card_"+tail));
				}
			}
			for(int i=0; i<Defines.NumberOfTactics; i++)
			{
				string tail = i.ToString();
				tacticsStack.Add(new TCard((Defines.Tactics)i, "Tcard_"+tail));
			}

			Shuffle(combatStack, 2000);
			Shuffle(tacticsStack, 2000);

			for(int i=0; i<Defines.MaxHand; i++)
			{
				Draw(0,combatStack);
				Draw(1,combatStack);
			}

			Random r = new Random();
			turn = r.Next(2);
			phase = Defines.Phase.ChooseCard;
			pCard = null;
			numTCard[0] = 0;
			numTCard[1] = 0;
			useLeader[0] = false;
			useLeader[1] = false;
		}
		private void ClearObject()
		{
			combatStack.Clear();
			tacticsStack.Clear();
			graveyardStack.Clear();
			foreach(SortedList h in hand)
				h.Clear();
			foreach(Flag f in flag)
				f.clear_it();
		}

		//カードスタックをシャッフルする
		private void Shuffle(ArrayList arr, int count)
		{
			int n = arr.Count;
			Random r = new Random();
			for(int i=0; i<count; i++)
			{
				int p=r.Next(n);
				Object tmp=arr[p];
				arr.RemoveAt(p);
				arr.Insert(r.Next(n),tmp);
			}
		}

		//カードを引く
		private void Draw(int side, ArrayList stack)
		{
			int n = stack.Count;
			if(n > 0)
			{
				hand[side].Add(((Card)stack[n-1]).SortKey, stack[n-1]);
				stack.RemoveAt(n-1);
			}
		}
		//カードを山に戻す
		private void ReturnCard(Card c)
		{
			if(c.Name == Defines.Tactics.TROOP)
			{
				combatStack.Add(c);
			}
			else
			{
                tacticsStack.Add(c);			
			}
            RemoveCard(c);
		}

		//カードを置く
		private bool PutCard(int side, Card c, Flag f)
		{
			switch(c.Name)
			{
				case Defines.Tactics.SCOUT:
				case Defines.Tactics.REDEPLOY:
				case Defines.Tactics.DESERTER:
				case Defines.Tactics.TRAITOR:
					return false;
				case Defines.Tactics.MUD:
				case Defines.Tactics.FOG:
					f.AddEffect((TCard)c);
					return true;
				default:
					if(f.GetLength(side) < f.MaxNumber)
					{
						f.AddCard(c, side);
						return true;
					}
					return false;
			}
		}
		//カードを置く場所があるか
		private bool CanPut(int side)
		{
			for(int i=0; i<Defines.NumOfFlag; i++)
			{
				if(!flag[i].IsProved && flag[i].GetCard(side).Count < flag[i].MaxNumber)
					return true;
			}
			return false;
		}
		//場にカードがあるか
		private bool IsCard(int side)
		{
			for(int i=0; i<Defines.NumOfFlag; i++)
			{
				if(!flag[i].IsProved && flag[i].GetCard(side).Count != 0)
					return true;
			}
			return false;
		}

		//カードをすべての手札及びフラッグの参照先から取り除く
		private void RemoveCard(Card c)
		{
			hand[0].Remove(c.SortKey);
            hand[1].Remove(c.SortKey);
			for(int i=0; i<Defines.NumOfFlag; i++)
			{
                flag[i].RemoveCard(c);			
			}		
		}
		//カードをグレイブヤードに送る
		private void TrashCard(Card c)
		{
			graveyardStack.Add(c);
            RemoveCard(c);		
		}
		//　フラッグ獲得解決のチェック
		private bool ProveFlag(Flag f)
		{
			if(f.IsProved )
				return false;
			if(f.GetLength(Turn) < f.MaxNumber)
				return false;
			ArrayList used = new ArrayList();
			ICollection pl  = f.GetCard(Turn);
			ICollection opp = f.GetCard(NextTurn);
			// 見えているカードのリストを作成
			foreach(Flag it in flag)
			{
				used.AddRange(it.GetCard(0));
				used.AddRange(it.GetCard(1));
			}
			used.AddRange(graveyardStack);
			int winner = WinnerChecker.FlagWinner(pl, opp, used, f.MaxNumber, f.IsFog);
			if(winner == 0)
			{
				f.Prove(Turn);
                if (WinnerChecker.GameWinner(flag, Turn))
                {
                    phase = Defines.Phase.FinishGame;
                }
                return true;
			}
            return false;
		}
        
		//カードスタックがどちらも空か？
		public bool IsStackEmpty
		{
			get { return combatStack.Count == 0 && tacticsStack.Count == 0; }
		}
		//手札のリストを取得する
		public ICollection GetHand(int side)
		{
			return hand[side].Values;
		}
		//グレイブヤードのカードリストを取得する
		public ICollection GetGraveyard()
		{
            return graveyardStack;		
		}
		//index番目のフラッグを取得する
		public Flag GetFlag(int index)
		{
			return flag[index];
		}
		//現在のターンを取得する
		public int Turn
		{
			get { return turn; }
		}
		//次のターンを取得する
		public int NextTurn
		{
			get
			{
				if(turn == 0)
					return 1;
				else
					return 0;
			}
		}
		//現在選択されているカードを取得する
		public Card PresentCard
		{
			get { return pCard;}
		}
		//現在のフェイズを取得する
		public Defines.Phase Phase
		{
			get { return phase; }
		}
		//Tカードスタックの残り枚数を調べる
		public int RestTCard
		{
			get { return tacticsStack.Count; }
		}
		//Cカードスタックの残り枚数を調べる
		public int RestCCard
		{
			get { return combatStack.Count; }
		}

		//カードスタックが選択された
		public bool SelectStack(Defines.StackType t)
		{
			ArrayList arr;
			if(t == Defines.StackType.Tactics)
				arr = tacticsStack;
			else
				arr= combatStack;

			if(arr.Count == 0)
				return false;

			if(phase == Defines.Phase.ChooseStack)
			{
				if(hand[turn].Count < Defines.MaxHand)
				{
					Draw(turn, arr );
					phase = Defines.Phase.ChooseCard;
					turn = NextTurn;
					return true;
				}
			}
			else if(phase == Defines.Phase.ScoutDraw)
			{
				if(hand[turn].Count <= Defines.MaxHand + 1)
				{
					if(hand[turn].Count == Defines.MaxHand + 1 || IsStackEmpty )
						phase = Defines.Phase.Discard1;
					Draw(turn, arr );
					return true;
				}
			}
			return false;
		}

		// 「破棄」コマンドが選択された
		public bool SelectGraveyard()
		{
			if(phase == Defines.Phase.Redeploy2 && pCard != null)
			{
				TrashCard(pCard);					
				pCard = null;
				phase = Defines.Phase.ChooseStack;
				return true;
			}
			return false;
		}

		// 「使用」コマンドが選択された
		public bool SelectUse()
		{
			if(pCard == null || phase != Defines.Phase.ChooseFlag)
				return false;
			switch(pCard.Name)
			{
				case Defines.Tactics.SCOUT:
					if(IsStackEmpty)
						return false;
					phase = Defines.Phase.ScoutDraw;
					numTCard[turn]++;
					TrashCard(pCard);					
					pCard = null;
					return true;
				case Defines.Tactics.DESERTER:
					if(!IsCard(NextTurn))
						return false;
					phase = Defines.Phase.Deserter;
					numTCard[turn]++;
					TrashCard(pCard);					
					pCard = null;
					return true;
				case Defines.Tactics.REDEPLOY:
					if(!IsCard(turn))
                        return false;
					phase = Defines.Phase.Redeploy1;
					numTCard[turn]++;
					TrashCard(pCard);					
					pCard = null;
					return true;
				case Defines.Tactics.TRAITOR:
					if( !CanPut(turn) || !IsCard(NextTurn) )
						return false;
					phase = Defines.Phase.Traitor1;
					numTCard[turn]++;
					TrashCard(pCard);					
					pCard = null;
					return true;
			}
			return false;
		}

		// いずれかの場所にあるカードが選択された
		public bool SelectCard(Card c)
		{
			//手札にある場合
			if(hand[turn].ContainsKey(c.SortKey))
			{
				return SelectCardInMyHand(c);				
			}
			//場にある場合
			foreach(Flag f in flag)
			{
				//自分が出したとか相手が出したとか
				if(f.IsCard(c, Turn))
				{
					return SelectCardInMyFlag(c, f);				
				}
				else if(f.IsCard(c, NextTurn))
				{
					return SelectCardInYourFlag(c, f);				
				}
			}
			return false;
		}
        //自分の手札が選択された
		public bool SelectCardInMyHand(Card c)
		{
			switch(phase)
			{
                case Defines.Phase.ChooseFlag:
				case Defines.Phase.ChooseCard:
					if(c.Name != Defines.Tactics.TROOP && numTCard[turn] > numTCard[NextTurn])
						return false;
					if((c.Name == Defines.Tactics.ALEXANDER || c.Name == Defines.Tactics.DARIUS) && useLeader[turn])
						return false;
					pCard = c;
					phase = Defines.Phase.ChooseFlag;
					return true;
				case Defines.Phase.Discard1:
					ReturnCard(c);
					phase = Defines.Phase.Discard2;
					return true;
				case Defines.Phase.Discard2:
					ReturnCard(c);
					phase = Defines.Phase.ChooseCard;
					turn = NextTurn;
					return true;
			}
			return false;
		}
		//フラッグにある自分の札が選択された
		public bool SelectCardInMyFlag(Card c, Flag f)
		{
			if(f.IsProved)
				return false;
			if(phase == Defines.Phase.Redeploy1)
			{
				pCard = c;
				RemoveCard(c);
				phase = Defines.Phase.Redeploy2;
				return true;
			}
			return false;		
		}
		//フラッグにある相手の札が選択された
		public bool SelectCardInYourFlag(Card c, Flag f)
		{
			if(f.IsProved)
				return false;
			switch(phase)
			{
				case Defines.Phase.Deserter:
					TrashCard(c);
					phase = Defines.Phase.ChooseStack;
					return true;
				case Defines.Phase.Traitor1:
					if(c.Name == Defines.Tactics.TROOP)
					{
						pCard = c;
						RemoveCard(c);
						phase = Defines.Phase.Traitor2;
						return true;
					}
					break;
			}
			return false;		
		}
		//フラッグが選択された
		public bool SelectFlag(Flag fg)
		{
			Flag f = flag[fg.Index];	//応急処置
			switch(phase)
			{
				case Defines.Phase.ChooseFlag:
					if(f.IsProved)
						return false;
					if(PutCard(Turn, pCard, f))
					{
						if(pCard.Name != Defines.Tactics.TROOP) 
							numTCard[turn]++;
						if(pCard.Name == Defines.Tactics.DARIUS || pCard.Name == Defines.Tactics.ALEXANDER)
							useLeader[turn] = true;
						phase = Defines.Phase.ChooseStack;
						hand[turn].Remove(pCard.SortKey);
						pCard = null;
						return true;
					}
					break;
				case Defines.Phase.Redeploy2:
				case Defines.Phase.Traitor2:
					if(PutCard(Turn, pCard, f))
					{
						phase = Defines.Phase.ChooseStack;
						pCard = null;
						return true;
					}
					break;
				case Defines.Phase.ChooseCard:
				case Defines.Phase.ChooseStack:
					return ProveFlag(f);			// 証明判定
			}
			return false;
		}
		// リセット
		public bool ResetGame()
		{
			ClearObject();
			InitializeObjects();
			return true;
		}
		// 選択の解除
		public bool Undo()
		{
			if(phase == Defines.Phase.ChooseFlag && pCard != null)
			{
				pCard = null;
				phase = Defines.Phase.ChooseCard;
				return true;
			}
			return false;
		}
		// 次のターンへ
		public bool GoToNext()
		{
			switch(phase)
			{
				case Defines.Phase.ChooseStack:
					if(IsStackEmpty)
					{
						turn = NextTurn;
						phase = Defines.Phase.ChooseCard;
						return true;
					}
					return false;
				case Defines.Phase.ChooseCard:
					if(!CanPut(turn))
					{
						turn = NextTurn;
						phase = Defines.Phase.ChooseCard;
						return true;						
					}
					return false;
			}
			return false;
		}
		// デバック用　手札を入れ替え
		public void Redraw()
		{
			if(phase == Defines.Phase.ChooseCard)
			{
				while(hand[Turn].Count != 0)
				{
					ReturnCard((Card)hand[Turn].GetByIndex(0));
				}
				Shuffle(combatStack, 100);
				Shuffle(tacticsStack, 100);
				Draw(Turn, tacticsStack);
				for(int i=1; i<Defines.MaxHand; i++)
				{
					Draw(Turn, combatStack);
				}
			}
		}
	}
}
