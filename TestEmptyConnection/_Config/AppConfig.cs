using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestEmptyConnection
{
    public class AppConfig
    {
        public static string Camera
        {
            get;
            set;
        }

        public static string Server
        {
            get;
            set;
        }

        public static int ServerPort
        {
            get;
            set;
        }

        static AppConfig()
        {
            var appRoot = AppDomain.CurrentDomain.BaseDirectory;
            XDocument doc = XDocument.Load(appRoot + "_Config/config.xml");

            Camera = doc.Root.Element("Camera").Value;
            Server = doc.Root.Element("Server").Value;
            ServerPort = int.Parse(doc.Root.Element("ServerPort").Value);

        }
    }
}
