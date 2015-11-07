using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCAServer
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            VCAService client = new VCAService();
            client.Start();
            log.Info("start");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;

            Console.WriteLine("stop");
            client.Stop();

        }
    }
}
