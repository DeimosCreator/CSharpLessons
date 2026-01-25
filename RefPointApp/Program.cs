using System;

namespace RefPointApp
{
    struct Point
    {
        public int x, y;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Point p = new Point { x = 1, y = 2 };

            Console.WriteLine($"Исходные координаты: x = {p.x}, y = {p.y}");

            MoveByValue(p, 5, 5);
            Console.WriteLine($"После MoveByValue: x = {p.x}, y = {p.y}"); // не изменится

            MoveByRef(ref p, 5, 5);
            Console.WriteLine($"После MoveByRef: x = {p.x}, y = {p.y}");   // изменится
        }

        static void MoveByValue(Point p, int dx, int dy)
        {
            p.x += dx;
            p.y += dy;
        }

        static void MoveByRef(ref Point p, int dx, int dy)
        {
            p.x += dx;
            p.y += dy;
        }
    }
}