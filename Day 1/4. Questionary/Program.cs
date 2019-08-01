using System;

namespace Questionary
{
    /*  
        Анкетка

        https://pastebin.com/hSe1b92a

        Воспользуйтесь классами и выделите типичную структуру данных. Воспользуйте
        массивом объектов этого класса, для того, чтобы сделать цикл и избавиться от
        повторяющегося кода.
    */

    class Program
    {
        class Door
        {
            public bool OpenStatus { get; set; } = false;
            public string Question { get; private set; }
            public string[] Answers { get; private set; }

            public Door(string question, params string[] answers)
            {
                Question = question;
                Answers = answers;
            }
        }

        static void Main(string[] args)
        {
            Door[] doors = {
                new Door("Кто вы?","Человек", "Брандлмуха", "Кхаджит"),
                new Door("Что вы хотите?","Победить Аразота", "Стать богатым", "Найти боевых товарищей"),
                new Door("Чем вы можете помочь ордену?","Я отлчиный воин", "Я добротный маг", "Я могу работать в кузнице")
            };

            Console.WriteLine("Совершенно очевидно, что мы не берём в наш орден кого попало. По этому заполни вот эту анкету, " +
                              "и мы примем решение, брать тебя или нет");

            foreach (var door in doors)
            {
                Console.WriteLine(door.Question);
                for (int i = 0; i < door.Answers.Length; i++)
                {
                    Console.WriteLine($"[{i}]>{door.Answers[i]}");
                }
                Console.ReadLine();
                door.OpenStatus = true;
            }

        }
    }
}
