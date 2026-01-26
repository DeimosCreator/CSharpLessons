using System;

namespace ArraySearch
{
    public class Program
    {
        static int FindMax(int[] array)
        {
            int result = array[0];
            for (int i = 1; i < array.Length; i++)
                if (result < array[i]) result = array[i];
            return result;
        }

        static int FindPositiveCount(int[] array)
        {
            int result = 0;
            foreach (var item in array)
                if (item > 0) result++;
            return result;
        }

        static int FindNum(int[] array, int target)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == target)
                    return i;
            return -1;
        }

        static void Main()
        {
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Ошибка: введите целое число!");
                return;
            }

            if (n > 10 || n <= 0)
            {
                Console.WriteLine("Ошибка: размер массива должен быть от 1 до 10!");
                return;
            }

            int[] array = new int[n];
            for (int i = 0; i < n; i++)
            {
                if (!int.TryParse(Console.ReadLine(), out array[i]))
                {
                    Console.WriteLine("Ошибка: введите целое число!");
                    return;
                }
            }

            Console.WriteLine($"Максимальный элемент: {FindMax(array)}");
            Console.WriteLine($"Количество положительных чисел: {FindPositiveCount(array)}");

            if (!int.TryParse(Console.ReadLine(), out int target))
            {
                Console.WriteLine("Ошибка: введите целое число!");
                return;
            }

            int index = FindNum(array, target);

            if (index == -1)
                Console.WriteLine($"Число {target} не найдено в массиве");
            else
                Console.WriteLine($"Число {target} найдено по индексу {index}");
        }
    }
}
