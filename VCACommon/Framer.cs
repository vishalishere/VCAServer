using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VCACommon
{
    public class Framer
    {
        private  byte[] FrameBuffer = new byte[1024 * 50]; //5k
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
            //find magic code
            int readBytes = 0;
            int magicStart = -1;
            int count = 0;
            DateTime lastRead = DateTime.Now;
            while (true)
            {
                count = input.Read(FrameBuffer, readBytes, 100);

                if (count > 0)
                {
                    lastRead = DateTime.Now;
                }
                else if (DateTime.Now.Subtract(lastRead).TotalSeconds > 3)
                    throw new EndOfStreamException("No magic code!!!");

                readBytes += count;
                magicStart = FrameBuffer.Find(MagicCode);
                if (magicStart != -1 && magicStart < readBytes)
                    break;
            }
            //read magic code
            byte[] magiccode = new byte[8];
            Array.Copy(FrameBuffer, magicStart, magiccode, 0, 8);
            Debug.Assert(magiccode.SequenceEqual(MagicCode), "Find magic code error!!!!");

            //read data length 
            if (magicStart + 12 > readBytes) // datalength is not include 
            {
                int fourByteToRead = 4;
                while (fourByteToRead > 0)
                {
                    count = input.Read(FrameBuffer, readBytes, 4);
                    readBytes += count;
                    fourByteToRead -= count;

                    if (count > 0)
                    {
                        lastRead = DateTime.Now;
                    }
                    else if (DateTime.Now.Subtract(lastRead).TotalSeconds > 10)
                        throw new EndOfStreamException("Read length error!!!");
                }
            }
            Debug.Assert(BitConverter.IsLittleEndian, "Not BigEndian");
            byte[] len = new byte[4];
            Array.Copy(FrameBuffer, magicStart + 8, len, 0, 4);
            int dataLength = BitConverter.ToInt32(len, 0);
            if(!BitConverter.IsLittleEndian)
            {
                dataLength = (len[0] << 24) | (len[1] << 16) | (len[2] << 8) | len[3];
            }

            //read reminder
            int reminder = dataLength - readBytes + magicStart + 12;
            count = input.Read(FrameBuffer, readBytes, reminder);
            readBytes += count;
            lastRead = DateTime.Now;
            while (reminder > 0)
            {
                count = input.Read(FrameBuffer, readBytes, reminder);
                reminder -= count;
                readBytes += count;
                
                if(count > 0)
                {
                    lastRead = DateTime.Now;
                }
                else if(DateTime.Now.Subtract(lastRead).TotalSeconds > 10)
                    throw new EndOfStreamException("No metadata any more!!!");
            }
            
            byte[] frame = new byte[dataLength];
            Array.Copy(FrameBuffer, magicStart + 12, frame,0, dataLength);
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
