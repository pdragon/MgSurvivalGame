using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalGame
{
    public class ClientNetworkManager
    {
        private TcpClient client;
        private NetworkStream stream;
        // Локальное состояние мира, полученное от сервера
        public List<PlantState> PlantsState { get; private set; } = new List<PlantState>();

        public async Task Connect(string ip, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port);
            stream = client.GetStream();
            Console.WriteLine("Подключено к серверу: " + ip + ":" + port);
            ReceiveLoop();
        }

        private async void ReceiveLoop()
        {
            byte[] buffer = new byte[4096];
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // Обновляем локальное состояние мира
                PlantsState = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlantState>>(json);
            }
        }

        // Отправка сообщения серверу
        public async Task Send(string message)
        {
            if (stream != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
            }
        }
    }
}
