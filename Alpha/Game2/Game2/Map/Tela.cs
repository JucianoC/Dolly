using Alpha.Enemies;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Map
{
    class Tela
    {
        public List<Placa> placas;
        public List<Enemie> enemies;
        public List<Rectangle> mapTileColliderBoxes;

        public static void LoadTiles(TiledMap map, Vector2 TileSize, Dictionary<Vector2, Tela> telas)
        {
            var blockedLayer = (TiledTileLayer)map.GetLayer("tiles1");
            if (blockedLayer != null) {
                foreach (var tile in blockedLayer.Tiles)
                {
                    if (tile.Id != 0)
                    {
                        Vector2 posicaoTela = new Vector2((tile.X * (int)TileSize.X) / 256, tile.Y * (int)TileSize.Y / 176);

                        List<Rectangle> tileColliderBoxes = new List<Rectangle>();
                        Tela tela;

                        if (telas.ContainsKey(posicaoTela))
                        {
                            tela = telas[posicaoTela];
                            tileColliderBoxes = tela.mapTileColliderBoxes;
                        }
                        else
                        {
                            tela = new Tela();
                            tela.mapTileColliderBoxes = tileColliderBoxes;
                            telas.Add(posicaoTela, tela);
                        }

                        tileColliderBoxes.Add(new Rectangle(tile.X * (int)TileSize.X, tile.Y * (int)TileSize.Y, (int)TileSize.X, (int)TileSize.Y));
                    }
                }
            }
        }
    }
}
