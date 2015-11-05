using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source.Packets
{
    class ResetConnection2 : Packet
    {
        public override byte ID => 255;

        public override int Size => 4;

        public override void WriteBytes(BinaryWriter writer)
        {
            writer.Write((uint)2200049715);
        }
    }
}
