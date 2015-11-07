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
using VCAServer.HttpService;

namespace VCAServer
{
    public class VCAService
    {

        private BlockingCollection<vca> _queue = new BlockingCollection<vca>(new ConcurrentQueue<vca>());
        private TcpListener _tcpListener = null;
        private CounterService counterServer;
        private HeatMapService heatMapService;

        public VCAService()
        {
            counterServer = new CounterService();
            heatMapService = new HeatMapService();
        }


        public void Start()
        {
            int servPort = 9001;
            try
            {
                //Create a TCPListener to accept client connections
                _tcpListener = new TcpListener(IPAddress.Any, servPort);
                _tcpListener.Start();
                CLog.Info("Listening on port:" + servPort);
            }
            catch (SocketException se)
            {
                CLog.Error("端口被占用");
                Environment.Exit(se.ErrorCode);
            }

            Task.Factory.StartNew(() => { MonitorQueue(); }, TaskCreationOptions.LongRunning);
            //Run forever for accepting and servincing connections 
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();//Get client connection
                    Task.Factory.StartNew((arg) => 
                    {
                        TcpClient localClient = (TcpClient)arg;
                        ProcessNewClient(localClient);
                    }, client);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _tcpListener.Stop();
            _queue.CompleteAdding();
            counterServer.Dispose();
        }

        private void ProcessNewClient(TcpClient client)
        {
            CLog.Info("ProcessNewClient: " + client.Client.RemoteEndPoint);
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

                    if (metadata.objects != null && metadata.objects.Length > 0)
                    {
                        _queue.Add(metadata);
                    }
                        
                }
            }
            catch (Exception error)
            {
               CLog.Error("Proccess Client Exception: " + error.Message + 
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
            int count = 0;
            while (true)
            {
                try
                {
                    vca metadata = _queue.Take();

                    if (metadata.events != null && metadata.events.Length > 0)
                    {
                        counterServer.Add(metadata);
                    }
                    heatMapService.Add(metadata);
                    

                    if (count % 10 == 0)
                    {
                        CLog.Debug("metadata from " + metadata.cam_ip);
                    }

                }
                catch (Exception)
                {
                    break;
                }
               
                if (count >= 1000)
                {
                    count = 0;
                    Console.Clear();
                }
                count++;
            }
        }

       
    }
}
