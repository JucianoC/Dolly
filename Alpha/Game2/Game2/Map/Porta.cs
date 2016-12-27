using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Map
{
    public class Porta
    {
        private String nome;
        public String Nome
        {
            get { return nome; }
        }
        private Rectangle posicao;

        public Rectangle Posicao
        {
            set { posicao = value; }
            get
            {
                return posicao;
            }
        }

        private String destino;
        public String Destino
        {
            get { return destino; }
        }
        public Porta( String nome, Rectangle posicao, String destino)
        {
            this.nome = nome;
            this.posicao = posicao;
            this.destino = destino;
        }
    }
}
