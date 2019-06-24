using System;

namespace _5_task
{
/*
    Вомбат и человек
    Нужно избавиться от дублирующегося кода с помощью наследования -
    https://pastebin.com/2AE5txc5
*/
    abstract class Organism
    {
        public int Health { get; set; }

        public void TakeDamage(int damage)
        {
            ApplyDamage(damage);

            if (Health <= 0)
            {
                Console.WriteLine("Я умер");
            }
        }

        protected abstract void ApplyDamage(int damage);
    }
    class Wombat : Organism
    {
        public int Armor;

        protected override void ApplyDamage(int damage)
        {
            Health -= damage - Armor;
        }
    }

    class Human : Organism
    {
        public int Agility;

        protected override void ApplyDamage(int damage)
        {
            Health -= damage / Agility;
        }
    }
}
