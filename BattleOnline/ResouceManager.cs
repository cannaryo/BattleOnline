using System;
using System.Collections;
using System.Drawing;

namespace NetgameProj
{
	/// <summary>
	/// �r�b�g�}�b�v���\�[�X�̊Ǘ������܂��B
	/// </summary>
	public class ResouceManager
	{
        static Hashtable bitmapFileHolder = new Hashtable();
		static Hashtable imageHolder = new Hashtable();

		public ResouceManager()
		{
			// 
			// TODO: �R���X�g���N�^ ���W�b�N�������ɒǉ����Ă��������B
			//
		}

		//�r�b�g�}�b�v�t�@�C�������[�h����
		static public void LoadBitmap(string filename)
		{
			if(bitmapFileHolder.ContainsKey(filename))
				return;
            bitmapFileHolder.Add(filename,new Bitmap(filename));			
		}

		//�r�b�g�}�b�v�C���[�W���畔���C���[�W���쐬���A���O������
		static public void NamePartialImage(string name, string filename, int x, int y, int w, int h)
		{
			LoadBitmap(filename);
			Image src=(Image)bitmapFileHolder[filename];
            Image dest= new Bitmap(w,h);
			using(Graphics gs = Graphics.FromImage(dest))
			{
				Rectangle rc = new Rectangle(x,y,w,h);
				gs.DrawImage(src,0,0,rc, GraphicsUnit.Pixel);
			}
			if(imageHolder.ContainsKey(name))
			{
				((Image)imageHolder[name]).Dispose();
				imageHolder[name]=dest;
			}
			else
			{
				imageHolder.Add(name,dest);
			}
		}
        
        //�t�@�C�����琶�������r�b�g�}�b�v�C���[�W���擾����
		static public Image GetFullImage(string filename)
		{
			LoadBitmap(filename);
            return (Image)bitmapFileHolder[filename];
		}

		//���O������ꂽ�����C���[�W���擾����
		static public Image GetImage(string name)
		{
			return (Image)imageHolder[name];       
		}

		//�������
		static public void Dispose()
		{
			foreach(Image i in bitmapFileHolder.Values)
			{
				i.Dispose();	
			}
			bitmapFileHolder.Clear();
			foreach(Image i in imageHolder.Values)
			{
				i.Dispose();	
			}
			imageHolder.Clear();
		}
	}
}
