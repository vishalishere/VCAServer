using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCACommon
{
    public class Framer
    {
        private static byte[] FrameBuffer = new byte[1024 * 1024 ];
        public static byte[] nextFrameByLength(Stream input)
        {
            //TODO: add magic code
            byte[] buffer = new byte[4];
                
            //读取长度
            int count = input.Read(buffer, 0, 4);
            while(count < 4)
            {
                count += input.Read(buffer, count, 4 - count);
            }
               
            int length = BitConverter.ToInt32(buffer, 0);
            Console.WriteLine(length);

            count = input.Read(FrameBuffer, 0, length);
            while(count < length)
            {
                count += input.Read(FrameBuffer, count, length - count );
            }
            byte[] frame = new byte[length];
            Array.Copy(FrameBuffer, frame, length);
            return frame;
            
        }
    }
}
