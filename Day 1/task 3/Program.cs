using System;
using System.Linq;

namespace task_3
{
    /*
        Избавьтесь от дублирующего кода

        https://pastebin.com/cNQggMPK
    */

    class GoalList
    {
        public string Name { get; private set; }
        private string[] _goals;
        public int Length => _goals.Length;

        public string GetItem(int index)
        {
            if (_goals.Length > index)
                return _goals[index];
            else
                return "Empty";
        }
        public void AddItem(string item)
        {
            string[] newGoalArray = new string[_goals.Length + 1];
            for (int j = 0; j < _goals.Length; j++)
            {
                newGoalArray[j] = _goals[j];
            }
            newGoalArray[newGoalArray.Length - 1] = item;
            _goals = newGoalArray;
        }

        public GoalList(string name)
        {
            Name = name;
            _goals = new string[0];
        }

        public override string ToString()
        {
            return Name;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            GoalList[] goals = { new GoalList("Личный"), new GoalList("Рабочий"), new GoalList("Семейный") };
            while (true)
            {
                Console.Clear();

                int max = goals.Max(g => g.Length);

                Console.WriteLine($"{goals[0]} | {goals[1]} | {goals[2]}");

                for (int i = 0; i < max; i++)
                {
                    Console.Write(String.Join("|", goals.Select(g => g.GetItem(i))));
                    Console.WriteLine();
                }

                Console.WriteLine("Куда вы хотите добавить цель?");
                string userListName = Console.ReadLine(); //то что введёт пользователь переведённое в нижний регистр
                Console.WriteLine("Что это за цель?");
                string userGoal = Console.ReadLine().ToLower();

                goals.FirstOrDefault(g => g.Name.ToLower().Equals(userListName))?
                    .AddItem(userGoal);
            }

        }

    }
}
