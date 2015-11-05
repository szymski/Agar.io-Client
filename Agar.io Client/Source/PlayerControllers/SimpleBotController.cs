using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;
using OpenTK.Graphics.OpenGL;

namespace Agar.io_Client.Source.PlayerControllers
{
    class SimpleBotController : PlayerController
    {
        Blob mainBlob = null;
        Blob targetBlob = null;
        float direction = 0;

        protected override void Update()
        {
            // Update only when player owns blobs, otherwise request spawn
            mainBlob = PlayerBlobs.FirstOrDefault();
            if (mainBlob == null)
            {
                Timer.Every("spawn", 2f, Spawn);
                return;
            }

            //if (targetBlob == null || !Blobs.Contains(targetBlob))
            //    targetBlob =
            //        Blobs.Where(b => b.size < PlayerBlobs.First().size)
            //            .OrderBy(b => (b.position - PlayerBlobs.First().position).Length)
            //            .FirstOrDefault();

            //if (targetBlob == null)
            //    return;

            //DesiredPosition = targetBlob.position;

            targetBlob =
                        Blobs.Where(b => b.size < PlayerBlobs.First().size)
                            .OrderBy(b => (b.position - PlayerBlobs.First().position).Length)
                            .FirstOrDefault();

            if (targetBlob != null)
                direction = -(targetBlob.position - mainBlob.position).Angle + 90;

            foreach (var blob in Blobs.Where(b => b.desiredSize > mainBlob.desiredSize))
            {
                direction = (direction + (-(blob.position - mainBlob.position).Angle + 90)*(1/Vector2.Distance(mainBlob.position,blob.position)))*0.5f;
            }

            DesiredPosition = mainBlob.position + Vector2.FromAngle(direction) * 100f;
        }

        public override void Render()
        {
            if (mainBlob == null || targetBlob == null)
                return;

            GL.Color3(1f, 0, 0);
            GraphicsHelper.DrawLine(mainBlob.position, targetBlob.position);
        }
    }
}
