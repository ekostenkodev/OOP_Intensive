using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_task
{
    /*
        Разделить ответственности классов

        Воспользуйтесь принципов SRP и выделите ответственность сохранения\загрузки в другой класс

        https://gist.github.com/HolyMonkey/a2eda8d353918a43d25e44ad1f6be0aa
    */

    class Player
    {
        private float _health;
        private float _armor;
        private int _id;

        public float Id => _id;
        public float Health => _health;

        public event Action<Player> Instantiate;
        public event Action<int,float> HealthChanged;

        public Player(float health, float armor, int id)
        {
            _health = health;
            _armor = armor;
            _id = id;

            Instantiate?.Invoke(this);
        }

        public void ApplyDamage(float damage)
        {
            float healthDelta = damage - _armor;
            _health -= healthDelta;
            _armor /= 2;

            Console.WriteLine($"Вы получили урона - {healthDelta}");

            HealthChanged.Invoke(_id,_health);
        }
    }
    class PlayerFile
    {
        public PlayerInfoFile(Player player)
        {
            player.HealthChanged += onHealthChanged;
            player.Instantiate += onInstantiate;
        }
        private void onInstantiate(Player player)
        {
            if (File.Exists(($"user_{player.Id}.data")))
            {
                var data = File.ReadAllText($"user_{player.Id}.data");
                player.Health = float.Parse(data);
            }
        }
        private void onHealthChanged(int id,float health)
        {
            File.WriteAllText($"user_{id}.data", health.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
