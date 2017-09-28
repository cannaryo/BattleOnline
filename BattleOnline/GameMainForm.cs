using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NetgameProj
{
	/// <summary>
	/// ゲームのメインフォーム 基本型
	/// </summary>
	public class GameMainForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected Game gs = null;
		private bool isDebug = false;
		protected bool isStart = false;
		private ArrayList pastList = new ArrayList();	// 以前に表示しているコントロールのリスト
		//どっちのプレイヤーか
		protected int mySide;
		protected int yourSide;
		//表示する座標 readonly意味ないかも？
		private readonly Point [] myHandPt = new Point[9];
		private readonly Point [] yourHandPt = new Point[9];
		private readonly Point [] flagPt = new Point[9];
		private readonly int [] myCPtY = new int[4];
		private readonly int [] yourCPtY = new int[4];
		private readonly Point [] effectPt = new Point[2];
		private readonly Point [] graveyardPt = new Point[7];
		private readonly Point [] etcPt = new Point[9];
		//説明とかのキャプション
		private System.Windows.Forms.Label phaseCaption;
		private System.Windows.Forms.Label objCaption;
		private System.Windows.Forms.Button debugBt1;
		private System.Windows.Forms.Label labelC;
		private System.Windows.Forms.Label labelT;
		private System.Windows.Forms.Button debugBt2;

		public GameMainForm()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			LoadResources();
			SetPoint();
			WinnerChecker.Initialize();
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			DisposeGameObject();		
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.phaseCaption = new System.Windows.Forms.Label();
            this.objCaption = new System.Windows.Forms.Label();
            this.debugBt1 = new System.Windows.Forms.Button();
            this.debugBt2 = new System.Windows.Forms.Button();
            this.labelC = new System.Windows.Forms.Label();
            this.labelT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // phaseCaption
            // 
            this.phaseCaption.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.phaseCaption.Location = new System.Drawing.Point(0, 0);
            this.phaseCaption.Name = "phaseCaption";
            this.phaseCaption.Size = new System.Drawing.Size(544, 16);
            this.phaseCaption.TabIndex = 0;
            // 
            // objCaption
            // 
            this.objCaption.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objCaption.Location = new System.Drawing.Point(0, 488);
            this.objCaption.Name = "objCaption";
            this.objCaption.Size = new System.Drawing.Size(448, 48);
            this.objCaption.TabIndex = 1;
            // 
            // debugBt1
            // 
            this.debugBt1.BackColor = System.Drawing.SystemColors.Control;
            this.debugBt1.Enabled = false;
            this.debugBt1.Location = new System.Drawing.Point(456, 466);
            this.debugBt1.Name = "debugBt1";
            this.debugBt1.Size = new System.Drawing.Size(56, 24);
            this.debugBt1.TabIndex = 2;
            this.debugBt1.Text = "Shuffle!";
            this.debugBt1.UseVisualStyleBackColor = false;
            this.debugBt1.Visible = false;
            this.debugBt1.Click += new System.EventHandler(this.debugBt1_Click);
            // 
            // debugBt2
            // 
            this.debugBt2.BackColor = System.Drawing.SystemColors.Control;
            this.debugBt2.Enabled = false;
            this.debugBt2.Location = new System.Drawing.Point(456, 496);
            this.debugBt2.Name = "debugBt2";
            this.debugBt2.Size = new System.Drawing.Size(56, 24);
            this.debugBt2.TabIndex = 5;
            this.debugBt2.Text = "Update";
            this.debugBt2.UseVisualStyleBackColor = false;
            this.debugBt2.Visible = false;
            this.debugBt2.Click += new System.EventHandler(this.debugBt2_Click);
            // 
            // labelC
            // 
            this.labelC.Location = new System.Drawing.Point(544, 24);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(24, 24);
            this.labelC.TabIndex = 6;
            this.labelC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelT
            // 
            this.labelT.Location = new System.Drawing.Point(544, 96);
            this.labelT.Name = "labelT";
            this.labelT.Size = new System.Drawing.Size(24, 24);
            this.labelT.TabIndex = 7;
            this.labelT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameMainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(630, 531);
            this.Controls.Add(this.labelT);
            this.Controls.Add(this.labelC);
            this.Controls.Add(this.debugBt2);
            this.Controls.Add(this.debugBt1);
            this.Controls.Add(this.objCaption);
            this.Controls.Add(this.phaseCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameMainForm";
            this.Text = "GameMainForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownEvent);
            this.ResumeLayout(false);

		}
		#endregion

		virtual public void Start(bool debug)
		{
			if(isStart == false)
			{
				isDebug = debug;
				InitializeGameObject();
				this.Show();
			}
		}
		virtual protected void InitializeGameObject()
		{
			gs = new Game();
			mySide = 0;
			isStart = true;
			debugBt1.Visible = isDebug;
            debugBt1.Enabled = isDebug;
            debugBt2.Visible = isDebug;
            debugBt2.Enabled = isDebug;
            yourSide = 1;
			UpdateAll();
		}
 
		private void LoadResources()
		{
			try
			{
				ResouceManager.LoadBitmap(Defines.FileCardImage);
				ResouceManager.LoadBitmap(Defines.FileTacticsImage);
				ResouceManager.LoadBitmap(Defines.FileEtcImage);
			
				int cx = Defines.CardSizeX;
				int cy = Defines.CardSizeY;
				for(int j=0; j<6; j++)
				{
					for(int i=0; i<10; i++)
					{
						string tail=(i+1).ToString()+"_"+j.ToString();
						ResouceManager.NamePartialImage("card_"+tail,Defines.FileCardImage,i*cx,j*cy,cx,cy);
					}
				}
				for(int i=0; i<Defines.NumberOfTactics; i++)
				{
					string tail = i.ToString();
					ResouceManager.NamePartialImage("Tcard_"+tail,Defines.FileTacticsImage,i*cx,0,cx,cy);
				}
				ResouceManager.NamePartialImage(Defines.ImageName.BlackTCard, Defines.FileEtcImage, 0, 0, cx, cy);
				ResouceManager.NamePartialImage(Defines.ImageName.BlackCard, Defines.FileEtcImage, 0, 64, cx, cy);
				ResouceManager.NamePartialImage(Defines.ImageName.TacticsStack, Defines.FileEtcImage, 0, 128, cx, cy);
				ResouceManager.NamePartialImage(Defines.ImageName.TroopStack, Defines.FileEtcImage, 0, 192, cx, cy);
				ResouceManager.NamePartialImage(Defines.ImageName.Flag, Defines.FileEtcImage, 0, 256, 64, 64);
				ResouceManager.NamePartialImage(Defines.ImageName.LabelUse, Defines.FileEtcImage, 64, 0, 64, 32);
				ResouceManager.NamePartialImage(Defines.ImageName.LabelDestroy, Defines.FileEtcImage, 64, 32, 64, 32);
				ResouceManager.NamePartialImage(Defines.ImageName.LabelWin, Defines.FileEtcImage, 64, 64, 64, 32);	
				ResouceManager.NamePartialImage(Defines.ImageName.LabelLose, Defines.FileEtcImage, 64, 96, 64, 32);	
				ResouceManager.NamePartialImage(Defines.ImageName.LabelNext, Defines.FileEtcImage, 64, 128, 64, 32);	
				ResouceManager.NamePartialImage(Defines.ImageName.LabelReset, Defines.FileEtcImage, 64, 160, 64, 32);	
				ResouceManager.NamePartialImage(Defines.ImageName.SelectBox, Defines.FileEtcImage, 128, 0, 96, 96);	
				ResouceManager.NamePartialImage(Defines.ImageName.EffFog, Defines.FileEtcImage, 64, 256, 32, 16);	
				ResouceManager.NamePartialImage(Defines.ImageName.EffMud, Defines.FileEtcImage, 64, 272, 32, 16);
			}
			catch
			{
				MessageBox.Show("ファイルの読み込みに失敗しました。アプリケーションを終了してください。");
			}
		}

		private void SetPoint()
		{
			for(int i=0; i<9; i++)
			{
				myHandPt[i] = new Point(5+60*i,422);
				yourHandPt[i] = new Point(5+60*i,18);
				flagPt[i] = new Point(12+60*i,220);
			}
			for(int i=0; i<4; i++)
			{
				myCPtY[i] = 284+i*24;
				yourCPtY[i] = 156-i*24;
			}
			for(int i=0; i<7; i++)
			{
				graveyardPt[i] = new Point(565,286+i*15);
			}

			effectPt[0] = new Point(36, 24);	// 「効果」を位置(フラッグに位置に対する増分)
			effectPt[1] = new Point(36, 42);	// 「効果」を置く位置(フラッグに位置に対する増分)
			etcPt[0] = new Point(565,10);	// トループスタック
			etcPt[1] = new Point(565,80);	// タクティクススタック
			etcPt[2] = new Point(565,150);	//	「リセット」ラベル
			etcPt[3] = new Point(565,184);	//	「ネクスト」ラベル
			etcPt[4] = new Point(565,218);	// 「使用」ラベル
			etcPt[5] = new Point(565,252);	// 「破棄」ラベル
			etcPt[6] = new Point(0,24);		//	「勝利」「敗北」ラベル(フラッグに対する増分)
			etcPt[7] = new Point(540,430);	//	セレクトボックス
			etcPt[8] = new Point(556,450);	//	セレクトボックス（内側）
		}

		protected void DisposeGameObject()
		{
			ResouceManager.Dispose();
		}
		//ゲーム状態によって変わるオブジェクトを更新する
        protected void UpdateChanged()
		{
			ArrayList addList = new ArrayList();
			ArrayList deleteList = new ArrayList();
			// 自分の手札を並べる
			int i=0;
			foreach(Card c in gs.GetHand(mySide))
			{
				CardObject obj = new CardObject(myHandPt[i], c, true);
				obj.MouseEnter += new EventHandler(ShowCaption);
				obj.MouseLeave += new EventHandler(HideCaption);
				obj.MouseDown += new MouseEventHandler(MouseDownEvent);
				addList.Add(obj);
				i++;
			}
			// 相手の手札を並べる
			i=0;
			foreach(Card c in gs.GetHand(yourSide))
			{
				
				CardObject obj = new CardObject(yourHandPt[i], c, isDebug);
				if(isDebug)
					obj.MouseDown += new MouseEventHandler(MouseDownEvent);
				addList.Add(obj);			
				i++;			
			}
			// 場にあるカードとエフェクトを並べる
			for(i=0; i<Defines.NumOfFlag; i++)
			{
				Flag f = gs.GetFlag(i);
				int j=0;
				foreach(Card c in f.GetCard(mySide))
				{
					Point pt = new Point(flagPt[i].X, myCPtY[j]);			
					CardObject obj = new CardObject(pt, c, true);
					obj.MouseEnter += new EventHandler(ShowCaption);
					obj.MouseLeave += new EventHandler(HideCaption);
					obj.MouseDown += new MouseEventHandler(MouseDownEvent);
					addList.Add(obj);			
					j++;
				}
				j=0;
				foreach(Card c in f.GetCard(yourSide))
				{
					Point pt = new Point(flagPt[i].X, yourCPtY[j]);
					CardObject obj = new CardObject(pt, c, true);
					obj.MouseEnter += new EventHandler(ShowCaption);
					obj.MouseLeave += new EventHandler(HideCaption);
					obj.MouseDown += new MouseEventHandler(MouseDownEvent);
					addList.Add(obj);	
					j++;
				}
				if(f.Winner == mySide)
				{
					Point pt=new Point(etcPt[6].X+flagPt[i].X, etcPt[6].Y+flagPt[i].Y);
					OneObject obj = new OneObject(pt, Defines.ImageName.LabelWin, new Size(60,28));
					addList.Add(obj);
				}
				else if(f.Winner == yourSide)
				{
					Point pt=new Point(etcPt[6].X+flagPt[i].X, etcPt[6].Y+flagPt[i].Y);
					OneObject obj = new OneObject(pt, Defines.ImageName.LabelLose, new Size(60,28));
					addList.Add(obj);					
				}
				else
				{
					j=0;
					foreach(string s in f.GetEffect())
					{
						Point pt=new Point(effectPt[j].X+flagPt[i].X, effectPt[j].Y+flagPt[i].Y);
						OneObject obj = new OneObject(pt, s, new Size(32,16));
						obj.MouseEnter += new EventHandler(ShowCaption);
						obj.MouseLeave += new EventHandler(HideCaption);
						addList.Add(obj);
						j++;
					}
				}
			}
			// グレイブヤードのカードを並べる
			i=0;
			foreach(Card c in gs.GetGraveyard())
			{		
				CardObject obj = new CardObject(graveyardPt[i],c,true);
				obj.MouseEnter += new EventHandler(ShowCaption);
				obj.MouseLeave += new EventHandler(HideCaption);
				addList.Add(obj);
				i++;				
			}
			//選択されているカード
			if( gs.Turn == mySide && gs.PresentCard != null)
			{	
				CardObject obj = new CardObject(etcPt[8], gs.PresentCard, true);
				addList.Add(obj);	
			}

			//作成したオブジェクトのうち更新が必要なものだけを追加や削除する
			foreach(ClickableObject obj in pastList)
			{
				ClickableObject t;
				if((t = GetEqualObj(addList, obj)) != null)
				{
					addList.Remove(t);
				}
				else
				{
					deleteList.Add(obj);
				}
			}
			foreach(ClickableObject obj in addList)
			{
				pastList.Add(obj);
				this.Controls.Add(obj);
				obj.BringToFront();
			}
			foreach(ClickableObject obj in deleteList)
			{
				pastList.Remove(obj);
				this.Controls.Remove(obj);
			}
            if (gs.Turn == mySide)
            {
                if (gs.Phase == Defines.Phase.FinishGame)
                {
                    phaseCaption.Text = Defines.CaptionWin + Defines.CaptionPhase[(int)gs.Phase];
                }
                else
                {
                    phaseCaption.Text = "あなたのターンです。  " + Defines.CaptionPhase[(int)gs.Phase];
                }
            }
            else
            {
                if (gs.Phase == Defines.Phase.FinishGame)
                {
                    phaseCaption.Text = Defines.CaptionLose + Defines.CaptionPhase[(int)gs.Phase];
                }
                else
                {
                    phaseCaption.Text = "相手のターンです。";
                }
            }
            
            labelC.Text = gs.RestCCard.ToString();
			labelT.Text = gs.RestTCard.ToString();
		}

		internal ClickableObject GetEqualObj(ArrayList arr, ClickableObject obj)
		{
			foreach(ClickableObject t in arr)
			{
				if(obj.Equals(t))
					return t;
			}
			return null;
		}

		//一度だけ作成されるオブジェクトを更新する
		protected void UpdateStaticObject()
		{
			this.Controls.Add(phaseCaption);
			this.Controls.Add(objCaption);
			this.Controls.Add(debugBt1);
			this.Controls.Add(debugBt2);
			this.Controls.Add(labelC);
			this.Controls.Add(labelT);

			// フラッグを並べる
			for(int i=0; i<Defines.NumOfFlag; i++)
			{
				Flag f = gs.GetFlag(i);
				FlagObject obj = new FlagObject(flagPt[i], f);
				obj.MouseEnter += new EventHandler(ShowCaption);
				obj.MouseLeave += new EventHandler(HideCaption);
				obj.MouseDown += new MouseEventHandler(MouseDownEvent);
				this.Controls.Add(obj);
			}
			// ボタンとかを並べる
			Size sz= new Size(Defines.CardSizeX,Defines.CardSizeY);
			OneObject [] objList = 
			{
				new OneObject(etcPt[0],Defines.ImageName.TroopStack, sz),
				new OneObject(etcPt[1],Defines.ImageName.TacticsStack, sz),
//				new OneObject(etcPt[2],Defines.ImageName.LabelReset, new Size(64,32)),
				new OneObject(etcPt[3],Defines.ImageName.LabelNext, new Size(64,32)),
				new OneObject(etcPt[4],Defines.ImageName.LabelUse, new Size(64,32)),
				new OneObject(etcPt[5],Defines.ImageName.LabelDestroy, new Size(64,32)),
			};
			foreach(OneObject obj in objList)
			{
				obj.MouseEnter += new EventHandler(ShowCaption);
				obj.MouseLeave += new EventHandler(HideCaption);
				obj.MouseDown += new MouseEventHandler(MouseDownEvent);				
			}
			this.Controls.AddRange(objList);
            //セレクトボックス
			OneObject o = new OneObject(etcPt[7],Defines.ImageName.SelectBox, new Size(96,96));
			this.Controls.Add(o);
			//リセットスイッチ
			o = new OneObject(etcPt[2],Defines.ImageName.LabelReset, new Size(64,32));
			o.MouseEnter += new EventHandler(ShowCaption);
			o.MouseLeave += new EventHandler(HideCaption);
			o.MouseDown += new MouseEventHandler(ResetEvent);	
			this.Controls.Add(o);
		}

		//全部更新する
		protected void UpdateAll()
		{
			this.Controls.Clear();
			pastList.Clear();
			UpdateStaticObject();
			UpdateChanged();
		}

		private void ShowCaption(Object sender, EventArgs e)
		{
			objCaption.Text = ((ClickableObject)sender).GetCaption();
		}
		private void HideCaption(Object sender, EventArgs e)
		{
			objCaption.Text = null;
		}
        // マウスが押されたときの処理
		virtual protected void MouseDownEvent(object sender, MouseEventArgs e)
		{
			//普通は自分のターンじゃないとなんもできない
			if( isStart && ( gs.Turn == mySide || isDebug) )
			{
				if(e.Button == MouseButtons.Right)
				{
					if(gs.Undo())
					{
						UpdateChanged();
					}
				}
				else if(sender.GetType().BaseType == typeof(NetgameProj.ClickableObject) )
				{
					if( ((ClickableObject)sender).PerformClickEvent(gs) )
					{
						UpdateChanged();
					}
				}
			}
		}

        // リセットが押されたときの処理
		virtual protected void ResetEvent(object sender, MouseEventArgs e)
		{
			if( isStart )
			{
				DialogResult r = MessageBox.Show("ゲームリセットしますか？","Confirmation",MessageBoxButtons.YesNo);
				if(r == DialogResult.Yes)
				{
					MouseDownEvent(sender, e);
				}
			}
		}

		private void debugBt1_Click(object sender, System.EventArgs e)
		{
			if(isDebug && isStart && gs != null)
			{
				gs.Redraw();
				UpdateChanged();
			}
		}

		private void debugBt2_Click(object sender, System.EventArgs e)
		{
			if(isStart && gs != null)
			{
				UpdateAll();
			}
		}
	}
}
