using System;
using System.Diagnostics;

namespace BasicSearchAlgorithmsApp
{
    class Program
    {
        static int LinearSearch(int[] array, int target)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == target)
                    return i;
            }
            return -1;
        }

        static int BinarySearch(int[] array, int left, int right, int target)
        {
            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (array[mid] == target)
                    return mid;
                if (array[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            return -1;
        }

        static void CompareSearches()
        {
            int[] array = new int[10000];
            Random rnd = new Random();

            for (int i = 0; i < array.Length; i++)
                array[i] = rnd.Next(0, 100000);

            int target = array[array.Length - 1];

            Stopwatch sw = new Stopwatch();

            sw.Start();
            int linearIndex = LinearSearch(array, target);
            sw.Stop();
            Console.WriteLine($"Линейный поиск: {sw.ElapsedMilliseconds} мс");

            sw.Restart();
            Array.Sort(array);
            int binaryIndex = BinarySearch(array, 0, array.Length - 1, target);
            sw.Stop();
            Console.WriteLine($"Бинарный поиск: {sw.ElapsedMilliseconds} мс (включая сортировку)");

            int notFound = LinearSearch(array, -1);
            Console.WriteLine(notFound == -1
                ? "Элемент не найден (возвращено -1)"
                : "Ошибка обработки отсутствующего элемента");
        }

        static void Main()
        {
            CompareSearches();
        }
    }
}
