using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 9001);
            Stream stream = client.GetStream();
            while(true)
            {
                string message = Console.ReadLine();
                if(message == "q")
                {
                    break;
                }
                var buff = ASCIIEncoding.ASCII.GetBytes(message);
                stream.Write(buff, 0, buff.Length);
            }
            stream.Close();
            client.Close();
        }
    }
}
