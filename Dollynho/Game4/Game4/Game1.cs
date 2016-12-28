using Game4.dollynho;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System.Collections.Generic;


namespace Game4
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Scene scene;
        private readonly int scale = 2;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);            
            Content.RootDirectory = "Content";

            scene = new Scene();
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 176 * scale;
            graphics.PreferredBackBufferWidth = 224 * scale;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, graphics.PreferredBackBufferWidth / scale, graphics.PreferredBackBufferHeight / scale);
            Camera2D camera = new Camera2D(viewportAdapter);

            this.scene.LoadContent(Content, camera, spriteBatch);

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            this.scene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.scene.Draw(spriteBatch);   
            base.Draw(gameTime);
        }
    }
}
