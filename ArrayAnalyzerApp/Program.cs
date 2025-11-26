using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Ввод данных
        int size = 0;
        Console.Write("Введите размер массива (не менее 5): ");
        string tempSize = Console.ReadLine();
        if (!int.TryParse(tempSize, out size) || size < 5)
        {
            Console.WriteLine("Вы ввели некорректные данные");
            return;
        }

        int[] A = new int[size];
        Console.WriteLine($"Введите {size} целых чисел:");
        for (int i = 0; i < size; i++)
        {
            if (!int.TryParse(Console.ReadLine(), out A[i]))
            {
                Console.WriteLine("Вы ввели некорректные данные");
                return;
            }
        }

        int[] originalA = (int[])A.Clone();

        // Анализ
        int summ = 0;
        int min = A[0];
        int max = A[0];
        int evenCount = 0;
        int zeroIndex = -1;

        for (int i = 0; i < size; i++)
        {
            summ += A[i];
            if (A[i] < min) min = A[i];
            if (A[i] > max) max = A[i];
            if (A[i] % 2 == 0) evenCount++;
            if (A[i] == 0 && zeroIndex == -1) zeroIndex = i;
        }

        // Сортировка
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size - 1 - i; j++)
            {
                if (A[j] > A[j + 1])
                {
                    int temp = A[j];
                    A[j] = A[j + 1];
                    A[j + 1] = temp;
                }
            }
        }

        // Вывод
        Console.Write("\nИсходный массив: [");
        for (int i = 0; i < size; i++)
        {
            Console.Write(originalA[i]);
            Console.Write(i == size - 1 ? "]\n" : ", ");
        }

        Console.Write("Отсортированный массив: [");
        for (int i = 0; i < size; i++)
        {
            Console.Write(A[i]);
            Console.Write(i == size - 1 ? "]\n" : ", ");
        }

        Console.WriteLine("\nРезультаты анализа:");
        Console.WriteLine($"Сумма элементов: {summ}");
        Console.WriteLine($"Минимальный элемент: {min}");
        Console.WriteLine($"Максимальный элемент: {max}");
        Console.WriteLine($"Количество чётных чисел: {evenCount}");
        Console.WriteLine($"Индекс первого нуля: {zeroIndex}");
    }
}
