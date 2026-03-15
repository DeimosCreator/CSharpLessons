using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsAndLINQ
{
    class OrderSystem
    {
        static void Main()
        {
            var random = new Random();
            HashSet<string> uniqueIds = new HashSet<string>();
            Dictionary<string, int> orders = new Dictionary<string, int>();

            Console.WriteLine("Система учёта заказов (формат: [ID]:[Количество])");
            Console.WriteLine("Команды: отчет, сброс, выход");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input == "отчет")
                {
                    Console.Write("\nУникальных товаров: ");
                    Console.Write(uniqueIds.Count);
                    
                    Console.Write("\nТоп-3 товаров:\n");
                    Console.Write(string.Join("\n", orders
                        .OrderByDescending(n => n.Value)
                        .Take(3)
                        .Select(n => $"{n.Key}: {n.Value} шт."))); 
                    
                    Console.Write("\nСреднее количество: ");
                    Console.Write((double)orders.Values.Sum() / uniqueIds.Count);
                    Console.WriteLine();
                }
                else if (input == "сброс")
                {
                    uniqueIds.Clear();
                    orders.Clear();
                    Console.WriteLine("Данные сброшены!");
                }
                else if (input == "выход") break;
                else
                {
                    var data = input?.Split(':');
                    if (data != null && data.Length == 2)
                    {
                        var id = data[0].Trim();
                        if (!string.IsNullOrEmpty(id) && int.TryParse(data[1], out var count) && count > 0)
                        {
                            uniqueIds.Add(id);

                            if (orders.ContainsKey(id))
                                orders[id] += count;
                            else
                                orders[id] = count;
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ввод. Используйте формат ID:Количество (например, A101:3)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод. Используйте формат ID:Количество");
                    }
                }
            }
        }
    }
}