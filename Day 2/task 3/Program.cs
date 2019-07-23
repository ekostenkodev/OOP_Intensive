using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace task_3
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

    class Space
    {
        public List<Shape> Shapes = new List<Shape>();

        public float AreaSum(IShapeCalculationVisitor shapeVisitor)
        {
            foreach (var shape in Shapes)
            {
                shape.Accept(shapeVisitor);
            }

            return shapeVisitor.Result;
        }

        public void ShowShapes(IShapeVisitor shapeVisitor) => Shapes.ForEach(shape => shape.Accept(shapeVisitor));
    }

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

        public abstract void Accept(IShapeVisitor visitor);
    }

    class Square : Shape
    {
        public int Size { get; private set; }

        public Square(int x,int y,int size) : base(x,y)
        {
            Size = size;
        }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Rectangle : Shape
    {
        public int Width{ get; private set; }
        public int Height { get; private set; }

        public Rectangle(int x, int y, int width, int height) : base(x,y)
        {
            Width = width;
            Height = height;
        }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Triangle : Shape
    {
        public int Height { get; private set; }
        public int Bottom { get; private set; }

        public Triangle(int x, int y, int height, int bottom) : base(x, y)
        {
            Height = height;
            Bottom = bottom;
        }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Circle : Shape
    {
        public int Radius { get; private set; }

        public Circle(int x, int y, int radius) : base(x, y)
        {
            Radius = radius;
        }

        public override void Accept(IShapeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    #endregion

    #region ShapeVisitor
    interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Square square);
        void Visit(Triangle triangle);
        void Visit(Rectangle rectangle);
    }
    interface IShapeCalculationVisitor : IShapeVisitor
    {
        float Result { get; }
    }

    class ShapeArea : IShapeCalculationVisitor
    {
        public float Result { get; private set; }

        public void Visit(Circle circle) => Result += (float)Math.PI * circle.Radius * circle.Radius;
        public void Visit(Square square) => Result += square.Size * square.Size;
        public void Visit(Triangle triangle) => Result += 0.5f * triangle.Height * triangle.Bottom;
        public void Visit(Rectangle rectangle) => Result += rectangle.Height * rectangle.Width;
    }

    class ShapeConsoleWriter : IShapeVisitor
    {
        public void Visit(Circle circle) => Console.WriteLine($"Круг : ({circle.X}, {circle.Y}), радиус = {circle.Radius}");
        public void Visit(Square square) => Console.WriteLine($"Квадрат : ({square.X}, {square.Y}), сторона = {square.Size}");
        public void Visit(Triangle triangle) => Console.WriteLine($"Треугольник : ({triangle.X}, {triangle.Y}), основание = {triangle.Bottom}, высота = {triangle.Height}");
        public void Visit(Rectangle rectangle) => Console.WriteLine($"Прямоугольник : ({rectangle.X}, {rectangle.Y}), ширина = {rectangle.Width}, высота = {rectangle.Height}");
    }
    class ShapeWinApiWriter : IShapeVisitor
    {
        public void Visit(Circle circle)
        {
            string shapeInfo = $"Круг : ({circle.X}, {circle.Y}), радиус = {circle.Radius}";
            File.WriteAllText("circle.txt", shapeInfo);
        }

        public void Visit(Square square)
        {
            string shapeInfo = $"Квадрат : ({square.X}, {square.Y}), сторона = {square.Size}";
            File.WriteAllText("square.txt", shapeInfo);
        }

        public void Visit(Triangle triangle)
        {
            string shapeInfo = $"Треугольник : ({triangle.X}, {triangle.Y}), основание = {triangle.Bottom}, высота = {triangle.Height}";
            File.WriteAllText("triangle.txt", shapeInfo);
        }

        public void Visit(Rectangle rectangle)
        {
            string shapeInfo = $"Прямоугольник : ({rectangle.X}, {rectangle.Y}), ширина = {rectangle.Width}, высота = {rectangle.Height}";
            File.WriteAllText("rectangle.txt", shapeInfo);
        }
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Space space = new Space();

            space.Shapes.Add(new Circle(1, 2, 5));
            space.Shapes.Add(new Square(5, 7, 3));
            space.Shapes.Add(new Rectangle(0, 0, 5, 1));
            space.Shapes.Add(new Triangle(10, 10, 10, 10));

            space.ShowShapes(new ShapeConsoleWriter());

            float area = space.AreaSum(new ShapeArea());

            Console.WriteLine($"Сумма всех площадей : {area}");
        }
    }
}
