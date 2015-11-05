using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source.Packets
{
    class Split : Packet
    {
        public override byte ID => 17;

        public override int Size => 0;

        public override void WriteBytes(BinaryWriter writer)
        {

        }
    }
}
