using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalGame
{
    public class Camera2D
    {
        private readonly GraphicsDevice graphicsDevice;
        public Vector2 Position { get; private set; }
        public float Zoom { get; set; } = 1f;
        public float Rotation { get; set; } = 0f;

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Position = Vector2.Zero;
        }

        public Matrix GetTransformation()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        }

        public void Follow(Vector2 targetPosition)
        {
            Position = targetPosition;
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Matrix inverse = Matrix.Invert(GetTransformation());
            return Vector2.Transform(screenPosition, inverse);
        }
    }
}

