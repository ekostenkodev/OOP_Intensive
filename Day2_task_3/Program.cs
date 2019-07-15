using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2_task_3
{
    /*
        Фигуры. Проектирование

        Мы разрабатываем систему моделирования геометрическими фигурами которая
        абстрагирует нас от какого-то низкоуровневого векторного API.
        Первый термин - некое пространство над которым выполняются операции.
        Пространство может состоять из следующих фигур:
        - Квадрат
        - Прямоугольник
        - Треугольник
        - Круг

        Каждая фигура описывается двумя координатами указывающими на центр фигуры в
        пространстве. Квадрат имеет ширину и высоту одинаковую и описывается условным
        свойством Size, прямоугольник описывается шириной и высотой, круг радиусом и
        треугольник сходным образом.
        А также над пространство можно совершать следующие операции:
        - Посчитать сумму площадей фигур
        - Отобразить фигуры с помощью WinApi формы
        - Отобразить фигуры с помощью Консоли
    */

    #region Shapes

    abstract class Shape
    {
        public int X { get; private set; }
        public int Y { get; private set; }


        public Shape(int x, int y)
        {
            X = x;
            Y = y;
        }

        public abstract float Area { get; }
    }

    class Square : Shape
    {
        int _size;

        public Square(int x,int y,int size) : base(x,y)
        {
            _size = size;
        }

        public override float Area => _size * _size;
    }

    class Rectangle : Shape
    {
        int _width, _height;

        public Rectangle(int x, int y, int width, int height) : base(x,y)
        {
            _width = width;
            _height = height;
        }

        public override float Area => _width * _height;
    }

    class Triangle : Shape
    {
        int _height, _bottom;

        public Triangle(int x, int y, int height, int bottom) : base(x, y)
        {
            _height = height;
            _bottom = bottom;
        }

        public override float Area => 0.5f * _height * _bottom;
    }

    class Circle : Shape
    {
        int _radius;

        public Circle(int x, int y, int radius) : base(x, y)
        {
            _radius = radius;
        }

        public override float Area => (float)Math.PI * _radius * _radius;
    }

    #endregion

    #region ShapeWriter
    interface IShapeWriter
    {
        void Write(Shape shape);
    }
    class ShapeConsoleWriter : IShapeWriter
    {
        public void Write(Shape shape)
        {
            Console.WriteLine($"Координаты фигуры : ({shape.X}, {shape.Y})");
            Console.WriteLine($"Площадь фигуры : {shape.Area}");
        }
    }
    class ShapeWinApiWriter : IShapeWriter
    {
        public void Write(Shape shape)
        {
            string shapeInfo = $"Координаты фигуры : ({shape.X}, {shape.Y})" + "/n"
                                + $"Площадь фигуры : {shape.Area}";
            File.WriteAllText("shape.txt", shapeInfo);
        }
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Shape shape = new Circle(1, 2, 5);

            IShapeWriter shapeWriter = new ShapeConsoleWriter();

            shapeWriter.Write(shape);
        }
    }
}
