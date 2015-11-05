using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;
using OpenTK.Graphics.OpenGL;

namespace Agar.io_Client.Source.Scenes
{
    class ShaderTest : Scene
    {
        NetController netController;
        World world;

        Blob blob;

        public ShaderTest()
        {
            blob = new Blob(0)
            {
                desiredPosition = new Vector2(300, 300),
                desiredSize = 50
            };
        }

        public override void Update()
        {
            //blob.desiredPosition = new Vector2(300+MathUtils.Sin(Time.Millis*0.1f) * 200, 300+MathUtils.Cos(Time.Millis * 0.1f) *200);
            blob.desiredPosition = Mouse.Position;
        }

        public override void Render()
        {
            GL.PushMatrix();
            GraphicsHelper.Translate(blob.position);
            blob.Render();
            GL.PopMatrix();
        }
    }
}