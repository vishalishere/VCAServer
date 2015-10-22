using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestEmptyConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {
                client = new TcpClient(AppConfig.Server, AppConfig.ServerPort);
                Console.WriteLine("Connected to server...");
                netStream = client.GetStream();
                byte[] buff = Encoding.ASCII.GetBytes("vca_meta");
                netStream.Write(buff, 0, 8);
                netStream.Close();
                
                while (true)
                {
                    Thread.Sleep(3000);
                }

            }
            catch (SocketException se)
            {
                Console.WriteLine("Socket Exception");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
