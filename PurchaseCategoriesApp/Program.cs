using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PurchaseCategoriesApp
{
    internal class Program
    {
        private static void Main()
        {
            var categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Console.Write("Введите количество покупок: ");
            if (!int.TryParse(Console.ReadLine(), out var n) || n <= 0)
            {
                Console.WriteLine("Некорректное число.");
                return;
            }

            for (var i = 0; i < n; i++)
            {
                Console.Write($"Введите категорию #{i + 1}: ");
                var input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input))
                    categories.Add(input);
            }

            var sortedCategories = categories.OrderBy(c => c).ToList();

            try
            {
                File.WriteAllLines("categories.txt", sortedCategories);

                Console.WriteLine("\nСохранённые категории:");
                foreach (var line in File.ReadAllLines("categories.txt"))
                    Console.WriteLine(line);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка работы с файлом: {ex.Message}");
            }
        }
    }
}