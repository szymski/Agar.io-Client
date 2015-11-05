using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaEpsilon.Engine.Utils;

namespace Agar.io_Client.Source.Packets
{
    class SetDirection : Packet
    {
        public override byte ID => 16;

        public override int Size => 12;

        Vector2 pos;
        uint id;

        public SetDirection(int blob, Vector2 position)
        {
            id = (uint)blob;
            pos = position;
        }

        public override void WriteBytes(BinaryWriter writer)
        {
            writer.Write((int)pos.x);
            writer.Write((int)pos.y);
            writer.Write(id);
        }
    }
}
