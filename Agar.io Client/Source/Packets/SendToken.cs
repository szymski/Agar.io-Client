using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Agar.io_Client.Source.Packets
{
    class SendToken : Packet
    {
        public override byte ID => 80;

        public override int Size => Encoding.ASCII.GetBytes(token).Length;

        string token;

        public SendToken(string token)
        {
            this.token = token;
        }

        public override void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(token));
            writer.Write((byte)0);
        }
    }
}
