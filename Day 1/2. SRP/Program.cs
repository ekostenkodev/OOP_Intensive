using System;
using System.IO;

namespace SRP
{
    /*
        Разделить ответственности классов

        Воспользуйтесь принципов SRP и выделите ответственность сохранения\загрузки в другой класс

        https://gist.github.com/HolyMonkey/a2eda8d353918a43d25e44ad1f6be0aa
    */
    interface IPlayerBuilder
    {
        Player CreatePlayer(float health, float armor, int id);
        void OnHealthChanged(Player player);
        float? GetStartHealth(int id);
    }
    class PlayerDataFileBuilder : IPlayerBuilder
    {
        public Player CreatePlayer(float health, float armor, int id)
        {
            health = GetStartHealth(id) ?? health;
            Player player = new Player(health, armor, id);
            player.HealthChanged += OnHealthChanged;

            return player;
        }

        public void OnHealthChanged(Player player)
        {
            File.WriteAllText($"user_{player.Id}.data", player.Health.ToString());
        }

        public float? GetStartHealth(int id)
        {
            if (File.Exists(($"user_{id}.data")))
            {
                var data = File.ReadAllText($"user_{id}.data");
                return float.Parse(data);
            }
            return null;
        }
    }

    class Player
    {
        private float _health;
        private float _armor;
        private int _id;

        public float Health => _health;
        public float Id => _id;

        public event Action<Player> HealthChanged;

        public Player(float health, float armor, int id)
        {
            _health = health;
            _armor = armor;
            _id = id;
        }

        public void ApplyDamage(float damage)
        {
            float healthDelta = damage - _armor;
            _health -= healthDelta;
            _armor /= 2;

            Console.WriteLine($"Вы получили урона - {healthDelta}");

            HealthChanged?.Invoke(this);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
