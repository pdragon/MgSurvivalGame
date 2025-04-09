using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalGame;
using System.Threading.Tasks;


namespace SurvivalGame
{
    public class MGGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Клиентские компоненты
        Camera2D camera;
        TileMap tileMap;
        Player player;
        ClientNetworkManager networkManager;

        public MGGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // Инициализация камеры
            camera = new Camera2D(GraphicsDevice);
            // Инициализация игрока
            player = new Player(new Vector2(100, 100));
            // Инициализация карты
            tileMap = new TileMap(30, 30, 128, 72);

            // Инициализация сетевого менеджера
            networkManager = new ClientNetworkManager();
            // Подключаемся к серверу (127.0.0.1:5555) асинхронно
            Task.Run(async () =>
            {
                await networkManager.Connect("127.0.0.1", 5555);
            });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            tileMap.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Выход по Esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Обработка клика мыши – перемещение и отправка запроса на сервер
            HandleMouseInput();

            player.Update(gameTime);
            camera.Follow(player.Position);
            base.Update(gameTime);
        }

        private void HandleMouseInput()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Vector2 worldPos = camera.ScreenToWorld(new Vector2(mouse.X, mouse.Y));
                player.MoveTo(worldPos);

                // Отправляем команду перемещения на сервер
                // Формат сообщения может расширяться – пока отправляем координаты
                Task.Run(async () =>
                {
                    await networkManager.Send("MOVE " + worldPos.X + " " + worldPos.Y);
                });
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: camera.GetTransformation());
            tileMap.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
