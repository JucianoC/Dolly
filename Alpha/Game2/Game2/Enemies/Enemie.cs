using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Enemies
{
    class Enemie
    {

        private Texture2D bloco;
        private Rectangle posicao = new Rectangle(110, 130, 16, 16);

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //sprites
            bloco = content.Load<Texture2D>("bloco");
        }
        public Rectangle getPosition()
        {
            return posicao;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bloco, posicao, null, Color.Purple, 0, new Vector2(0, 0), SpriteEffects.None, 1f);
        }
    }
}
