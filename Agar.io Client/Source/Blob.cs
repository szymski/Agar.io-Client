using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;
using OpenTK.Graphics.OpenGL;
using ShaderType = DeltaEpsilon.Engine.Utils.ShaderType;

namespace Agar.io_Client.Source
{
    class Blob
    {
        public int id = 0;
        public Vector2 position = new Vector2(), desiredPosition = new Vector2();
        public float size = 0, desiredSize = 0;
        public byte r, g, b;
        public byte flags = 0;

        Shader shader;

        public Blob(int id)
        {
            this.id = id;

            shader = new Shader(@"

uniform vec2 resolution;
uniform vec2 worldPos;
uniform float size;
uniform vec3 color;

void main() {
    gl_FragColor = vec4(color.x, color.y, color.z, (length(worldPos-gl_FragCoord)-size*0.7)/size*2.0);
}

", ShaderType.Fragment);


        }

        public void UpdatePosition(Vector2 newPosition)
        {
            if (position.x == 0 && position.y == 0)
                position = newPosition;
            desiredPosition = newPosition;
        }

        public void UpdateSize(short newSize)
        {
            if (size == 0)
                size = newSize;
            desiredSize = newSize;
        }

        public void Render()
        {
            position = MathUtils.Lerp(Time.DeltaTime * 20f, position, desiredPosition);
            size = MathUtils.Lerp(Time.DeltaTime * 20f, size, desiredSize);

            GL.Color3(r, g, b);

            //  shader.SetVariable("resolution", Screen.Width, Screen.Height);
            //shader.SetVariable("time", Time.Millis / 1000f);
            //GL.Enable(EnableCap.Blend);
            //float[] matrix = new float[16];
            //GL.GetFloat(GetPName.ModelviewMatrix, matrix);
            //// Console.WriteLine($"X: {matrix[12]}\tY: {matrix[13]}");
            //shader.SetVariable("worldPos", Screen.Width / 2f + matrix[12] * Screen.Width / 2f, Screen.Height / 2f + matrix[13] * Screen.Height / 2f);
            //shader.SetVariable("size", size);
            //shader.SetVariable("color", r / 255f, g / 255f, b / 255f);
            //shader.Bind();

            //if (World.Instance.playerBlobs.Contains(id))
            //    GL.Color3(0, 0, 0);

            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < 360; i++)
            {
                if ((flags & 0x01) > 0) // If virus
                    GL.Vertex2(MathUtils.Sin(i) * (size + MathUtils.Sin(i * 50 + 77) * 3), MathUtils.Cos(i) * (size + MathUtils.Sin(i * 50 + 77) * 3));
                else
                    GL.Vertex2(MathUtils.Sin(i) * size, MathUtils.Cos(i) * size);
            }
            GL.End();

            //shader.UnBind();

            GL.Disable(EnableCap.Blend);
        }
    }
}
