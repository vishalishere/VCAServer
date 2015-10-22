using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VCA;
using VCACommon;

namespace VCATestClient
{
    class Program
    {
        static BlockingCollection<string> MetaDataQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());

        static void Main(string[] args)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(vca));

            VCAClient vcaClient = new VCAClient();
            vcaClient.FrameArrived += VcaClient_FrameArrived;
            vcaClient.ConnectStatus += VcaClient_ConnectStatus;
            vcaClient.Play(AppConfig.Camera);

            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {
                client = new TcpClient(AppConfig.Server, AppConfig.ServerPort);
                Console.WriteLine("Connected to server...");
                netStream = client.GetStream();
               
                byte[] bla = Encoding.ASCII.GetBytes("aaaaaaaaaaaadfasdfasdfasdfasdfasdfasdfasdfasfdasdfasdf");
                while (true)
                {
                    string metadata = MetaDataQueue.Take();
                    byte[] frame = MsgCoder.toWire(metadata);
                    //测试前后插入垃圾数据 
                    netStream.Write(bla, 0, bla.Length);
                    netStream.Write(frame, 0, frame.Length);
                    netStream.Write(bla, 0, bla.Length);

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

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void VcaClient_ConnectStatus(object sender, EventArgs e)
        {
            var args = e as ConnectStatusArgs;
            var result = args.ResultCode;

        }

        private static void VcaClient_FrameArrived(object sender, EventArgs e)
        {

            MetaEventArgs args = e as MetaEventArgs;
            string metadata = args.Meta;
            MetaDataQueue.Add(metadata);
            Console.WriteLine("enter");

        }
    }

}
