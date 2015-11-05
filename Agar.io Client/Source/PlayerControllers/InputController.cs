using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaEpsilon.Engine;

namespace Agar.io_Client.Source.PlayerControllers
{
    class InputController : PlayerController
    {
        protected override void Update()
        {
            // Request spawn
            if (Keyboard.GetKeyDown(KeyCode.R))
                Spawn();

            // Update only when player owns blobs
            Blob mainBlob = PlayerBlobs.FirstOrDefault();
            if (mainBlob == null)
                return;

            // Update position
            DesiredPosition = mainBlob.position + Mouse.Position - Screen.Size / 2f;

            // Update split and mass ejecting
            if (Keyboard.GetKeyDown(KeyCode.Space))
                Split();
            if (Keyboard.GetKeyDown(KeyCode.W))
                EjectMass();
        }
    }
}
