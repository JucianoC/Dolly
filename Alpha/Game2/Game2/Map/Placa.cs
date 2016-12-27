using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Map
{
    class Placa
    {
        private String message;
        private Rectangle posicao;
        private Texture2D sprite;

        public Placa(String message, Rectangle posicao, Texture2D sprite)
        {
            this.message = message;
            this.posicao = posicao;
            this.sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, posicao, Color.White);
        }
    }
}
