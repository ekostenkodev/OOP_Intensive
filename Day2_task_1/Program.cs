using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2_task_1
{
/*
    Декоратор. Защищённые логеры

    Есть некоторый класс который во время работы выводит данные в лог. Необходимо
    спроектировать этот новый класс и перепроектировать систему логирования таким образом,
    чтобы объект этого класса можно было создать в четырёх вариантах:
    1) Защищённый лог в консоль
    2) Лог в консоль
    3) Защищённый лог в файл
    4) Лог в файл

    https://pastebin.com/7xL6S4vV
*/
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
 
    interface ILogWritter
    {
        void WriteError(string message);
    }

    class ConsoleLogWritter : ILogWritter
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogWritter
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureConsoleLogWritter : ILogWritter
    {
        ILogWritter _logWritter;

        public SecureConsoleLogWritter(ILogWritter logWritter)
        {
            _logWritter = logWritter;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logWritter.WriteError(message);
            }
        }
    }
}
