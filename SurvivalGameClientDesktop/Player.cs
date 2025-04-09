using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalGame
{
    public class Player
    {
        public Vector2 Position;
        private Texture2D texture;
        private Vector2 targetPosition;
        private float moveSpeed = 200f; // пикселей в секунду

        public Player(Vector2 startPosition)
        {
            Position = startPosition;
            targetPosition = startPosition;
        }

        public void LoadContent(ContentManager content)
        {
            // Загрузите спрайт игрока (например, "player.png" должен быть в Content)
            texture = content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float distance = Vector2.Distance(Position, targetPosition);

            // Если расстояние до цели больше минимального порога, выполняем движение
            if (distance > 1f)
            {
                Vector2 direction = targetPosition - Position;
                if (direction != Vector2.Zero)
                    direction.Normalize();

                // Если оставшийся шаг меньше чем перемещение за кадр, ставим позицию напрямую в целевое положение
                if (distance < moveSpeed * dt)
                {
                    Position = targetPosition;
                }
                else
                {
                    Position += direction * moveSpeed * dt;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Отрисовка игрока с центровкой спрайта
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void MoveTo(Vector2 newTarget)
        {
            targetPosition = newTarget;
        }
    }
}
