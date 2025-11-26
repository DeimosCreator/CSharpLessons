using System;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите размер массива: ");
        int size;
        if (!int.TryParse(Console.ReadLine(), out size) || size <= 0)
        {
            Console.WriteLine("Введите положительное число!");
            return;
        }

        int[] A = new int[size];
        int evenCount = 0;

        Console.WriteLine("Введите положительные числа массива:\n");
        for (int i = 0; i < size; i++)
        {
            if (!int.TryParse(Console.ReadLine(), out A[i]) || A[i] < 0)
            {
                Console.WriteLine("Введите положительное число!");
                return;
            }
            if (A[i] % 2 == 0) evenCount++;
        }

        Console.WriteLine($"В массиве {evenCount} чётных чисел.");
    }
}