using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agar.io_Client.Source.Packets;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;

namespace Agar.io_Client.Source
{
    class NetController
    {
        public ClientWebSocket socket;
        Queue<byte[]> incomingPackets = new Queue<byte[]>();

        public NetController()
        {
            Instance = this;
        }

        public void Update()
        {
            byte[] message;
            while ((message = GetIncomingMessage()) != null)
            {
                BinaryReader reader = new BinaryReader(new MemoryStream(message));
                byte id = reader.ReadByte();
                switch (id)
                {
                    // World Update
                    case 16:

                        // Eat events
                        short eats = reader.ReadInt16();
                        for (int i = 0; i < eats; i++)
                        {
                            int eater = reader.ReadInt32();
                            int victim = reader.ReadInt32();
                        }

                        // Blob update
                        int player;
                        while ((player = reader.ReadInt32()) != 0)
                        {
                            int x = reader.ReadInt32();
                            int y = reader.ReadInt32();
                            short size = reader.ReadInt16();
                            byte r = reader.ReadByte(), g = reader.ReadByte(), b = reader.ReadByte();
                            byte flags = reader.ReadByte();
                            if ((flags & 0x02) > 0)
                            {
                                int skip = reader.ReadInt32();
                            }
                            if ((flags & 0x04) > 0)
                            {
                                string str = reader.ReadNullTermString();
                            }

                            string name = reader.ReadNullTermStringUnicode();

                            var blob = World.Instance.GetBlobById(player);
                            blob.UpdatePosition(new Vector2(x, y));
                            blob.UpdateSize(size);
                            blob.r = r;
                            blob.g = g;
                            blob.b = b;
                            blob.flags = flags;
                        }

                        int removals = reader.ReadInt32();
                        for (int i = 0; i < removals; i++)
                        {
                            int blob = reader.ReadInt32();
                            World.Instance.RemoveBlob(blob);
                        }

                        break;

                    // Owns cell
                    case 32:
                        int cell = reader.ReadInt32();
                        World.Instance.playerBlobs.Add(cell);
                        Log.Print($"Player owns {cell}");
                        break;

                    // World size
                    case 64:
                        float min_x = (float)reader.ReadDouble();
                        float min_y = (float)reader.ReadDouble();
                        float max_x = (float)reader.ReadDouble();
                        float max_y = (float)reader.ReadDouble();
                        World.Instance.boundsMin = new Vector2(min_x, min_y);
                        World.Instance.boundsMax = new Vector2(max_x, max_y);
                        Log.Print($"World size: \n\tX: {min_x} - {max_x}\n\tY: {min_y} - {max_y}");
                        socket.Send(new Spawn("TEST CLIENT").GetBytes()); // Request player spawn
                        break;
                }
            }
        }

        public byte[] GetIncomingMessage()
        {
            if (incomingPackets.Count == 0)
                return null;

            return incomingPackets.Dequeue();
        }

        Queue<Packet> packetQueue = new Queue<Packet>();

        public static void SendPacket(Packet packet)
        {
            Instance.packetQueue.Enqueue(packet);
        }

        public async void Connect(string ip, string token)
        {
            socket = new ClientWebSocket();

            Log.Print($"Connecting to {ip}");
            await socket.ConnectAsync(new Uri($"ws://{ip}"), CancellationToken.None);
            Log.Print("Connected");

            await socket.Send(new ResetConnection1().GetBytes());
            await socket.Send(new ResetConnection2().GetBytes());
            await socket.Send(new SendToken(token).GetBytes());

            for (;;)
            {
                while(packetQueue.Count > 0)
                    await socket.Send(packetQueue.Dequeue().GetBytes());

                incomingPackets.Enqueue(await socket.Receive());
            }
        }

        public static NetController Instance { get; private set; }
    }
}
