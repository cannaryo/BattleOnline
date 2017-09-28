using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;


namespace NetgameProj
{
	/// <summary>
	/// ClientManager ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ClientManager
	{
		public delegate void streamReadCallback(Object obj);

		private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private NetworkStream strm;
		private Thread rdThd = null;
		private streamReadCallback strd_callback = null;
        
		public ClientManager()
		{

		}

		public void Connect(string host, int port)
		{
			if( socket.Connected )
				return;

			try 
			{
                // .NET2.0à»ç~Ç…ëŒâûÇµÇƒïœçX
                // IPAddress ipAddress = Dns.Resolve(host).AddressList[0];
                // IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress[0], port);
                // socket.Connect((ipLocalEndPoint);
                socket.Connect(host, port);
				strm = new NetworkStream(socket, true);
			}
			catch(Exception e)
			{
				Debug.WriteLine(e, "*** Socket connection failure\n");
				throw e;
			}
		}

		public void Disconnect()
		{
			try
			{
				if( socket.Connected ) 
				{
					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
					socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				}
			}
			catch(System.Exception e)
			{
				Debug.WriteLine(e, "*** unexpected exception in closing socket\n");
			}
			try
			{
				if(rdThd != null && rdThd.IsAlive)
				{
					rdThd.Abort();
					rdThd.Join();
				}
			}
			catch(System.Exception e)
			{
				Debug.WriteLine(e, "*** unexpected exception in closing thread\n");
			}
		}

		public void SendSerializedObject(Object obj)
		{
			try
			{
				if( strm.CanWrite) 
				{
					BinaryFormatter f = new BinaryFormatter();
					f.Serialize(strm,obj);
				}
			}
			catch(System.Exception e)
			{
				Debug.WriteLine(e, "*** Network data writing error\n");
				Disconnect();
				throw e;
			}
		}
		
		public void StartReadingThread(streamReadCallback cb)
		{
			if( rdThd != null ) 
				throw new Exception("Multi starting of the reading thread");
			strd_callback = cb;
			rdThd = new Thread(new ThreadStart(ReceiveSerializedObject));
			rdThd.Name = "NetworkStreamReader";
			rdThd.IsBackground = true;
			rdThd.Start() ;
		}

		private void ReceiveSerializedObject()
		{
			try
			{	
				while( strm.CanRead )
				{
					BinaryFormatter f = new BinaryFormatter();
					Object obj = f.Deserialize( strm );
					strd_callback(obj);
				}
			}
			catch(ThreadAbortException)
			{
				Debug.WriteLine("*** Network reading thread were aborted");
			}			
			catch(System.IO.IOException)
			{
				Debug.WriteLine("*** Client socket has been closed");
			}
			catch(System.Runtime.Serialization.SerializationException)
			{
				Debug.WriteLine("*** Data serialization failure");
			}
			finally
			{
				rdThd = null;
			}
		}
	}
}
