using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json; // или любая другая библиотека для JSON
using ServerProject.Entities;
using CommonLibrary;
using System.Numerics;

namespace SurvivalGame.Server
{
    public class GameServer
    {
        private List<Plant> plants = new List<Plant>();
        private TcpListener? listener;
        private bool isRunning;
        private const int port = 5555;
        private DateTime LastUpdateTime = new DateTime();

        public void Start()
        {
            // Инициализация мира (например, добавим несколько растений)
            plants.Add(new Plant(new Vector2(300, 300), maxStage: 3, growthInterval: 5f));
            // ... можно добавить дополнительные растения

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            isRunning = true;
            AcceptClients();

            // Игровой цикл сервера (упрощённый)
            while (isRunning)
            {
                UpdateWorld();
                SendWorldStateToClients();
                System.Threading.Thread.Sleep(50);
            }
        }

        private void UpdateWorld()
        {
            // Здесь обновляем каждое растение, NPC и т.п.
            DateTime currentTime = DateTime.UtcNow;
            double elapsedSeconds = (currentTime - LastUpdateTime).TotalSeconds;
            //var gameTime = (float)elapsedSeconds;
            foreach (var plant in plants)
            {
                plant.Update((float)elapsedSeconds);
            }
        }

        // Примерная реализация отправки данных клиентам
        private void SendWorldStateToClients()
        {
            // Собираем состояния растений
            List<PlantState> state = new List<PlantState>();
            foreach (var plant in plants)
            {
                state.Add(plant.GetState());
            }

            // Сериализуем в JSON
            string json = JsonConvert.SerializeObject(state);

            // Посылаем клиентам (здесь пример для одного клиента)
            // В реальном проекте реализуйте список подключённых клиентов и отправку каждому
            // Например:
            // foreach (var client in connectedClients)
            //    client.Send(json);
        }

        private async void AcceptClients()
        {
            while (isRunning)
            {
                if(listener == null)
                {
                    //TODO: Отправка в журнал
                    Console.WriteLine("Error!!! listener is empty");
                }
                TcpClient client = await listener!.AcceptTcpClientAsync();
                // Запускаем задачу для обработки клиента
                HandleClient(client);
            }
        }

        private async void HandleClient(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                // Пример: бесконечный цикл общения с клиентом
                byte[] buffer = new byte[1024];
                while (isRunning)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    // Разбор входящих сообщений (например, команды игрока)
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("От клиента получено: " + request);
                }
            }
        }
    }
}
