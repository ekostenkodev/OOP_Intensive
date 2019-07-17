using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    /*
    Генерация лабиринта

    Необходимо сделать алгоритм генерации алгоритма (выходные данные - двумерный
    массив с типом элементом int, в котором 0 - это дорога, 1 - стена, 2 - точка старта, 3 -
    конечная точка). Конечная точка должна быть достижима из начальной.
    */

    class Program
    {
        static void Main(string[] args)
        {
            #region Пошаговое отображение установки дорог
            //while (true)
            //{
            //    LabyrinthCreator labyrinthCreator = new LabyrinthCreator();
            //    LabyrinthWriter writer = new LabyrinthWriter(labyrinthCreator);

            //    var lab = labyrinthCreator.CreateLabyrinth(20, 0, 1, 2, 3);

            //    writer.ShowLabyrinth(lab);
            //    Console.ReadKey();
            //}
            #endregion

            #region Мгновенный показ результата
            while (true)
            {
                LabyrinthCreator labyrinthCreator = new LabyrinthCreator();
                LabyrinthWriter writer = new LabyrinthWriter();

                var lab = labyrinthCreator.CreateLabyrinth(25, 0, 1, 2, 3);
                writer.ShowLabyrinth(lab);

                Console.ReadKey();
                Console.Clear();
            }
            #endregion
        }
    }


    class Labyrinth
    {
        public int Road { get; set; }
        public int Wall { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public int[,] LabyrinthArray;

        // каким образом можно ходить по лабиринту
        public Tuple<int, int>[] Directions = {Tuple.Create(1, 0),
                                        Tuple.Create(0, 1),
                                        Tuple.Create(-1, 0),
                                        Tuple.Create(0, -1)};

        public Labyrinth(int road, int wall, int start, int end, int[,] labyrinthArray)
        {
            Road = road;
            Wall = wall;
            Start = start;
            End = end;
            LabyrinthArray = labyrinthArray;
        }
    }

    class LabyrinthWriter
    {
        public LabyrinthWriter() { }

        public LabyrinthWriter(LabyrinthCreator labyrinthCreator)
        {
            labyrinthCreator.SetRoad += OnSetRoad;
        }

        public void OnSetRoad(Labyrinth labyrinth)
        {
            // так не делается (захламление консоли + управление основным потоком), да, но ради эффекта почему бы и нет
            ShowLabyrinth(labyrinth);
            System.Threading.Thread.Sleep(20);
            Console.Clear();
        }

        public void ShowLabyrinth(Labyrinth labyrinth)
        {
            for (int i = 0; i < labyrinth.LabyrinthArray.GetLength(0); i++)
            {
                for (int j = 0; j < labyrinth.LabyrinthArray.GetLength(1); j++)
                {
                    if (labyrinth.LabyrinthArray[i, j] == labyrinth.Road)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (labyrinth.LabyrinthArray[i, j] == labyrinth.Wall)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (labyrinth.LabyrinthArray[i, j] == labyrinth.Start)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (labyrinth.LabyrinthArray[i, j] == labyrinth.End)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("|" + labyrinth.LabyrinthArray[i, j]);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
    class Dijkstra
    {
        Random random = new Random();

        class Vertex
        {
            public Tuple<int, int> Coordinates;
            public int Length;

            public Vertex(Tuple<int, int> coordinates, int length)
            {
                Coordinates = coordinates;
                Length = length;
            }
        }
        public Tuple<int, int> GetFarPoint(Labyrinth labyrinth, Tuple<int, int> startPoint)
        {
            Dictionary<Tuple<int, int>, int> vertexPoints = new Dictionary<Tuple<int, int>, int>();
            List<Vertex> vertexOrder = new List<Vertex>();

            vertexOrder.Add(new Vertex(startPoint, 0));

            while (vertexOrder.Count != 0)
            {
                var vertex = vertexOrder[0];

                foreach (var direction in labyrinth.Directions.OrderBy(d => random.Next()))
                {
                    Tuple<int, int> newPoint = null;
                    var x = vertex.Coordinates.Item1 + direction.Item1;
                    var y = vertex.Coordinates.Item2 + direction.Item2;

                    if (x >= labyrinth.LabyrinthArray.GetLength(0) - 1 || x <= 0 ||
                    y >= labyrinth.LabyrinthArray.GetLength(0) - 1 || y <= 0 ||
                    labyrinth.LabyrinthArray[x, y] == labyrinth.Wall)
                        continue;

                    newPoint = Tuple.Create(x, y);
                    int newVertexLength = 0;
                    var newVertex = GetNextVertex(labyrinth, vertexPoints, newPoint, direction, ref newVertexLength);

                    if (newVertex == null)
                        continue;

                    if (vertexOrder.FirstOrDefault(v => v.Coordinates.Equals(newVertex)) != null)
                    {
                        if (vertex.Length + newVertexLength < vertexOrder.First(v => v.Coordinates.Equals(newVertex)).Length)
                            vertexOrder.First(v => v.Coordinates.Equals(newVertex)).Length = vertex.Length + newVertexLength;
                    }
                    else
                    {
                        vertexOrder.Add(new Vertex(newVertex, newVertexLength + vertex.Length));
                    }
                }
                vertexPoints.Add(vertex.Coordinates, vertex.Length);
                vertexOrder.Remove(vertex);

            }
            return vertexPoints.OrderByDescending(v => v.Value).ToArray()[0].Key;
        }
        private Tuple<int, int> GetNextVertex(Labyrinth labyrinth, Dictionary<Tuple<int, int>, int> vertexPoints, Tuple<int, int> lastPoint, Tuple<int, int> lastDirection, ref int length)
        {
            lastDirection = Tuple.Create(lastDirection.Item1 * -1, lastDirection.Item2 * -1);
            length++;

            int roadCount = 0;
            Tuple<int, int> newPoint = null;
            Tuple<int, int> newRoad = null;
            Tuple<int, int> newDirection = null;

            foreach (var direction in labyrinth.Directions.OrderBy(d => random.Next()))
            {
                var x = lastPoint.Item1 + direction.Item1;
                var y = lastPoint.Item2 + direction.Item2;
                newPoint = Tuple.Create(x, y);

                if (lastDirection.Equals(direction))
                {
                    continue;
                }


                if (x > labyrinth.LabyrinthArray.GetLength(0) - 1 || x < 0 ||
                    y > labyrinth.LabyrinthArray.GetLength(0) - 1 || y < 0)
                {
                    continue;
                }

                if (labyrinth.LabyrinthArray[x, y] == labyrinth.Wall)
                {
                    continue;

                }

                roadCount++;
                newRoad = newPoint;
                newDirection = direction;
            }
            if (roadCount == 1)
            {
                return GetNextVertex(labyrinth, vertexPoints, newRoad, newDirection, ref length);
            }
            else
            {
                if (vertexPoints.ContainsKey(lastPoint))
                {
                    return null;
                }
                else
                {
                    return lastPoint;
                }
            }
        }

    }

    class LabyrinthCreator
    {
        Random random = new Random();

        public event Action<Labyrinth> SetRoad;

        public Labyrinth CreateLabyrinth(int size, int road, int wall, int start, int end)
        {
            int[,] labyrinthArray = new int[size, size];
            Labyrinth labyrinth = new Labyrinth(road,wall,start,end,labyrinthArray);
            
            Random random = new Random();
            setDefaultValues(labyrinthArray, labyrinth.Wall);

            // в случайном месте границы массива устанавливается точка старта
            var startPoint = getRandomStartPoint(size);
            labyrinthArray[startPoint.Item1, startPoint.Item2] = labyrinth.Start;

            // установка дороги случайным образом (рандом просто пуляет в разные стороны)
            setRoadInLabyrinth(labyrinth, startPoint);

            // находится самая дальняя точка (по алгоритму Дейкстры), 
            // затем в близжайшей от нее точке в боковой стенке лабиринта устанавливается выход
            setEndInLabyrinth(labyrinth, startPoint);
            return labyrinth;
        }

        private void setDefaultValues(int[,] labyrinth, int defalut)
        {
            for (int i = 0; i < labyrinth.GetLength(0); i++)
                for (int j = 0; j < labyrinth.GetLength(1); j++)
                    labyrinth[i, j] = defalut;
        }

        private Tuple<int, int> getRandomStartPoint(int size)
        {
            Random random = new Random();
            size--;

            // рандом не включает углы лабиринта
            double X = random.Next(1, size);
            double Y = random.Next(1, size);

            if (random.Next(0, 2) == 0)
            {
                X /= size;
                X = Math.Round(X) == 0 ? 0 : size;
            }
            else
            {
                Y /= size;
                Y = Math.Round(Y) == 0 ? 0 : size;
            }

            return Tuple.Create((int)X, (int)Y);
        }

        private void setRoadInLabyrinth(Labyrinth labyrinth, Tuple<int, int> lastPoint)
        {
            // метод вызывается рекурсивно из каждой точки дороги

            // рандомно выбираются направления установки дороги
            foreach (var direction in labyrinth.Directions.OrderBy(d=>random.Next()))
            {
                var newPoint = Tuple.Create(lastPoint.Item1 + direction.Item1, lastPoint.Item2 + direction.Item2);

                // условие не позволяет создавать "комнаты", ге дороги становятся в плотную друг к другу
                if (checkPoint(labyrinth, newPoint.Item1,newPoint.Item2))
                {
                    labyrinth.LabyrinthArray[newPoint.Item1, newPoint.Item2] = labyrinth.Road;
                    SetRoad?.Invoke(labyrinth);

                    setRoadInLabyrinth(labyrinth, newPoint);
                }
                
            }
        }
        private void setEndInLabyrinth(Labyrinth labyrinth, Tuple<int, int> startPoint)
        {
            // ищет самую дальнюю точку по алгоритму Дейкстры, а затем ищет от этой точки ближайшую стену
            Dijkstra dijkstra = new Dijkstra();
            var farPoint = dijkstra.GetFarPoint(labyrinth, startPoint);
            var farPointInWall = findPointInWall(labyrinth, farPoint);

            labyrinth.LabyrinthArray[farPointInWall.Item1, farPointInWall.Item2] = labyrinth.End;
        }
        private Tuple<int, int> findPointInWall(Labyrinth labyrinth, Tuple<int, int> point)
        {
            Tuple<int, int> lastDirection = null;
            Tuple<int, int> newDirection = null;
            Tuple<int, int> nextRoad = null;

            while (true)
            {
                nextRoad = point;
                lastDirection = newDirection;

                foreach (var direction in labyrinth.Directions.OrderBy(d => random.Next()))
                {
                    int x = nextRoad.Item1 + direction.Item1;
                    int y = nextRoad.Item2 + direction.Item2;

                    var newPoint = Tuple.Create(x, y);

                    if (lastDirection == direction)
                    {
                        continue;
                    }

                    if (x == 0 || x == labyrinth.LabyrinthArray.GetLength(0) - 1 ||
                    y == 0 || y == labyrinth.LabyrinthArray.GetLength(1) - 1)
                    {
                        return newPoint;
                    }
                    if(labyrinth.LabyrinthArray[x,y] == labyrinth.Road)
                    {
                        newDirection = Tuple.Create(direction.Item1*-1, direction.Item2*-1);
                        point = newPoint;
                    }
                }


            }
            
        }
            
        private bool checkPoint(Labyrinth labyrinth, int x, int y)
        {
            if (x >= labyrinth.LabyrinthArray.GetLength(0)-1 || x <= 0 ||
                y >= labyrinth.LabyrinthArray.GetLength(0)-1 || y <= 0 ||
                labyrinth.LabyrinthArray[x,y] != labyrinth.Wall)
            {
                return false;
            }
            else
            {
                int size = labyrinth.LabyrinthArray.GetLength(0);
                int roadCount = 0;
                for (int xOffset = -1; xOffset < 2; xOffset++)
                {
                    for (int yOffset = -1; yOffset < 2; yOffset++)
                    {
                        
                        if (xOffset == 0 && yOffset == 0)
                            continue;
                        if (labyrinth.LabyrinthArray[x + xOffset, y + yOffset] == labyrinth.Road)
                            roadCount++;
                    }
                }

                if (roadCount >= 3)
                    return false;
                else
                    return true;
            }
        }
        
    }
}
