using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source.Packets
{
    class Spawn : Packet
    {
        public override byte ID => 0;

        public override int Size => Encoding.Unicode.GetBytes(nickname).Length + 1;

        string nickname;

        public Spawn(string nickname)
        {
            this.nickname = nickname;
        }

        public override void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Encoding.Unicode.GetBytes(nickname));
            writer.Write((byte)0);
        }
    }
}
