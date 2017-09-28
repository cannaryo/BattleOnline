using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NetgameProj
{
	/// <summary>
	/// ネットワークゲームのメインフォーム
	/// </summary>
	public class NetGameForm : GameMainForm
	{
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label labelMessage;
		private bool isServer = false;
		private System.Timers.Timer updateTimer;
		private System.Timers.Timer updateTimer2;
		private ClientManager client = null;

		public NetGameForm()
		{
			InitializeComponent();
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
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.labelMessage = new System.Windows.Forms.Label();
			this.updateTimer = new System.Timers.Timer();
			this.updateTimer2 = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.updateTimer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updateTimer2)).BeginInit();
			this.SuspendLayout();
			// 
			// labelMessage
			// 
			this.labelMessage.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.labelMessage.Location = new System.Drawing.Point(32, 48);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(560, 136);
			this.labelMessage.TabIndex = 2;
			// 
			// updateTimer
			// 
			this.updateTimer.Interval = 20;
			this.updateTimer.SynchronizingObject = this;
			this.updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.updateTimer_Elapsed);
			// 
			// updateTimer2
			// 
			this.updateTimer2.Interval = 20;
			this.updateTimer2.SynchronizingObject = this;
			this.updateTimer2.Elapsed += new System.Timers.ElapsedEventHandler(this.updateTimer2_Elapsed);
			// 
			// NetGameForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(632, 533);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelMessage});
			this.Name = "NetGameForm";
			this.Text = "Battle Online";
			((System.ComponentModel.ISupportInitialize)(this.updateTimer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updateTimer2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		override public void Start(bool debug)
		{
			// こっちは使わない
		}

		public void Start(ClientManager c, bool server)
		{
			if(isStart == false && client == null)
			{
				client = c;
				isServer = server;
				if(isServer)
				{
					mySide = 0;
					yourSide = 1;
					labelMessage.Text = "クライアントの接続を受け付けています";
					client.StartReadingThread(new ClientManager.streamReadCallback(procReceiveSafe));				
				}
				else
				{
					mySide = 1;
					yourSide = 0;
					labelMessage.Text = "サーバーとの同期を待っています。\nしばらくお待ちください。";
					client.StartReadingThread(new ClientManager.streamReadCallback(procReceiveSafe));
					client.SendSerializedObject(Defines.version);	//まずバージョン情報を送る
				}
				this.Show();
			}
		}

		override protected void InitializeGameObject()
		{
			labelMessage.Visible = false;
			isStart = true;
			updateTimer.Enabled = true;
		}

        private void procReceiveSafe(Object target)
        {
            if(this.InvokeRequired)
            {
                ClientManager.streamReadCallback str = new ClientManager.streamReadCallback(procReceive);
                this.BeginInvoke(str, new object[] { target });
            }
            else
            {
                procReceive(target);
            }
        }

        private void procReceive(Object target)
		{
			lock(this)
			{
				if( target.GetType() == typeof(string) )
					procString( (string)target);
				if( target.GetType() == typeof(NetgameProj.Game) )
					procGame( (Game)target);
				if( target.GetType() == typeof(System.Version) )
					procVersion( (Version)target);
				if( target.GetType() == typeof(NetgameProj.CCard) || target.GetType() == typeof(NetgameProj.TCard))
					procCard( (Card)target );
				if( target.GetType() == typeof(NetgameProj.Flag) )
					procFlag( (Flag)target );
				if( target.GetType() == typeof(NetgameProj.EventData) )
					procEvent( (EventData)target );
			}
			return;
		}

		private void procEvent(EventData d)
		{
			if(gs != null)
			{
				if( OneObject.PerformEvent(gs, d.data) )
				{
					updateTimer2.Enabled = true;
				}
			}
		}
		private void procCard(Card c)
		{
			if(gs != null)
			{
				if( gs.SelectCard(c) )
				{
					updateTimer2.Enabled = true;
				}
			}
		}
		private void procFlag(Flag f)
		{
			if(gs != null)
			{
				if( gs.SelectFlag(f) )
				{
					updateTimer2.Enabled = true;
				}
			}
		}
		private void procString( string s )
		{
			switch(s)
			{
				case "Undo":
					if(gs != null && gs.Undo()) 
					{
						gs.Undo();
						updateTimer2.Enabled = true;
					}
					break;
				case "Reset":
					if(isServer && gs != null)
					{
						isStart = false;
						gs.ResetGame();
						client.SendSerializedObject(gs);
					}
					else
					{
						isStart = false;
					}
					break;
				case "Update":
					if(isServer && gs != null)
					{
						isStart = false;
						client.SendSerializedObject(gs);
					}
					else
					{
						isStart = false;
					}
					break;
				case "Bad Version":
					labelMessage.Text = "ゲームのバージョンが一致しませんでした。\nアプリケーションを終了してください。";
					break;
			}
		}
		private void procVersion(Version v)
		{
			// マイナーバージョンまでが一致したら、ゲームを開始する
			if(isServer) 
			{
				if(v.Major == Defines.version.Major && v.Minor == Defines.version.Minor)				
				{
					Game g=new Game();
					client.SendSerializedObject(g);
				}
				else
				{
					client.SendSerializedObject("Bad Version");
				}
			}
		}
		private void procGame(Game g)
		{
			if(isStart == false)
			{
				gs = g;
				InitializeGameObject();
			}
		}

		override protected void MouseDownEvent(object sender, MouseEventArgs e)
		{
			//自分のターンじゃないとなんもできない
			if( isStart && gs.Turn == mySide )
			{
				if(e.Button == MouseButtons.Right)
				{
					client.SendSerializedObject("Undo");					
				}
				else if(sender.GetType().BaseType == typeof(NetgameProj.ClickableObject) )
				{
					client.SendSerializedObject( ((ClickableObject)sender).GetData() );
				}
			}
		}
		override protected void ResetEvent(object sender, MouseEventArgs e)
		{
			if( isStart )
			{
				DialogResult r = MessageBox.Show("ゲームリセットしますか？","Confirmation",MessageBoxButtons.YesNo);
				if(r == DialogResult.Yes)
				{
					client.SendSerializedObject("Reset");
				}
			}
		}

		private void updateTimer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			updateTimer.Enabled = false;
			if(isStart && gs != null)
			{
				lock(this)
				{
					UpdateChanged();
				}
			}
		}

		private void updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			updateTimer.Enabled = false;
			if(isStart && gs != null)
			{
				lock(this)
				{
					UpdateAll();
				}
			}
		}
	}
}
