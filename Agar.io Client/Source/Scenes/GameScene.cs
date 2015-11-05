using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agar.io_Client.Source.PlayerControllers;
using DeltaEpsilon.Engine;
using DeltaEpsilon.Engine.Utils;

namespace Agar.io_Client.Source.Scenes
{
    class GameScene : Scene
    {
        NetController netController;
        World world;
        PlayerController playerController;

        public GameScene()
        {
            var server = ServerManager.GetServer();
            Log.Print($"Address: {server.Item1}");
            Log.Print($"Token: {server.Item2}");

            netController = new NetController();
            netController.Connect(server.Item1, server.Item2);

            world = new World();

            playerController = new InputController();
        }

        public override void Update()
        {
            netController.Update();
            world.Update();
            playerController.UpdateInternal();
        }

        public override void Render()
        {
            world.Render();
            playerController.Render();
        }
    }
}
