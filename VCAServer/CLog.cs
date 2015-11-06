using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCAServer
{
    public class CLog
    {
        private static int Level = DEBUG;
        private static int ERROR = 5;
        private static int WARN = 4;
        private static int INFO = 3;
        private static int DEBUG = 2;

        public static void Debug(string info)
        {
            lock (Console.Out)
            {
                Console.WriteLine(info);
            }
        }

        public static void Info(string info)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(info);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }


        public static void Warn(string warn)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(warn);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void Error(string error)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
