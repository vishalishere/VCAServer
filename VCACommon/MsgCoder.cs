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
        public static string MagicString = "vca_meta";
        public static byte[] MagicCode = Encoding.ASCII.GetBytes(MagicString);

        public static vca fromWire(byte[] input)
        {
            try {
                MemoryStream inputStream = new MemoryStream(input);
                return (vca)serializer.Deserialize(inputStream);
            }
            catch(Exception)
            {
                Console.WriteLine("Deserialize Error" + Encoding.ASCII.GetString(input));
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "vcadump.txt", Encoding.ASCII.GetString(input));

                return null;
            }
        }

        public static byte[] toWire(string vca)
        {
            int datalength = vca.Length;
            byte[] frame = new byte[datalength + 4 + 8];
            Console.WriteLine("datalength: " + datalength);

            byte[] len = BitConverter.GetBytes(datalength);
            byte[] data = Encoding.ASCII.GetBytes(vca);
            Debug.Assert(data.Length == datalength);
            Array.Copy(MagicCode, frame, 8);
            Array.Copy(len,0 ,frame,8, 4);
            Array.Copy(data,0, frame,12 , datalength);
            return frame;
        }
    }
}
