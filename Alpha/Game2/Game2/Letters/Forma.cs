using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Letters
{
    class Forma
    {

        private int tamanho;
        private List<char> letras = new List<char>();
        private Dictionary<String, String> escreviveis;

        private Rectangle posicao;
        public Rectangle Posicao
        {
            set { posicao = value; }
            get
            {
                return posicao;
            }
        }

        private Texture2D sprite;

        public Forma(int tamanho, Rectangle posicao, Texture2D sprite)
        {
            this.tamanho = tamanho;
            this.posicao = posicao;
            this.sprite = sprite;

            escreviveis = new Dictionary<String, String>();

            escreviveis.Add("FAZ", "Alpha.Letters.escreviveis.Faz");

        }

        public void putLetter(char letter){
            letras.Add(letter);
        }

        //TODO escolher um jeito de implementar essa coisa
        public Escrevivel forge()
        {
            String chave = "";
            foreach (char l in letras){
                chave += l;
            }

            if (escreviveis.ContainsKey(chave))
            {
                Type t = Type.GetType(escreviveis[chave]); 
                return  (Escrevivel)Activator.CreateInstance(t);
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont fonte, Vector2 offset)
        {
 	        spriteBatch.Draw(sprite, posicao, Color.White);
            int x = 10;
            foreach (char c in letras){
                spriteBatch.DrawString(fonte, c.ToString(), new Vector2(posicao.X + x, posicao.Y), Color.Green);
                x += 18;
            }
        }
    }
}
