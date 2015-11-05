using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using VCACommon;

namespace VCAServer
{
    public class VCAService
    {

        private BlockingCollection<vca> _queue = new BlockingCollection<vca>(new ConcurrentQueue<vca>());
        private TcpListener _tcpListener = null;
        private XmlSerializer serializer = null;

        public VCAService()
        {
            serializer = new XmlSerializer(typeof(vca));
        }


        public void Start()
        {
            int servPort = 9001;
            try
            {
                //Create a TCPListener to accept client connections
                _tcpListener = new TcpListener(IPAddress.Any, servPort);
                _tcpListener.Start();
                LogInfo("Listening on port:" + servPort);
            }
            catch (SocketException se)
            {
                LogInfo("端口被占用");
                Environment.Exit(se.ErrorCode);
            }

            Task.Factory.StartNew(() => { MonitorQueue(); }, TaskCreationOptions.LongRunning);
            //Run forever for accepting and servincing connections 
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();//Get client connection
                    Task.Factory.StartNew(() => {
                        ProcessNewClient(client);
                    }, TaskCreationOptions.LongRunning);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _tcpListener.Stop();
            _queue.CompleteAdding();
        }

        private void ProcessNewClient(TcpClient client)
        {
            LogInfo("ProcessNewClient: " + client.Client.RemoteEndPoint);
            NetworkStream netStream = null;
            try
            {
                netStream = client.GetStream();
                
                Framer framer = new Framer(netStream);
                while (true)
                {
                    byte[] frame = framer.nextFrameByMagicCode();
                    vca metadata = MsgCoder.fromWire(frame);
                    if (metadata == null)
                        break;

                    _queue.Add(metadata);
                        
                }
            }
            catch (Exception error)
            {
                LogError("Proccess Client Exception: " + error.Message + 
                         "\n断开连接 " + client.Client.RemoteEndPoint);
            }
            finally
            {
                if (netStream != null)
                    netStream.Close();
                if (client != null)
                    client.Close();
                
            }

        }

        private void MonitorQueue()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(vca));
            int count = 0;
            while (true)
            {
                try
                {
                    vca frame = _queue.Take();
                    if (count % 10 == 0)
                    {
                        Console.WriteLine("vca meta " + frame.cam_ip );
                    }
                    if (count >= 1000)
                    {
                        count = 0;
                        Console.Clear();
                    }
                    count++;
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void LogInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
