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
        private IPlayerHealthLoader _healthLoader;

        public float Health => _health;

        public Player(float health, float armor, int id, IPlayerHealthLoader healthLoader)
        {
            _health = health;
            _armor = armor;
            _id = id;
            _healthLoader = healthLoader;

            _health = _healthLoader.GetStartHealth(_id, _health);
        }

        public void ApplyDamage(float damage)
        {
            float healthDelta = damage - _armor;
            _health -= healthDelta;
            _armor /= 2;

            Console.WriteLine($"Вы получили урона - {healthDelta}");

            _healthLoader.SetNewHealth(_id, _health);

        }
    }
    interface IPlayerHealthLoader
    {
        float GetStartHealth(int id, float defaultHealth);
        void SetNewHealth(int id, float health);
    }

    class PlayerHealthFileLoader : IPlayerHealthLoader
    {
        public float GetStartHealth(int id, float defaultHealth)
        {
            if(File.Exists(($"user_{id}.data")))
            {
                var data = File.ReadAllText($"user_{id}.data");
                return float.Parse(data);
            }
            return defaultHealth;
        }

        public void SetNewHealth(int id, float health)
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
