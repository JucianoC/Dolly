using Alpha.Letters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Weapons
{
    public class Axe : Weapon
    {
        private Texture2D sprite;
        private Rectangle posicao;

        public void setSprite(Texture2D sprite)
        {
            this.sprite = sprite;
        }
        public Texture2D getSprite()
        {
           return sprite;
        }
        public void setPosicao(Rectangle posicao)
        {
            this.posicao = posicao;
        }

        private static readonly float meioPI = (float)(Math.PI / 2.0);

        //frente
        private float rotacaoInitDownAtk = meioPI;
        private float rotacaoEndDown = (float)(meioPI + Math.PI);

        //tras
        private float rotacaoInitUpAtk = (float)(meioPI + Math.PI);
        private float rotacaoEndUp = (float)(meioPI + (Math.PI * 2));

        //esquerda
        private float rotacaoInitLeftAtk = (float)(Math.PI);
        private float rotacaoEndLeft = (float)(Math.PI * 2);

        //direita
        private float rotacaoInitRightAtk = 0;
        private float rotacaoEndRight = (float)(Math.PI);

        private float rotacaoA = 0;
        private float rotacaoI = 0;
        private float rotacaoE = (float)Math.PI;
        private float rotacao = 0;

        private float rotacaoInc = 0f;
        private float atkVel = 0.21f;

        public void Update()
        {
            if (rotacaoInc > 0f)
            {
                rotacao += atkVel;
                rotacaoInc -= atkVel;

                if (rotacao >= rotacaoE)
                {
                    rotacaoInc = 0f;
                    rotacao = rotacaoI;
                }
            }
        }

        public Vector2 origem = new Vector2(-8, 40);
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle(posicao.X, posicao.Y + 8, 10, 16), null, Color.White, rotacao, origem, SpriteEffects.None, 1f);
        }
        public EscrevivelType getType()
        {
            return EscrevivelType.WEAPON;
        }

        public void setUp()
        {
            rotacaoA = rotacaoInitUpAtk;
            rotacaoE = rotacaoEndUp;
            rotacaoI = 0;
        }
        public void setDown()
        {
            rotacaoA = rotacaoInitDownAtk;
            rotacaoE = rotacaoEndDown;
            rotacaoI = 0;
        }
        public void setLeft()
        {
            rotacaoA = rotacaoInitLeftAtk;
            rotacaoE = rotacaoEndLeft;
            rotacaoI = 0;
        }
        public void setRight()
        {
            rotacaoA = rotacaoInitRightAtk;
            rotacaoE = rotacaoEndRight;
            rotacaoI = 0;
        }
        public void Atk()
        {
            rotacao = rotacaoA;
            rotacaoInc = 3f;
        }
        public List<Rectangle> getAtk()
        {
            double x = Math.Cos(rotacao - meioPI);
            double y = Math.Sin(rotacao - meioPI);

            List<Rectangle> retorno = new List<Rectangle>();

            retorno.Add(new Rectangle(posicao.X + (int)Math.Round((x * 21.0)), posicao.Y + 8 + (int)Math.Round((y * 21.0)), 1, 1));
            retorno.Add(new Rectangle(posicao.X + (int)Math.Round((x * 20.0)), posicao.Y + 8 + (int)Math.Round((y * 20.0)), 1, 1));
            retorno.Add(new Rectangle(posicao.X + (int)Math.Round((x * 18.0)), posicao.Y + 8 + (int)Math.Round((y * 18.0)), 1, 1));
            retorno.Add(new Rectangle(posicao.X + (int)Math.Round((x * 16.0)), posicao.Y + 8 + (int)Math.Round((y * 16.0)), 1, 1));

            return retorno;
        }
    }
}
