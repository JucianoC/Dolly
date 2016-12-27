using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.HUD
{
    class Hud
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont fonte, Vector2 position, List<char> letras)
        {
            int y = 10;

            foreach (char l in letras)
            {
                spriteBatch.DrawString(fonte, l.ToString(), new Vector2(position.X + 10, position.Y + y), Color.Black);
                y += 18;
            }
        }
    }
}
