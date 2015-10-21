using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCAServer
{
    class Program
    {
        static void Main(string[] args)
        {


            VCAService client = new VCAService();
            client.Start();
            Console.WriteLine("start");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;

            Console.WriteLine("stop");
            client.Stop();

        }
    }
}
