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

        private TiledMap map;
        private Camera2D camera;

        private Dollynho dolly;

        private SpriteFont arial;

        private readonly int scale = 2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            dolly = new Dollynho();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 176 * scale;
            graphics.PreferredBackBufferWidth = 224 * scale;

            base.Initialize();
        }

        public List<Rectangle> listaColisoes;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, graphics.PreferredBackBufferWidth / scale, graphics.PreferredBackBufferHeight / scale);
            camera = new Camera2D(viewportAdapter);

            map = this.Content.Load<TiledMap>("cut");

            listaColisoes = new List<Rectangle>();

            var blockedLayer = (TiledTileLayer)map.GetLayer("colisao");
            if (blockedLayer != null)
            {
                foreach (var tile in blockedLayer.Tiles)
                {
                    if (tile.Id != 0)
                    {
                        listaColisoes.Add(new Rectangle(tile.X * 16, tile.Y * 16, 16, 16));
                    }
                }
            }

            arial = this.Content.Load<SpriteFont>("Arial");
            camera.Move(new Vector2(0, 2380));

            dolly.LoadContent(Content, spriteBatch);
            dolly.Posicao = new Rectangle(60, 2480, 16, 16);

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private readonly int gravidade = 4;
        private readonly int alturaPulo = 56;
        private int pulo = 0;
        private bool noChao = true;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bool KeyUpPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool KeyDownPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool KeyLeftPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool KeyRightPressed = Keyboard.GetState().IsKeyDown(Keys.Right);
            bool KeyZPressed = Keyboard.GetState().IsKeyDown(Keys.Z);

            if (KeyZPressed && noChao)
            {
                pulo = alturaPulo;
                noChao = false;
            }

            if (pulo > 0 && pulo > alturaPulo/3)
            {
                dolly.Posicao = new Rectangle(dolly.Posicao.X, dolly.Posicao.Y - (gravidade), 16, 16);
                pulo -= (gravidade);
            }
            else if (pulo > alturaPulo / 3 * 2)
            {
                dolly.Posicao = new Rectangle(dolly.Posicao.X, dolly.Posicao.Y - (gravidade / 3* 2), 16, 16);
                pulo -= (gravidade / 3 * 2);
            }
            else if (pulo > 0)
            {
                dolly.Posicao = new Rectangle(dolly.Posicao.X, dolly.Posicao.Y - (gravidade/2), 16, 16);
                pulo -= (gravidade/2);
            } else
            {
                
                bool cai = true;
                foreach (Rectangle r in listaColisoes)
                {
                    if (r.Intersects(new Rectangle(dolly.Posicao.X, dolly.Posicao.Y + gravidade, 16, 16)))
                    {
                        cai = false;
                        noChao = true;
                    }
                }
                if (cai) dolly.Posicao = new Rectangle(dolly.Posicao.X, dolly.Posicao.Y + gravidade, 16, 16);
            }

            if (KeyUpPressed)
            {
            }
            
            if (KeyLeftPressed)
            {
                bool move = true;
                foreach (Rectangle r in listaColisoes)
                {
                    if (r.Intersects(new Rectangle(dolly.Posicao.X - 1, dolly.Posicao.Y, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) dolly.moveLeft();
            }
            if (KeyRightPressed)
            {
                bool move = true;
                foreach (Rectangle r in listaColisoes)
                {
                    if (r.Intersects(new Rectangle(dolly.Posicao.X + 1, dolly.Posicao.Y, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) dolly.moveRight();
            }

            dolly.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var transformMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            //spriteBatch.Begin();

            map.Draw(spriteBatch, camera);

            dolly.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
