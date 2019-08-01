using System;
using System.IO;

namespace Decorator
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
 
    interface ILogWriter
    {
        void WriteError(string message);
    }

    class ConsoleLogWritter : ILogWriter
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogWriter
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureConsoleLogWritter : ILogWriter
    {
        ILogWriter _logWriter;

        public SecureConsoleLogWritter(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logWriter.WriteError(message);
            }
        }
    }
}
