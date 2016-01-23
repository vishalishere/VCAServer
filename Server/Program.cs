using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener _tcpListener = null;
             int servPort = 9001;
            try
            {
                //Create a TCPListener to accept client connections
                _tcpListener = new TcpListener(IPAddress.Any, servPort);
                 _tcpListener.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine("start socket error: " + se.Message);
            }
            //new thread
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    ProcessNewClient(client);
                }
            }, TaskCreationOptions.LongRunning);
            Console.WriteLine("Server listening on: " + servPort);
            Console.ReadLine();

        }

        private static void ProcessNewClient(TcpClient client)
        {
            Console.WriteLine("ProcessNewClient: " + client.Client.RemoteEndPoint);
            NetworkStream netStream = null;
            try
            {
                netStream = client.GetStream();
                byte[] buf = new byte[50];
                var appRoot = AppDomain.CurrentDomain.BaseDirectory;
                while (true)
                {

                    int count = netStream.Read(buf, 0, buf.Length);
                    Console.WriteLine(count);

                    File.AppendAllText(appRoot + "vca.txt", ASCIIEncoding.ASCII.GetString(buf.Take(count).ToArray()));
                   
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("error" + error.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                
            }
            finally
            {
                if (netStream != null)
                    netStream.Close();
                if (client != null)
                    client.Close();

            }
        }

    }
}
