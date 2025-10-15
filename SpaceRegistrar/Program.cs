using System;

namespace SpaceRegistrar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Объявление массивов и переменной
            string[] names = new string[5];
            int[] ages = new int[5];
            int suitableAstronauts = 0;

            //Ввод имён и возраста
            Console.WriteLine("Введите данные 5 космонавтов:");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Введите имя космонавта {i + 1}: ");
                names[i] = Console.ReadLine();
                Console.Write($"Введите возраст космонавта {i + 1}: ");
                ages[i] = Convert.ToInt32(Console.ReadLine());
            }

            //Проверка и вывод
            Console.WriteLine();
            Console.WriteLine("Результаты проверки:");
            for (int i = 0; i < 5; i++)
            {
                if (ages[i] >= 18 && ages[i] <= 60)
                {
                    Console.WriteLine($"Космонавт {names[i]} готов к полёту!");
                    suitableAstronauts++;
                }
            }
            Console.WriteLine($"Миссия готова! Найдено {suitableAstronauts} космонавтов для полёта.");

        }
    }
}
