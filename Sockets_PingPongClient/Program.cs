using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets_PingPongClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 9999);

                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    sender.Connect(localEndPoint);
 

                    bool endConnection = false;
                    while (!endConnection)
                    {
                        Console.WriteLine("enter command to server, if you wand to end connection enter: 0");
                        string message = Console.ReadLine();

                        if (message == "0")
                        {
                            endConnection = true;
                        }
                        else
                        {
                            byte[] messageSent = Encoding.ASCII.GetBytes(message);
                            int byteSent = sender.Send(messageSent);

                            byte[] messageReceived = new byte[1024];

                            int byteRecv = sender.Receive(messageReceived);
                            Console.WriteLine("Message from Server -> {0}",
                                  Encoding.ASCII.GetString(messageReceived,
                                                             0, byteRecv));
                        }

                    }
                }
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

        }
    }
}
