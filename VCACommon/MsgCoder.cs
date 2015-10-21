using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VCACommon
{
    public class MsgCoder
    {
        public static XmlSerializer serializer = new XmlSerializer(typeof(vca));
        private static byte[] BUFF = new byte[1024*1024];

        public static vca fromWire(byte[] input)
        {
            try {
                MemoryStream inputStream = new MemoryStream(input);
                return (vca)serializer.Deserialize(inputStream);
            }
            catch(Exception)
            {
                Console.WriteLine("Deserialize Error" + Encoding.ASCII.GetString(input));
                
                return null;
            }
        }

        public static byte[] toWire(string vca)
        {
            int datalength = vca.Length;
            byte[] frame = new byte[datalength + 4];
            Console.WriteLine("datalength: " + datalength);

            byte[] len = BitConverter.GetBytes(datalength);
            byte[] data = Encoding.ASCII.GetBytes(vca);
            Debug.Assert(data.Length == datalength);
            Array.Copy(len, frame, 4);
            Array.Copy(data,0, frame,4 , datalength);
            return frame;
        }
    }
}
