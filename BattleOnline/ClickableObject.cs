using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetgameProj
{
	/// <summary>
	/// クリックイベントを処理できるコントロール
	/// 面倒なのでpictureBoxを継承
	/// </summary>

	public class ClickableObject : PictureBox
	{
		virtual public string GetCaption()
		{
			return "未継承のコントロールです";
		}
		virtual public bool PerformClickEvent(Game g)
		{
			return false;
		}
		// センド用のデータを取得する
		virtual public Object GetData()
		{
			return "No Data";
		}
		public bool Equals(ClickableObject obj)
		{
			// 名前と表示場所が一致していれば同一オブジェクトと判断される
			return (Location == obj.Location && Name == obj.Name);			
		}
	}

    // カードの絵をしたオブジェクト
	public class CardObject : ClickableObject
	{
		private Card card;

		public CardObject(Point pt, Card c, bool canView)
		{
			this.Location= pt;
			this.Name = c.ImageName1;
			this.Size = new System.Drawing.Size(Defines.CardSizeX, Defines.CardSizeY);
			if(canView)
			{
				Image = ResouceManager.GetImage(c.ImageName1);
			}
			else
			{
				if(c.Name == Defines.Tactics.TROOP)
				{
					Image = ResouceManager.GetImage(Defines.ImageName.BlackCard);
				}
				else
				{
					Image = ResouceManager.GetImage(Defines.ImageName.BlackTCard);
				}
			}
			card = c;
			
		}
		override public string GetCaption()
		{
			if(card.Name == Defines.Tactics.TROOP)
			{
				int c = ((CCard)card).Color;
				int n = ((CCard)card).Number;
				return "トループカード："+Defines.CaptionColor[c]+Defines.CaptionNumber[n];
			}
			else
			{
				return Defines.CaptionTactics[(int)card.Name];
			}
		}
		//クリックしたときに「起きるべき」動作をする
		//クライアントオブジェクトからは、レシーブイベントに対応して呼び出す
		override public bool PerformClickEvent(Game g)
		{
			return g.SelectCard(card);
		}
		override public Object GetData()
		{
			return card;
		}
	}
	//フラッグの絵をしたオブジェクト
	public class FlagObject : ClickableObject
	{
		private Flag flag;

		public FlagObject(Point pt, Flag f)
		{
			this.Location= pt;
			this.Name = "FlagObject"+f.Index.ToString();
			this.Size = new System.Drawing.Size(64, 64);
			Image = ResouceManager.GetImage(Defines.ImageName.Flag);
			flag = f;
			
		}
		override public string GetCaption()
		{
            return (flag.Index +1).ToString()+Defines.CaptionFlag;
		}
	
		override public bool PerformClickEvent(Game g)
		{
			return g.SelectFlag(flag);
		}
		override public Object GetData()
		{
			return flag;
		}
	}
	//世界に一個しかないようなオブジェクトはまとめて処理
	public class OneObject : ClickableObject
	{
		private EventData data;
		public OneObject(Point pt, string imageName, Size s)
		{
			this.Location = pt;
			this.Name = imageName;
            this.Size = s;
			data = new EventData(imageName);
        	Image = ResouceManager.GetImage(imageName);
		}
		override public string GetCaption()
		{
			switch(Name)
			{
				case Defines.ImageName.TroopStack:
					return "トループカードを引く";
				case Defines.ImageName.TacticsStack:
					return "タクティクスカードを引く";
				case Defines.ImageName.LabelUse:
					return "タクティクスカード「偵察」「転進」「裏切り」「脱走」を使用する。";
				case Defines.ImageName.LabelDestroy:
					return "「転進」の効果で選択されたカードを破棄する。";
				case Defines.ImageName.LabelReset:
					return "＊注意＊　ゲーム状態をリセットする。";
				case Defines.ImageName.LabelNext:
					return "相手のターンへ進む。\n（山札が空か、置く場所がないとき）";
				case Defines.ImageName.EffFog:
					return Defines.CaptionTactics[(int)Defines.Tactics.FOG];
				case Defines.ImageName.EffMud:
					return Defines.CaptionTactics[(int)Defines.Tactics.MUD];
			}
            return "";
		}
		override public bool PerformClickEvent(Game g)
		{
            return PerformEvent(g, data.data);
		}

		static public bool PerformEvent(Game g, string s)
		{
			switch(s)
			{
				case Defines.ImageName.TroopStack:
					return g.SelectStack(Defines.StackType.Troop);
				case Defines.ImageName.TacticsStack:
					return g.SelectStack(Defines.StackType.Tactics);
				case Defines.ImageName.LabelUse:
					return g.SelectUse();
				case Defines.ImageName.LabelDestroy:
					return g.SelectGraveyard();
				case Defines.ImageName.LabelReset:
					return g.ResetGame();
				case Defines.ImageName.LabelNext:
					return g.GoToNext();
			}
			return false;		
		}
		override public Object GetData()
		{
			return data;
		}
	}
	
	//シリアル化可能なダミークラス
	[Serializable()]
	class EventData
	{
		public string data;
		public EventData(string s)
		{
			data = s;
		}
	}
}
