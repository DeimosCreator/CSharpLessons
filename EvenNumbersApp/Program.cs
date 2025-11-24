using System;

namespace EvenNumbersApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создайте массив для 6 чисел
            int[] numbers = new int[6];
            // Запросите ввод чисел
            Console.WriteLine("Введите 6 чисел:");
            for (int i = 0; i < 6; i++)
            {
                numbers[i] = Convert.ToInt32(Console.ReadLine());
            }
            // Проверьте и выведите чётные числа
            Console.WriteLine("Чётные числа:");
            for (int i = 0; i < 6; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    Console.WriteLine($"Чётное: {numbers[i]}");
                }
            }
            Console.ReadLine();
        }
    }
}