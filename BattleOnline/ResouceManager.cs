using System;
using System.Collections;
using System.Drawing;

namespace NetgameProj
{
	/// <summary>
	/// ビットマップリソースの管理をします。
	/// </summary>
	public class ResouceManager
	{
        static Hashtable bitmapFileHolder = new Hashtable();
		static Hashtable imageHolder = new Hashtable();

		public ResouceManager()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		//ビットマップファイルをロードする
		static public void LoadBitmap(string filename)
		{
			if(bitmapFileHolder.ContainsKey(filename))
				return;
            bitmapFileHolder.Add(filename,new Bitmap(filename));			
		}

		//ビットマップイメージから部分イメージを作成し、名前をつける
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
        
        //ファイルから生成したビットマップイメージを取得する
		static public Image GetFullImage(string filename)
		{
			LoadBitmap(filename);
            return (Image)bitmapFileHolder[filename];
		}

		//名前をつけられた部分イメージを取得する
		static public Image GetImage(string name)
		{
			return (Image)imageHolder[name];       
		}

		//解放処理
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
