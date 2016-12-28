using Game4.dollynho;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Scene
    {
        private TiledMap map;
        private Camera2D camera;
        private Dollynho dolly;
        private SpriteFont arial;
        public List<Rectangle> listaColisoes;

        private bool LastKeyUpPressed = false;
        private bool LastKeyDownPressed = false;
        private bool LastKeyLeftPressed = false;
        private bool LastKeyRightPressed = false;
        private bool LastKeyZPressed = false;

        public Scene()
        {
            this.dolly = new Dollynho();
        }

        public void LoadContent(ContentManager content, Camera2D camera, SpriteBatch spriteBatch)
        {

            this.camera = camera;

            map = content.Load<TiledMap>("cut");

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

            arial = content.Load<SpriteFont>("Arial");
            camera.Move(new Vector2(0, 2380));

            dolly.LoadContent(content, spriteBatch);
            dolly.Posicao = new Rectangle(new Point(60, 2480), dolly.TamanhoBoneco);
        }

        public void Update(GameTime gameTime)
        {
            bool KeyUpPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool KeyDownPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool KeyLeftPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool KeyRightPressed = Keyboard.GetState().IsKeyDown(Keys.Right);
            bool KeyZPressed = Keyboard.GetState().IsKeyDown(Keys.Z);

            Rectangle retanguloDolly = dolly.Posicao;


            if (KeyLeftPressed)
            {
                bool move = true;

                retanguloDolly.X -= 1;
                dolly.moveLeft();
                
                foreach (Rectangle r in listaColisoes)
                {
                    if (r.Intersects(retanguloDolly))
                    {
                        move = false;
                    }
                }
                if ( ! move)
                {
                    retanguloDolly.X += 1;
                }
            }
            if (KeyRightPressed)
            {
                bool move = true;

                retanguloDolly.X += 1;
                dolly.moveRight();

                foreach (Rectangle r in listaColisoes)
                {
                    if (r.Intersects(retanguloDolly))
                    {
                        move = false;
                    }
                }
                if (!move)
                {
                    retanguloDolly.X -= 1;
                }
            }

            if (!KeyRightPressed && !KeyLeftPressed && (!LastKeyLeftPressed || !LastKeyRightPressed))
            {
                if (LastKeyLeftPressed)
                {
                    dolly.setIdleAnimmationLeft();
                }
                else if (LastKeyRightPressed)
                {
                    dolly.setIdleAnimmationRight();
                }
            }

            LastKeyDownPressed = KeyDownPressed;
            LastKeyLeftPressed = KeyLeftPressed;
            LastKeyRightPressed = KeyRightPressed;
            LastKeyUpPressed = KeyUpPressed;
            LastKeyZPressed = KeyZPressed;


            dolly.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: transformMatrix);

            map.Draw(spriteBatch, camera);

            dolly.Draw(spriteBatch);

            spriteBatch.End();
        }

    }
}
