using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalGame
{
    public class TileMap
    {
        private int width, height;
        private int tileWidth, tileHeight;
        private Texture2D tileTexture;
        // Пример массива для хранения типа плитки (можно расширять)
        private int[,] tiles;

        public TileMap(int width, int height, int tileWidth, int tileHeight)
        {
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;

            // Генерация простого случайного мира
            tiles = new int[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    tiles[x, y] = (x + y) % 2; // пример паттерна
        }

        public void LoadContent(ContentManager content)
        {
            // Загрузите свою текстуру плитки; можно создать простую заливку
            tileTexture = content.Load<Texture2D>("tile"); // предположительно файл tile.png в Content
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Изометрическая отрисовка плиток.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 isoPos = ConvertToIsometric(new Vector2(x, y));
                    spriteBatch.Draw(tileTexture, isoPos, Color.White);
                }
            }
        }

        private Vector2 ConvertToIsometric(Vector2 tilePos)
        {
            // Простейшее преобразование: настроить по необходимости
            float isoX = (tilePos.X - tilePos.Y) * (tileWidth / 2f);
            float isoY = (tilePos.X + tilePos.Y) * (tileHeight / 2f);
            return new Vector2(isoX, isoY);
        }
    }
}
