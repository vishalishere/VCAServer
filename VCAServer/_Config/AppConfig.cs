using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VCAServer
{
    public class AppConfig
    {
        public static string Server { get; set; }

        public static string CounterApi { get; set; }

        public static string HeatPointApi { get; set; }

        static AppConfig()
        {
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            var doc = XDocument.Load(appRoot + "_Config/config.xml");
            AppConfig.Server = doc.Root.Element("Server").Value;
            AppConfig.CounterApi = doc.Root.Element("CounterApi").Value;
            AppConfig.HeatPointApi = doc.Root.Element("HeatPointApi").Value;
        }
    }
}
