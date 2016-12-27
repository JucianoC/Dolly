using Alpha.Enemies;
using Alpha.HUD;
using Alpha.Letters;
using Alpha.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System.Collections.Generic;

namespace Alpha
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private TiledMap map;
        private Camera2D camera;

        private Texture2D bloco;

        //TODO trocar a lista de retangulos por objetos referentes a cada tela do mapa
        private Dictionary<Vector2, Tela> telas;

        private Vector2 TileSize = new Vector2(16, 16);
        private SpriteFont fonte;

        private Character alpha;
        private Enemie enemie;
        private List<Porta> portas;
        private List<Forma> formas;

        private Hud hud;

        private readonly int scale = 2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            telas = new Dictionary<Vector2, Tela>();
            graphics.PreferredBackBufferHeight = 176 * scale;
            graphics.PreferredBackBufferWidth = 256 * scale;

            alpha = new Character();
            enemie = new Enemie();
            portas = new List<Porta>();
            formas = new List<Forma>();
            hud = new Hud();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bloco = Content.Load<Texture2D>("bloco");

            //camera
            var viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, graphics.PreferredBackBufferWidth / scale, graphics.PreferredBackBufferHeight / scale);
            camera = new Camera2D(viewportAdapter);

            alpha.LoadContent(this.Content, this.spriteBatch);
            enemie.LoadContent(this.Content, this.spriteBatch);

            fonte = this.Content.Load<SpriteFont>("Arial");

            //mapa
            map = this.Content.Load<TiledMap>("colisionTest1");

            Tela.LoadTiles(map, TileSize, telas);

            foreach (TiledObject porta in map.GetObjectGroup("Porta").Objects)
            {
                portas.Add(new Porta(porta.Name, new Rectangle((int)(porta.X), (int)(porta.Y), (int)TileSize.X, (int)TileSize.Y), porta.Properties["Destino"]));
            }
            Texture2D formaTexture = Content.Load<Texture2D>("scroll");
            foreach (TiledObject forma in map.GetObjectGroup("Forma").Objects)
            {
                formas.Add(new Forma(int.Parse(forma.Properties["tamanho"]), new Rectangle((int)(forma.X), (int)(forma.Y), (int)TileSize.X, (int)TileSize.Y), formaTexture));
            }
            
            alpha.Posicao = new Rectangle((int)map.GetObjectGroup("Alpha").Objects[0].X, (int)map.GetObjectGroup("Alpha").Objects[0].Y, 16, 16);
        }

        protected override void UnloadContent()
        {
        }

        Vector2 cameraMove = new Vector2(0, 0);
        Vector2 telaAtual = new Vector2(0, 0);

        private bool lastKeyXPressed = false;
        private bool lastAtak = false;
        private bool lastNaPorta = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //movimento do bloco
            bool KeyUpPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool KeyDownPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool KeyLeftPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool KeyRightPressed = Keyboard.GetState().IsKeyDown(Keys.Right);

            alpha.Update(gameTime);

            //movimento da tela
            Vector2 tela = getTelaAtual(alpha.Posicao);
            List<Rectangle> tileColliderBoxes = telas[tela].mapTileColliderBoxes;
            if (KeyRightPressed)
            {
                bool move = true;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(alpha.Posicao.X +1, alpha.Posicao.Y, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) alpha.moveRight();
            }
            if (KeyLeftPressed)
            {
                bool move = true;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(alpha.Posicao.X - 1, alpha.Posicao.Y, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) alpha.moveLeft();
            }
            if (KeyUpPressed)
            {
                bool move = true;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(alpha.Posicao.X, alpha.Posicao.Y -1, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) alpha.moveUp();
            }
            if (KeyDownPressed)
            {
                bool move = true;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(alpha.Posicao.X, alpha.Posicao.Y + 1, 16, 16)))
                    {
                        move = false;
                    }
                }
                if (move) alpha.moveDown();
            }

            if (!KeyRightPressed && !KeyLeftPressed && !KeyUpPressed && !KeyDownPressed)
            {
                alpha.setIdleAnimation();
            }


            foreach (Forma f in formas)
            {
                if (f.Posicao.Intersects(alpha.Posicao))
                {
                    alpha.Forma = f;
                }
            }

            //portas
            string nomePorta = null;
            bool naPorta = false;
            foreach (Porta p in portas)
            {
                if (p.Posicao.Intersects(alpha.Posicao))
                {
                    naPorta = true;
                    nomePorta = p.Destino;
                }
            }


            if (naPorta && !lastNaPorta)
            foreach (Porta d in portas)
            {
                if (nomePorta.Equals(d.Nome))
                {
                    alpha.Posicao = d.Posicao;
                    break;
                }
            }

            lastNaPorta = naPorta;

            //atk
            if ((!lastKeyXPressed && Keyboard.GetState().IsKeyDown(Keys.X)))
            {
                alpha.atk();
            }
            lastKeyXPressed = Keyboard.GetState().IsKeyDown(Keys.X);

            bool atk = false;

            foreach (Rectangle r in alpha.getAtk())
            {
                if (r.Intersects(enemie.getPosition()))
                {
                    atk = true;
                    break;
                }
            }

            if (atk && !lastAtak)
            {
                teste += "ai ";
                lastAtak = true;
            }

            lastAtak = atk;

            //camera
            if (tela != telaAtual)
            {
                if (tela.X != telaAtual.X)
                {
                    if (tela.X < telaAtual.X)
                    {
                        cameraMove = new Vector2(-256 * (telaAtual.X - tela.X), cameraMove.Y);
                    } 
                    else
                    {
                        cameraMove = new Vector2(256 * (tela.X - telaAtual.X), cameraMove.Y);
                    }
                } 
                if (tela.Y != telaAtual.Y) 
                {
                    if (tela.Y < telaAtual.Y)
                    {
                        cameraMove = new Vector2(cameraMove.X, -176 * (telaAtual.Y - tela.Y));
                    }
                    else
                    {
                        cameraMove = new Vector2(cameraMove.X, 176 * (tela.Y - telaAtual.Y));
                    }
                }
                telaAtual = tela;
            }

            Vector2 cameraMoveFrame = new Vector2(0, 0);

            if (cameraMove.Y > 0)
            {
                cameraMove.Y -= 16;
                cameraMoveFrame.Y += 16;
            }
            if (cameraMove.Y < 0)
            {
                cameraMove.Y += 16;
                cameraMoveFrame.Y -= 16;
            }
            if (cameraMove.X > 0)
            {
                cameraMove.X -= 16;
                cameraMoveFrame.X += 16;
            }
            if (cameraMove.X < 0)
            {
                cameraMove.X += 16;
                cameraMoveFrame.X -= 16;
            }

            camera.Move(cameraMoveFrame);
        }

        private Vector2 getTelaAtual(Rectangle posicao)
        {
            return new Vector2(posicao.X / 256, posicao.Y / 176);
        }
        
        private string teste = "";
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            var transformMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);

            map.Draw(spriteBatch, camera);

            alpha.Draw(spriteBatch);
            enemie.Draw(spriteBatch);

            foreach (Forma f in formas)
            {
                f.Draw(spriteBatch, fonte, camera.Position);
            }

            hud.Draw(spriteBatch, fonte, camera.Position, alpha.Letras);

            foreach (Rectangle r in alpha.getAtk())
            {
                spriteBatch.Draw(bloco, r, Color.Blue); 
            }

            spriteBatch.DrawString( fonte, teste, new Vector2(10, 10), Color.Red);

            //spriteBatch.Draw(bloco, alpha.Posicao, Color.Blue);

            /*Vector2 tela = getTelaAtual(alpha.Posicao);
            List<Rectangle> tileColliderBoxes = telas[tela].mapTileColliderBoxes;
            foreach (Rectangle r in tileColliderBoxes)
            {
                spriteBatch.Draw(bloco, r, Color.Purple); 
            }
            spriteBatch.Draw(bloco, alpha.Posicao, Color.Red); 
            */

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
