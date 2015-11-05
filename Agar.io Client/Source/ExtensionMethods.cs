using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeltaEpsilon.Engine;

namespace Agar.io_Client.Source
{
    static class ExtensionMethods
    {
        public static Task Send(this WebSocket socket, byte[] data)
        {
            return socket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        public static async Task<byte[]> Receive(this WebSocket socket)
        {
            byte[] data = new byte[1024 * 4];
            await socket.ReceiveAsync(new ArraySegment<byte>(data), CancellationToken.None);
            return data;
        }

        public static string ReadNullTermString(this BinaryReader reader)
        {
            string str = "";

            for (;;)
            {
                byte b = reader.ReadByte();
                if (b == 0x00)
                    break;
                str += (char)b;
            }

            return str;
        }

        public static string ReadNullTermStringUnicode(this BinaryReader reader)
        {
            string str = "";

            for (;;)
            {
                byte b = reader.ReadByte();
                byte b2 = reader.ReadByte();
                if (b == 0x00 && b2 == 0x00)
                    break;
                str += Encoding.Unicode.GetString(new[] { b, b2 });
            }

            return str;
        }
    }
}
