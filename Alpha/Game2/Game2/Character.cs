using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Alpha
{
    class Character
    {

        private Texture2D bloco;
        private Texture2D axe;
        private Rectangle posicao = new Rectangle(120, 80, 16, 16);

        public Rectangle Posicao { get { return posicao;} set { posicao = value;} }

        public void LoadContent(ContentManager content)
        {
            //sprites
            bloco = content.Load<Texture2D>("bloco");
            axe = content.Load<Texture2D>("axe");
        }

        private float rotacaoI = 44f;
        private float rotacaoF = 47f;
        private float rotacao = 44f;
        private float rotacaoInc = 0f;

        private bool lastKeyXPressed = false;

        private Vector2 origem = new Vector2(0, 16);

        private int velocidade = 2;

        private Vector2 getTelaAtual(Rectangle posicao)
        {
            return new Vector2(posicao.X / 256, posicao.Y / 176);
        }

        public void Update(Dictionary<Vector2, List<Rectangle>> mapTileColliderBoxes)
        {

            //movimento do bloco
            bool KeyUpPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool KeyDownPressed = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool KeyLeftPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool KeyRightPressed = Keyboard.GetState().IsKeyDown(Keys.Right);

            //movimento da tela
            Vector2 tela = getTelaAtual(posicao);

            List<Rectangle> tileColliderBoxes = mapTileColliderBoxes[tela];

            if (KeyRightPressed)
            {
                posicao.X += velocidade;

                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(posicao.X, posicao.Y, 16, 16)))
                    {
                        posicao.X -= velocidade;
                    }
                }
            }
            if (KeyLeftPressed)
            {
                posicao.X -= velocidade;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(posicao.X, posicao.Y, 16, 16)))
                    {
                        posicao.X += velocidade;
                    }
                }
            }
            if (KeyUpPressed)
            {
                posicao.Y -= velocidade;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(posicao.X, posicao.Y, 16, 16)))
                    {
                        posicao.Y += velocidade;
                    }
                }
            }
            if (KeyDownPressed)
            {
                posicao.Y += velocidade;
                foreach (Rectangle r in tileColliderBoxes)
                {
                    if (r.Intersects(new Rectangle(posicao.X, posicao.Y, 16, 16)))
                    {
                        posicao.Y -= velocidade;
                    }
                }
            }

            //atk            
            if (Keyboard.GetState().IsKeyDown(Keys.Z) || (!lastKeyXPressed && Keyboard.GetState().IsKeyDown(Keys.X)))
            {
                rotacaoInc = 3f;
            }

            if (rotacaoInc > 0f)
            {
                rotacao += 0.3f;
                rotacaoInc -= 0.3f;

                if (rotacao > rotacaoF)
                {
                    rotacaoInc = 0f;
                    rotacao = rotacaoI;
                }
            }
            lastKeyXPressed = Keyboard.GetState().IsKeyDown(Keys.X);

            
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                origem.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                origem.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                origem.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                origem.Y -= 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bloco, posicao, Color.Blue);
            spriteBatch.Draw(axe, new Rectangle(posicao.X + 16, posicao.Y + 16, 10, 16), null, Color.White, rotacao, origem, SpriteEffects.None, 1f);
        }
    }
}
