using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Globalization;

namespace NetgameProj
{
	/// <summary>
	/// Form1 の概要の説明です。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Encoding ASCII = Encoding.ASCII;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.RadioButton radioBt_server;
		private System.Windows.Forms.RadioButton radioBt_client;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button3;

		private EchoServer serverManager = new EchoServer();
		private ClientManager clientManager = new ClientManager();
		private GameMainForm gameMainForm;

        delegate void SetTextCallback(string str);

        public Form1()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
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
			if(gameMainForm != null)
			{
				gameMainForm.Dispose();
			}
			KillProcesses();
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioBt_server = new System.Windows.Forms.RadioButton();
            this.radioBt_client = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 8);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(384, 176);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(336, 200);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(40, 19);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "2555";
            // 
            // radioBt_server
            // 
            this.radioBt_server.Location = new System.Drawing.Point(24, 200);
            this.radioBt_server.Name = "radioBt_server";
            this.radioBt_server.Size = new System.Drawing.Size(72, 24);
            this.radioBt_server.TabIndex = 8;
            this.radioBt_server.Text = "server";
            this.radioBt_server.CheckedChanged += new System.EventHandler(this.radioBt_server_CheckedChanged);
            // 
            // radioBt_client
            // 
            this.radioBt_client.Location = new System.Drawing.Point(24, 240);
            this.radioBt_client.Name = "radioBt_client";
            this.radioBt_client.Size = new System.Drawing.Size(72, 24);
            this.radioBt_client.TabIndex = 9;
            this.radioBt_client.Text = "client";
            this.radioBt_client.CheckedChanged += new System.EventHandler(this.radioBt_client_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(296, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Start";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(104, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "IP:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(296, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "port:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(136, 240);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(144, 19);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = "localhost";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(336, 240);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(40, 19);
            this.textBox3.TabIndex = 15;
            this.textBox3.Text = "2555";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(304, 280);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "Debug Start";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(416, 325);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioBt_client);
            this.Controls.Add(this.radioBt_server);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Monitor Window";
            this.Closed += new System.EventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		public void DrawText(string str)
		{
			richTextBox1.AppendText(str);
		}

        public void DrawTextSafe(string str)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(DrawTextSafe);
                this.BeginInvoke(d, new object[] { str });
            }
            else
            {
                richTextBox1.AppendText(str);
            }
        }

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			richTextBox1.AppendText("Battle Online ver."+Defines.version.ToString()+"\n");
		}

		private void KillProcesses()
		{
			clientManager.Disconnect();
			serverManager.StopListening();
		}

		private void StartAsServer()
		{
			
			serverManager.SetOutputMethod(new EchoServer.OutputCallback(DrawTextSafe));
			serverManager.StartListening(Convert.ToInt32(textBox1.Text),2);
			DrawText("Listening thread started\n");	
			clientManager.Connect("localhost", Convert.ToInt32(textBox1.Text));
			DrawText("Local client connected to the server\n");	
			button1.Text = "Server Stop";
		}
		private void StopServer()
		{
			clientManager.Disconnect();
			serverManager.StopListening();
			DrawText("Listening thread stopped and local client disconnected\n");
			button1.Text = "Start";
		}
		private void StartAsClient()
		{
					
			clientManager.Connect(textBox2.Text,Convert.ToInt32(textBox3.Text));
			DrawText("Client connected to " + textBox2.Text + ":" + textBox3.Text + "\n");
			button1.Text = "Client Stop";
		
		}
		private void StopClient()
		{
			clientManager.Disconnect();
			DrawText("Client disconnected\n");
			button1.Text = "Start";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if(button1.Text == "Start")
			{
				if(radioBt_client.Checked)
				{
					try
					{
						StartAsClient();
						StartGame(false);
					}
					catch
					{
						DrawText("failed to connect\n");
					}
				} 
				else if(radioBt_server.Checked)
				{
					try
					{
						StartAsServer();
						StartGame(true);
					}
					catch
					{
						DrawText("failed to start server\n");
					}
				}
			} 
			else if(button1.Text == "Server Stop")
			{
				StopServer();
				if(gameMainForm != null)
					gameMainForm.Close();
			}
			else if(button1.Text == "Client Stop")
			{
				StopClient();
				if(gameMainForm != null)
					gameMainForm.Close();
			}
			this.Invalidate();
		}

		private void radioBt_server_CheckedChanged(object sender, System.EventArgs e)
		{
//			textBox1.ReadOnly = !radioBt_server.Checked;   
            textBox1.Enabled = radioBt_server.Checked;
        }

		private void radioBt_client_CheckedChanged(object sender, System.EventArgs e)
		{
            //			textBox2.ReadOnly = !radioBt_client.Checked;
            //			textBox3.ReadOnly = !radioBt_client.Checked;
            textBox2.Enabled = radioBt_client.Checked;
            textBox3.Enabled = radioBt_client.Checked;
        }

        private void button3_Click(object sender, System.EventArgs e)
		{
			if(gameMainForm == null)
			{
				gameMainForm = new GameMainForm();
				//gameMainForm = new NetGameForm();
				gameMainForm.Start(true);
				gameMainForm.Closed += new EventHandler(gameMainForm_Close);
			}
		}

		private void gameMainForm_Close(Object sender, EventArgs e)
		{
			gameMainForm = null;
		}

		private void StartGame(bool isServer)
		{
			if(gameMainForm == null)
			{
				gameMainForm = new NetGameForm();
				((NetGameForm)gameMainForm).Start(clientManager, isServer);
				gameMainForm.Closed += new EventHandler(gameMainForm_Close);				
			}
		}
	}
}
