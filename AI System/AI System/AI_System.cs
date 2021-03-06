using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Sandi_s_Way;
using Debugging;
using C3.XNA;

namespace AI_System
{
    public class AI_System_Graphics : Microsoft.Xna.Framework.Game
    {
        //Basic game info:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static GraphicsDevice Device;

        //The debug console:
        public static DebugConsole Console;

        //Game object references:
        public static Grid ObjectGrid;

        public AI_System_Graphics()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Device = graphics.GraphicsDevice;

            //Initialize game info:
            graphics.PreferredBackBufferWidth = 620;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "AI System";

            //Initialize the static classes:
            ObjectManager.Initialize();

            //Initialize the debug console:
            Console = new DebugConsole(spriteBatch, new Vector2(0, 0));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Initialize the GameInfo:
            GameInfo.RefSpriteBatch = spriteBatch;
            GameInfo.RefDevice = Device;
            GameInfo.RefDeviceManager = graphics;
            GameInfo.RefContent = Content;
            GameInfo.RefConsole = Console;

            //Initialize the texture container:
            TextureContainer.Initialize();

            //Load fonts:
            Console.Font = Content.Load<SpriteFont>("DebuggConsoleFont");

            //Load default textures:
            TextureContainer.DefaultTextures[typeof(Life)] = TextureContainer.AddTextureAndReturn("LifeCell");
            TextureContainer.DefaultTextures[typeof(TestObject)] = TextureContainer.AddTextureAndReturn("GenericObject");

            //Make game objects:
            Grid grid = new Grid(new Vector2(30, 30), new Dictionary<Type, char>(), 5, 10, 50);
            ObjectManager.InstantImportExisting(grid);

            //Make refreneces:
            ObjectGrid = grid;

            //Test it:

            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        grid.Objects[i, j] = ObjectManager.InstantCreateAndReturn(typeof(TestObject), Vector2.Zero);
            //    }
            //}

            grid.Objects[3, 4] = ObjectManager.InstantCreateAndReturn(typeof(TestObject), Vector2.Zero);

            ObjectGrid.SetPosition(ObjectManager.Get(typeof(TestObject))[0], new Vector2(2, 1));
            ObjectGrid.RelativeSetPosition(ObjectManager.Get(typeof(TestObject))[0], new Vector2(-1, 0));

            Console.WriteLine(ObjectGrid.GetXY(ObjectManager.Get(typeof(TestObject))[0]).ToString());
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            ObjectManager.UpdateAll();
            Console.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            ObjectManager.DrawAll();
            Console.WriteConsole();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
