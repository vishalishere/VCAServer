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
using Newtonsoft.Json;
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
                Debug.WriteLine("Listening on port:" + servPort);
            }
            catch (SocketException se)
            {
                ErrorLog.Instance.Log(se, "端口被占用");
                Environment.Exit(se.ErrorCode);
            }

            Task.Run(() => { MonitorQueue(); });
            //Run forever for accepting and servincing connections 
            Task.Run(() =>
            {
                while (true)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();//Get client connection
                    Task.Run(() => {
                        Console.WriteLine("ProcessNewClient");
                        ProcessNewClient(client);
                    });
                }
            });
        }

        public void Stop()
        {
            _tcpListener.Stop();
            _queue.CompleteAdding();
        }

        private void ProcessNewClient(TcpClient client)
        {
            NetworkStream netStream = null;
            try
            {
                netStream = client.GetStream();
                
                Framer framer = new Framer(netStream);
                while (true)
                {
                    byte[] frame = framer.nextFrameByMagicCode();
                    vca metadata = MsgCoder.fromWire(frame);
                    _queue.Add(metadata);
                        
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Proccess Client Exception: " + error.Message);
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
            while (true)
            {
                try
                {
                    vca frame = _queue.Take();

                    if (frame.events == null)
                    {
                        Console.WriteLine("event null");
                    }
                    else
                    {
                        Console.WriteLine(frame.schema_version);
                        Console.WriteLine("events " + frame.events.Length);
                        Console.WriteLine(JsonConvert.SerializeObject(frame.events));
                        Console.WriteLine("objects " + frame.objects.Length);
                        Console.WriteLine(JsonConvert.SerializeObject(frame.objects));
                        Console.WriteLine("-----------");

                    }


                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}
