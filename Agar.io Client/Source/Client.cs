using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agar.io_Client.Source.Packets;
using Agar.io_Client.Source.Scenes;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;
using OpenTK.Graphics.OpenGL;

namespace Agar.io_Client.Source
{
    class Client : App
    {
        NetController netController;
        World world;

        public void Start()
        {
            Initialize();
            InitializeInput();
            InitializeGraphics();
            Graphics.RenderWindow.SetTitle("Agar.io Client");

            CurrentScene = new GameScene();
            //CurrentScene = new ShaderTest();

            Run();
        }

        public override void Update()
        {
            CurrentScene.Update();
        }

        public override void Render()
        {
            Graphics.RenderWindow.Clear();
            GL.ClearColor(1f, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.LoadIdentity();
            GraphicsHelper.SetupFullscreenOrtho();

            CurrentScene.Render();

            Graphics.RenderWindow.Display();
        }
    }
}
