using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NetgameProj
{
	/// <summary>
	/// �l�b�g���[�N�Q�[���̃��C���t�H�[��
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
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
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
			// �������͎g��Ȃ�
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
					labelMessage.Text = "�N���C�A���g�̐ڑ����󂯕t���Ă��܂�";
					client.StartReadingThread(new ClientManager.streamReadCallback(procReceiveSafe));				
				}
				else
				{
					mySide = 1;
					yourSide = 0;
					labelMessage.Text = "�T�[�o�[�Ƃ̓�����҂��Ă��܂��B\n���΂炭���҂����������B";
					client.StartReadingThread(new ClientManager.streamReadCallback(procReceiveSafe));
					client.SendSerializedObject(Defines.version);	//�܂��o�[�W�������𑗂�
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
					labelMessage.Text = "�Q�[���̃o�[�W��������v���܂���ł����B\n�A�v���P�[�V�������I�����Ă��������B";
					break;
			}
		}
		private void procVersion(Version v)
		{
			// �}�C�i�[�o�[�W�����܂ł���v������A�Q�[�����J�n����
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
			//�����̃^�[������Ȃ��ƂȂ���ł��Ȃ�
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
				DialogResult r = MessageBox.Show("�Q�[�����Z�b�g���܂����H","Confirmation",MessageBoxButtons.YesNo);
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
