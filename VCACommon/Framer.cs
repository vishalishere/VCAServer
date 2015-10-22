using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VCACommon
{
    public class Framer
    {
        private  byte[] FrameBuffer = new byte[1024 * 1024 ];
        private Stream input;

        public static string MagicString = "vca_meta";
        public static byte[] MagicCode = Encoding.ASCII.GetBytes(MagicString);

        public Framer(Stream input)
        {
            this.input = input;
        }

        public  byte[] nextFrameByLength()
        {
            //TODO: add magic code
            byte[] buffer = new byte[4];

            //读取长度
            int count = input.Read(buffer, 0, 4);
            while (count < 4)
            {
                count += input.Read(buffer, count, 4 - count);
            }

            int length = BitConverter.ToInt32(buffer, 0);

            count = input.Read(FrameBuffer, 0, length);
            while (count < length)
            {
                count += input.Read(FrameBuffer, count, length - count);
            }
            byte[] frame = new byte[length];
            Array.Copy(FrameBuffer, frame, length);
            return frame;
        }

        public byte[] nextFrameByMagicCode()
        {
            //read magic code
            byte[] buffer = new byte[8];
            
            int readBytes = input.Read(buffer,0, 8);
            
            while(readBytes < 8)
            {
                int count = input.Read(buffer, readBytes, 8 - readBytes);
                if (count == 0) throw new EndOfStreamException("Cannt read data any more!!!");
                readBytes += count;
            }

            if (!buffer.SequenceEqual(MagicCode)) throw new Exception("Illegal Connection");
           
            //读取长度
            readBytes = input.Read(buffer, 0, 4);
            
            while (readBytes < 4)
            {                
                int count = input.Read(buffer, readBytes, 4 - readBytes);
                if (count == 0) throw new EndOfStreamException("Cannt read data any more!!!");
                readBytes += count;

            }

            int length = BitConverter.ToInt32(buffer, 0);

            readBytes = input.Read(FrameBuffer, 0, length);
            while (readBytes < length)
            {
                int count = input.Read(FrameBuffer, readBytes, length - readBytes);
                if (count == 0) throw new EndOfStreamException("Cannt read data any more!!!");
                readBytes += count;
            }
            byte[] frame = new byte[length];
            Array.Copy(FrameBuffer, frame, length);
            return frame;


        }

        
    }

    static class Extensions
    {
        public static int Find(this byte[] buff, byte[] search)
        {
            // enumerate the buffer but don't overstep the bounds
            for (int start = 0; start < buff.Length - search.Length; start++)
            {
                // we found the first character
                if (buff[start] == search[0])
                {
                    int next;

                    // traverse the rest of the bytes
                    for (next = 1; next < search.Length; next++)
                    {
                        // if we don't match, bail
                        if (buff[start + next] != search[next])
                            break;
                    }

                    if (next == search.Length)
                        return start;
                }
            }
            // not found
            return -1;
        }
    }
}
