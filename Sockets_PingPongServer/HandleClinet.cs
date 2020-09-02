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
        private string CheckLastCommand(string comnmand)
        {
            if (comnmand != "")
            {
                if (comnmand.Contains("cd") )
                {
                    return comnmand;
                }
                if (comnmand.Contains("mkdir"))
                {
                    string newCommand = "cd ";
                    newCommand += comnmand.Split(" ")[1];
                    return newCommand;
                }
            }

            return "";
        }
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
                string lastCommand = "";

                try
                {
                    int numByte = clientSocket.Receive(bytesReceive);
                    data = Encoding.ASCII.GetString(bytesReceive,
                                          0, numByte);
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.WorkingDirectory = @"C:/";
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C" + data;
                    process.StartInfo = startInfo;
                    

                    Console.WriteLine("Command Line received -> {0} ", data);
                    string commandAfterCheck = CheckLastCommand(lastCommand);
                    if (commandAfterCheck != "")
                    {
                        startInfo.Arguments = "/C" + commandAfterCheck + " & "+data;
                    }
                    var isSuccess = process.Start();
                    lastCommand = data;

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
