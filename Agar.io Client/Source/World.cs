using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agar.io_Client.Source.Packets;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;
using OpenTK.Graphics.OpenGL;

namespace Agar.io_Client.Source
{
    class World
    {
        public List<int> playerBlobs = new List<int>();
        public List<Blob> blobs = new List<Blob>();
        public Dictionary<int, Blob> blobIds = new Dictionary<int, Blob>();

        public Vector2 boundsMin = new Vector2(), boundsMax = new Vector2();

        public Vector2 cameraPos = new Vector2();
        public float cameraSize = 1;

        public World()
        {
            Instance = this;

            //Blob targetBlob = null;

            //Timer.Create("update_direction", 0.1f, 0, () =>
            //{
            //    if (playerBlobs.Count > 0)
            //    {
            //        Blob playerBlob = blobIds[playerBlobs[0]];

            //        Vector2 pos = Mouse.Position - Screen.Size / 2;
            //        //NetController.Instance.socket.Send(new SetDirection(playerBlobs[0], blobIds[playerBlobs[0]].position + pos).GetBytes());

            //        // Bot

            //        //if (targetBlob == null)
            //        targetBlob =
            //            blobs.Where(b => b.size < playerBlob.size)
            //                .OrderBy(b => (b.position - playerBlob.position).Length)
            //                .FirstOrDefault();

            //        if (targetBlob == null)
            //            return;

            //        NetController.Instance.socket.Send(new SetDirection(playerBlobs[0], targetBlob.position).GetBytes());
            //    }
            //});

            Timer.Create("show_mass", 1f, 0, () =>
            {
                Log.Print($"Mass: {GetTotalMass()}");
            });
        }

        public Blob GetBlobById(int id)
        {
            if (!blobIds.ContainsKey(id))
            {
                var blob = new Blob(id);
                blobs.Add(blob);
                blobIds.Add(id, blob);
                return blob;
            }
            return blobIds[id];
        }

        public void RemoveBlob(int id)
        {
            if (blobIds.ContainsKey(id))
            {
                var blob = blobIds[id];
                blobIds.Remove(id);
                blobs.Remove(blob);
            }
            if (playerBlobs.Contains(id))
            {
                playerBlobs.Remove(id);
            }
        }

        public void Update()
        {
            // Update camera size
            cameraSize += Mouse.MouseWheelDelta * 0.02f;

            // Update camera position by all player blobs
            Vector2 desiredPosition = new Vector2(cameraPos.x, cameraPos.y);
            foreach (var blob in playerBlobs.Select(b => blobIds[b]))
                desiredPosition = (desiredPosition + blob.position) * 0.5f;
            cameraPos = MathUtils.Lerp(Time.DeltaTime * 30f, cameraPos, desiredPosition);
        }

        public void Render()
        {
            // Translate the view to player's blob
            GL.Translate(Screen.Width / 2f, Screen.Height / 2f, 0);
            GL.Scale(cameraSize, cameraSize, 1f);
            //GL.Translate(-Screen.Width / 2f, -Screen.Height / 2f, 0);

            GL.Translate(-cameraPos.x, -cameraPos.y, 0);

            // Draw bounds
            GL.Color3(0, 0, 1f);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(boundsMin.x, boundsMin.y);
            GL.Vertex2(boundsMax.x, boundsMin.y);
            GL.Vertex2(boundsMax.x, boundsMax.y);
            GL.Vertex2(boundsMin.x, boundsMax.y);
            GL.End();

            // Draw grid

            GL.Color3(0.8f, 0.8f, 0.8f);

            for (float x = boundsMin.x; x < boundsMax.x; x += 100)
                GraphicsHelper.DrawLine(x, boundsMin.y, x, boundsMax.y);

            for (float y = boundsMin.y; y < boundsMax.y; y += 100)
                GraphicsHelper.DrawLine(boundsMin.x, y, boundsMax.x, y);


            // Draw blobs

            foreach (var blob in blobs)
            {
                GL.PushMatrix();
                GL.Translate(blob.position.x, blob.position.y, 0);
                blob.Render();
                GL.PopMatrix();
            }
        }

        public int GetTotalMass()
        {
            int mass = 0;
            foreach (var blob in playerBlobs.Select(id => blobIds[id]))
                mass += (int)blob.size;

            return mass;
        }

        public static World Instance
        { get; private set; }
    }
}
