using System;
using System.Numerics;
using CommonLibrary;

namespace ServerProject.Entities
{
    public class Plant
    {
        public Vector2 Position;
        public int GrowthStage { get; private set; } = 0;
        private float growthTimer = 0f;
        private readonly float growthInterval; // интервал между стадиями роста
        private readonly int maxStage;
        public bool IsMature { get; private set; } = false;
        public DateTime lastUpdateTime;

        public Plant(Vector2 position, int maxStage, float growthInterval = 10f)
        {
            Position = position;
            this.maxStage = maxStage;
            this.growthInterval = growthInterval;
        }

        public void Update(float elapsedSeconds)
        {
            if (IsMature)
                return;

            // Обновление логики на сервере
            //DateTime currentTime = DateTime.UtcNow;
            //double elapsedSeconds = (currentTime - lastUpdateTime).TotalSeconds;
            //lastUpdateTime = currentTime;

            // Использование elapsedSeconds для обновления игрового состояния
            growthTimer += (float)elapsedSeconds;
            if (growthTimer >= growthInterval)
            {
                // Переход на следующую стадию роста
                growthTimer = 0f;
                GrowthStage++;
                if (GrowthStage >= maxStage)
                {
                    GrowthStage = maxStage;
                    IsMature = true;
                }
            }
        }

        // Формирование данных о состоянии растения для отправки клиенту
        public PlantState GetState()
        {
            return new PlantState
            {
                X = Position.X,
                Y = Position.Y,
                GrowthStage = GrowthStage,
                IsMature = IsMature
            };
        }
    }
}
