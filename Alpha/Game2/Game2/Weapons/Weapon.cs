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
    public interface Weapon : Escrevivel
    {
        Texture2D getSprite();
        void setSprite(Texture2D sprite);
        void setPosicao(Rectangle posicao);

        void Draw(SpriteBatch spriteBatch);

        void setUp();
        void setDown();
        void setLeft();
        void setRight();

        void Update();

        void Atk();
        List<Rectangle> getAtk();
    }
}
