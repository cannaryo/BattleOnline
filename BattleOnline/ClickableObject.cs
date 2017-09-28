using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetgameProj
{
	/// <summary>
	/// �N���b�N�C�x���g�������ł���R���g���[��
	/// �ʓ|�Ȃ̂�pictureBox���p��
	/// </summary>

	public class ClickableObject : PictureBox
	{
		virtual public string GetCaption()
		{
			return "���p���̃R���g���[���ł�";
		}
		virtual public bool PerformClickEvent(Game g)
		{
			return false;
		}
		// �Z���h�p�̃f�[�^���擾����
		virtual public Object GetData()
		{
			return "No Data";
		}
		public bool Equals(ClickableObject obj)
		{
			// ���O�ƕ\���ꏊ����v���Ă���Γ���I�u�W�F�N�g�Ɣ��f�����
			return (Location == obj.Location && Name == obj.Name);			
		}
	}

    // �J�[�h�̊G�������I�u�W�F�N�g
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
				return "�g���[�v�J�[�h�F"+Defines.CaptionColor[c]+Defines.CaptionNumber[n];
			}
			else
			{
				return Defines.CaptionTactics[(int)card.Name];
			}
		}
		//�N���b�N�����Ƃ��Ɂu�N����ׂ��v���������
		//�N���C�A���g�I�u�W�F�N�g����́A���V�[�u�C�x���g�ɑΉ����ČĂяo��
		override public bool PerformClickEvent(Game g)
		{
			return g.SelectCard(card);
		}
		override public Object GetData()
		{
			return card;
		}
	}
	//�t���b�O�̊G�������I�u�W�F�N�g
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
	//���E�Ɉ�����Ȃ��悤�ȃI�u�W�F�N�g�͂܂Ƃ߂ď���
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
					return "�g���[�v�J�[�h������";
				case Defines.ImageName.TacticsStack:
					return "�^�N�e�B�N�X�J�[�h������";
				case Defines.ImageName.LabelUse:
					return "�^�N�e�B�N�X�J�[�h�u��@�v�u�]�i�v�u���؂�v�u�E���v���g�p����B";
				case Defines.ImageName.LabelDestroy:
					return "�u�]�i�v�̌��ʂőI�����ꂽ�J�[�h��j������B";
				case Defines.ImageName.LabelReset:
					return "�����Ӂ��@�Q�[����Ԃ����Z�b�g����B";
				case Defines.ImageName.LabelNext:
					return "����̃^�[���֐i�ށB\n�i�R�D���󂩁A�u���ꏊ���Ȃ��Ƃ��j";
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
	
	//�V���A�����\�ȃ_�~�[�N���X
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
