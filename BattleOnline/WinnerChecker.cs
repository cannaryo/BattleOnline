using System;
using System.Collections;

namespace NetgameProj
{
	/// <summary>
	/// �t���b�O�l���������s��
	/// Game �I�u�W�F�N�g�Ɏ����������Ȃ��̂ŁA�X�^�e�B�b�N����
	/// </summary>
	public class WinnerChecker
	{
		static private int [] allCard = new int[60];
		static private int [] presentCard = new int[60];
		static private int [] tKey = new int[4];
		static private int numOfPresent = 60;

		static internal int MaxKey(int [] list)
		{
			int max = -99999;
			foreach(int i in list)
			{
				if(max < i)
					max = i;
			}
			return max;
		}
		static internal int MinKey(int [] list)
		{
			int min = 99999;
			foreach(int i in list)
			{
				if(i < min)
					min = i;
			}
			return min;
		}
		static internal bool IsSameNumber(int [] list)
		{
			int [] ls = GetNumber(list);
			return MaxKey(ls) == MinKey(ls);
		}
		static internal bool IsSameColor(int [] list)
		{
			return (MaxKey(list) - MinKey(list)) < 10;
		}
		static internal bool IsStreight(int [] list)
		{
			int [] ls = GetNumber(list);
			Array.Sort(ls);
			for(int i=0; i < ls.Length -1; i++)
			{
				if( ls[i]+1 != ls[i+1] )
					return false;
			}
			return true;
		}
		static internal int [] GetNumber(int [] list)
		{
			// �L�[�̐��������݂̂�Ԃ�
			int [] ls = new int[list.Length];
			for(int i =0; i<list.Length; i++)
			{
				ls[i] = list[i] % 100;
			}
			return ls;
		}
		static internal int GetPower(int [] list, bool isFog)
		{
			// �w�`�̃p���[���v�Z����
			int [] ls = GetNumber(list);
			int power = 0;
			for(int i=0; i<ls.Length; i++)
			{
				power += ls[i];
			}
			if(isFog)
				return power;
			if(IsSameNumber(list))
				power += 4000;
			else if(IsSameColor(list))
				power += 3000;
			if(IsStreight(list))
				power += 2000;
			return power;
		}
		static internal int [] ToIntArray(ICollection arr)
		{
			return ToIntArray(arr, arr.Count);
		}
		static internal int [] ToIntArray(ICollection arr, int length)
		{
			int [] ls = new int[length];
			int i=0;
			foreach(Card c in arr)
			{
				ls[i++] = c.SortKey;				
			}
			return ls;
		}
		static internal bool Contains(int[] list , int val)
		{
			foreach(int i in list)
			{
				if(i == val)
					return true;
			}
			return false;
		}

		// ��肤��g�ݍ��킹�̂����ő�p���[�̂���
		static internal int SubBiggest(int [] list, Array[] arr, int count, bool isFog)
		{
			int max = 0;
			if(count == list.Length)
				return GetPower(list,isFog);
			foreach(int i in arr[count])
			{
				list[count] = i;
				int tmp = SubBiggest(list, arr, count+1, isFog);
				if( max < tmp)
					max =tmp;
			}
			return max;
		}

		// tacticsKey[0] == SHIELD_BEARERS
		// tacticsKey[1] == COMPANION_CAVAERY
		// tacticsKey[2] == DARIUS
		// tacticsKey[3] == ALEXANDER
		// tactics�J�[�h���܂ނ����ׁA���̌��ʂ͈̔͂ōő�ɂȂ�悤�ȃp���[��Ԃ�
		static internal int BiggestPowOf(int [] list, bool isFog)
		{
			//��肤��g�ݍ��킹�̃��X�g�i�̔z��j���쐬����
			Array [] arr = new Array[list.Length];
			int [] ls = new int [list.Length];
			int c;
			if(MinKey(list) >= 1000)		//�S��tactics�J�[�h�̏ꍇ
				c = 100;
			else
				c = (list[0]/100)*100;	//�F�͈ꖇ�ڂɍ��킹�Ă������
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] == tKey[0] )
					arr[i] = new int[]{c+1, c+2, c+3};
				else if(list[i] == tKey[1] )
					arr[i] = new int[]{c+8};
				else if(list[i] == tKey[2] || list[i] == tKey[3])
					arr[i] = new int[]{c+1,c+2,c+3,c+4,c+5,c+6,c+7,c+8,c+9,c+10};
				else
					arr[i] = new int[]{list[i]};					
			}
			return SubBiggest(ls, arr, 0, isFog);
		}

		// ���傫�ȑg�����݂����邩���ׂ�
		// pl �͐��tactics�J�[�h���������ꍇ�̉��������Ă����K�v������
		static private bool IsBigger(int pl_pow, int [] opp, int index, int count, int max_num, bool isFog)
		{
			if(count == max_num) 
			{
				if( pl_pow < BiggestPowOf(opp,isFog) )
					return true;
				else
					return false;
			}
			for(int i=index; i<numOfPresent; i++)
			{
				opp[count] = presentCard[i];
				if( IsBigger(pl_pow,opp,i,count+1,max_num,isFog) )
				{
					return true;
				}
			}
			return false;
		}

		static private void MakePresentCard(int[] delete_list)
		{
			int i=0;
			foreach(int c in allCard)
			{
				if(! Contains(delete_list,c))
				{
					presentCard[i++] = c;	
				}
			}
			numOfPresent = i;
		}

		// �v�Z�p�̃f�[�^���쐬����
		static public void Initialize()
		{
			int p=0;
			for(int c=0; c < 6; c++)
			{
				for(int n=1; n<11; n++)
				{
					// �\�[�g�L�[�@���@���{�F�L�[�~�P�O�O
					allCard[p++] = n + c*100;
				}
			}
			tKey[0] = 1000 + 100*(int)Defines.Tactics.SHIELD_BEARERS;
			tKey[1] = 1000 + 100*(int)Defines.Tactics.COMPANION_CAVAERY;
			tKey[2] = 1000 + 100*(int)Defines.Tactics.DARIUS;
			tKey[3] = 1000 + 100*(int)Defines.Tactics.ALEXANDER;
		}

		// �t���b�O�l���������s��
		// �^�[���v���C���[(pl)�̏���	= 0
		// �^�[���v���C���[(pl)�̕���	= 1
		// �����ł��Ȃ�				= -1
		static public int FlagWinner(ICollection pl, ICollection opp, ICollection del, int cardmax, bool isFog)
		{
			if(pl.Count != cardmax)
				return -1;
			if( opp.Count == cardmax )
			{
				if(BiggestPowOf(ToIntArray(pl), isFog) < BiggestPowOf(ToIntArray(opp), isFog))
					return 1;
				else
					return 0;
			}
			// pl.Count == cardmax�� opp.Count < cardmax �ł���悤�ȂƂ�
			MakePresentCard( ToIntArray(del));
			int pl_pow=BiggestPowOf(ToIntArray(pl),isFog);
			if( !IsBigger(pl_pow, ToIntArray(opp, cardmax), 0, opp.Count, cardmax, isFog))
				return 0;
			return -1;
		}

        // �t���b�O�̊l���󋵂����������𖞂����Ă��邩�`�F�b�N����
        // �^�[���v���C���[(pl)�̏����Ȃ�true
        // ����ȊO�Ȃ�false
        static public bool GameWinner(Flag [] flags, int turn)
        {
            int n_win = 0;
            int n_seqwin = 0;
            int max_seqwin = 0;
            foreach(Flag f in flags)
            {
                if(f.Winner == turn)
                {
                    n_win += 1;
                    n_seqwin += 1;
                    max_seqwin = Math.Max(n_seqwin, max_seqwin);
                }
                else
                {
                    n_seqwin = 0;
                }
            }
            if(n_win >= Defines.CountForWin || max_seqwin >= Defines.SequencedCountForWin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
