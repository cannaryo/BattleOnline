using System;
using System.Net;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace NetgameProj
{
	/// <summary>
	/// 単純なエコーサーバースレッドを立ち上げます
	/// </summary>
	public class EchoServer
	{
		public delegate void OutputCallback(string text);
		
		private Hashtable streamHolder = new Hashtable();      
		private Hashtable threadHolder = new Hashtable();
		private Thread tcpThd = null;
		private TcpListener tcpLsn = null;
		private static int connectId=0;
		private static int maxConnection=0;
		private OutputCallback output_callback = new OutputCallback(DefaultOutput);

		public EchoServer()
		{

		}

		// 接続を受付けるスレッドを開始します
		public void StartListening(int port, int max_connection)
		{
			lock(this)
			{
				if (tcpLsn != null)
					return;
                tcpLsn = new TcpListener(IPAddress.Any, port);
				maxConnection = max_connection;
			}
			tcpLsn.Start();
			output_callback("* Listen at: " + tcpLsn.LocalEndpoint.ToString() + "\n");
			tcpThd = new Thread(new ThreadStart(WaitingForClient));
			tcpThd.Name = "TcpListener";
			tcpThd.IsBackground = true;
			tcpThd.Start() ;
		}

		// すべてのスレッドとソケットを閉じます
		public void StopListening()
		{
            // Stop listening
            if (tcpLsn != null)
            {
                tcpLsn.Stop();
                lock (this)
                {
                    tcpLsn = null;
                }
            }
			
			// Stop listening thread
			if( tcpThd !=null && tcpThd.IsAlive )
			{
				tcpThd.Abort();
				tcpThd.Join();
			}

			// Close all sockets
			lock ( this )
			{
				foreach (NetworkStream s in streamHolder.Values) 
				{
					s.Close();
				}
			}
			
			// Stop all threads
			Hashtable threadHolderTmp;
			lock ( this ) 
			{
				threadHolderTmp = (Hashtable)threadHolder.Clone();
			}
			foreach (Thread t in threadHolderTmp.Values) 
			{
				if(t.IsAlive) 
				{
					t.Abort();
					t.Join();
				}
			}
		}

		// 情報の出力先を設定します
		public void SetOutputMethod(OutputCallback c)
		{
			output_callback = c;
		}

		private void WaitingForClient()                                                
		{
			try
			{
				while(tcpLsn != null)                                         
				{                                        
					// Accept will block until someone connects                       
					Socket sckt = tcpLsn.AcceptSocket();
					if (streamHolder.Count < maxConnection || maxConnection == 0 )                           
					{                                                                 
						while (streamHolder.Contains(connectId) )                   
						{                                                           
							if (connectId < 10000)                                            
								Interlocked.Increment(ref connectId);                       
							else                                                              
								Interlocked.Exchange(ref connectId, 1);                                              
						}
						lock(this)
						{
							output_callback("* Accept client (ID:" + connectId.ToString() + ")\n");
							NetworkStream srm = new NetworkStream(sckt, true);
							Thread td = new Thread(new ThreadStart(ReadSocket));
							// it is used to keep connected Sockets
							streamHolder.Add(connectId, srm);
							// it is used to keep the active thread
							threadHolder.Add(connectId, td);
							td.Name = "Socket:" + connectId.ToString();
							td.IsBackground = true;
							td.Start();					
						}
					}
					else
					{
						sckt.Shutdown(SocketShutdown.Both);
						sckt.Close();
					}				
				}
			}
			catch ( ThreadAbortException)
			{
				output_callback("*** Listening thread were aborted.\n");
			}			
			catch( Exception e)
			{
				output_callback("*** Listening thread exception\n" + e.ToString());
				if (tcpLsn != null) 
				{
					tcpLsn.Stop();
					tcpLsn = null;
				}
			}
		}
                                            
		private void ReadSocket()                                                      
		{                                                                             
			// the connectId is keeping changed with new connection added. it can't 
			// be used to keep the real connectId, the local variable realId will   
			// keep the value when the thread started.                              
			int realId = connectId;
			NetworkStream s = (NetworkStream)streamHolder[realId];       
			Byte[] receive = new Byte[256] ;
			try 
			{               
				while (s.CanRead)                                                            
				{                                            
					// Receive will block until data coming               
					// ret is 0 or Exception happen when Socket connection
					// is broken                     
					int ret = s.Read(receive,0,receive.Length);          
					if (ret > 0) 
					{                                                     
						string rcv = System.Text.Encoding.ASCII.GetString(receive,0,ret);
						if(rcv == "exit") 
							s.Close();
						else
						{
							//output_callback(rcv);
							BroadCast(receive, ret);
						}
					}
				}
			}
			catch ( ThreadAbortException)
			{
				output_callback("*** Thread (ID:"+ realId.ToString() +") were aborted.\n");
			}			
			catch(System.IO.IOException)
			{
				output_callback("*** Stream (ID:"+ realId.ToString() +") were forced to be closed.\n");
			}
			finally
			{
				s.Close();
				RemoveTheThread(realId);
			}       
		}

		private void BroadCast( byte[] send_buf, int size)
		{
			lock(this)
			{
				foreach(NetworkStream s in streamHolder.Values)
				{
					try
					{
						if(s.CanWrite)
						{
							s.Write(send_buf,0, size);
						}
					}
					catch
					{
						s.Close();
					}
				}
			}
		}
                                                       
		private void RemoveTheThread(int realId)                                      
		{
			lock(this)
			{                    
				streamHolder.Remove(realId);
				threadHolder.Remove(realId);                                            
			}
		}

		private static void DefaultOutput(string text)
		{
			Debug.Write(text);
			return;
		}
	}
}
