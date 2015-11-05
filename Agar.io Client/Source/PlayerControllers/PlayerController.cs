using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agar.io_Client.Source.Packets;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;

namespace Agar.io_Client.Source.PlayerControllers
{
    abstract class PlayerController
    {
        protected abstract void Update();
        public virtual void Render() { }

        World World => World.Instance;
        protected List<Blob> Blobs => World.blobs;
        protected Dictionary<int, Blob> BlobIds => World.blobIds;
        protected IEnumerable<Blob> PlayerBlobs => World.playerBlobs.Select(id => World.blobIds[id]);
        protected int TotalMass => PlayerBlobs.Sum(blob => (int)blob.desiredSize);
        protected Vector2 DesiredPosition
        { get; set; } = new Vector2();


        protected void Split() => NetController.SendPacket(new Split());
        protected void EjectMass() => NetController.SendPacket(new EjectMass());
        protected void Spawn() => NetController.SendPacket(new Spawn("TEST CLIENT"));

        public void UpdateInternal()
        {
            Update();

            Timer.Every("direction_update", 0.1f, () =>
            {
                if (PlayerBlobs.Any())
                    NetController.SendPacket(new SetDirection(PlayerBlobs.First().id, DesiredPosition));
            });
        }
    }
}
