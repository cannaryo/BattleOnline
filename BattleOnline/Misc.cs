using System;
using System.Collections;

namespace NetgameProj
{
	/// <summary>
	/// カードオブジェクト
	/// </summary>
	public interface Card
	{
		string ImageName1
		{
			get;
		}
		int SortKey
		{
			get;
		}
		Defines.Tactics Name
		{
			get;
		}
	}

	[Serializable()]
	public class CCard : Card
	{
		int num;
		int color;
		string imageName1;
		public CCard(int n, int c, string img_name)
		{
			num=n;
			color=c;
			imageName1 = img_name;
		}
		public string ImageName1
		{
			get { return imageName1; }
		}
		public int SortKey
		{
			get
			{
				return num + color * 100;
			}
		}
		public int Number
		{
			get { return num; }
		}
		public int Color
		{
			get { return color; }
		}
		public Defines.Tactics Name
		{
			get { return Defines.Tactics.TROOP; }
		}
	}

	[Serializable()]
	public class TCard : Card
	{
		string imageName1;
        Defines.Tactics name;
		public TCard(Defines.Tactics n, string img_name)
		{
			name=n;
			imageName1 = img_name;
		}
		public string ImageName1
		{
			get { return imageName1; }
		}
		public int SortKey
		{
			get
			{
				return 1000 + (int)name*100;
			}
		}
		public Defines.Tactics Name
		{
			get { return name; }
		}
	}

	/// <summary>
	/// フラッグオブジェクト
	/// </summary>
	[Serializable()]
	public class Flag
	{
		private bool isProved = false;
		public bool IsProved
		{
			get { return isProved; }
		}
		private SortedList [] cardList = {new SortedList(), new SortedList()};
		private ArrayList effectList = new ArrayList();
		private int maxNumber;
		public int MaxNumber
		{
			get { return maxNumber;}
		}
		private int index;
		public int Index
		{
			get { return index; }
		}
		private int winner = -1;
		public int Winner
		{
			get { return winner; }
		}

		public Flag(int i)
		{
			index = i;
			clear_it();
		}
		public void clear_it()
		{
			maxNumber = 3;
			isProved = false;
			winner = -1;
			effectList.Clear();
			foreach(SortedList c in cardList)
			{
				c.Clear();
			}
		}
		//フラッグにトループとして扱えるカードを追加する
		public void AddCard(Card c, int side)
		{
			cardList[side].Add(c.SortKey,c);
		}
		//フラッグに特殊カード効果（FOGとMUD）を加える
		public void AddEffect(TCard c)
		{
			switch(c.Name)
			{
				case Defines.Tactics.FOG:
					effectList.Add(Defines.ImageName.EffFog);
					break;
				case  Defines.Tactics.MUD:
					effectList.Add(Defines.ImageName.EffMud);
					maxNumber = 4;
					break;
			}
		}
		//フラッグから任意のカードを取り除く
		public void RemoveCard(Card c)
		{
			/*FogとMudは取り除けないらしいよ
			switch(c.Name)
			{
				case Defines.Tactics.FOG:
					effectList.Remove(Defines.ImageName.EffFog);
					break;
				case  Defines.Tactics.MUD:
					effectList.Remove(Defines.ImageName.EffMud);
					maxNumber = 3;
					break;
			}
			*/
			for(int i=0; i<2; i++)
			{
				cardList[i].Remove(c.SortKey);
			}
		}
		public bool IsCard(Card c, int side)
		{
			return cardList[side].ContainsKey(c.SortKey);
		}
		public int GetLength(int side)
		{
			return cardList[side].Count;
		}
		public ICollection GetCard(int side)
		{
			return cardList[side].Values;
		}
		public ICollection GetEffect()
		{
			return effectList;
		}
		public bool IsFog
		{
			get { return effectList.Contains(Defines.ImageName.EffFog); }
		}
		public void Prove(int side)
		{
            isProved = true;
			winner = side;
		}
	}
}