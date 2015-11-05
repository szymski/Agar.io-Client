using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source.Packets
{
    internal abstract class Packet
    {
        public abstract byte ID
        { get; }
        public abstract int Size
        { get; }
        public abstract void WriteBytes(BinaryWriter writer);

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[1024];
            bytes[0] = ID;
            WriteBytes(new BinaryWriter(new MemoryStream(bytes, 1, 1023, true)));
            Array.Resize(ref bytes, 1 + Size);
            return bytes;
        }
    }
}
