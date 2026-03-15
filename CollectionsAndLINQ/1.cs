using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsAndLINQ
{
    internal class GradingSystem
    {
        private static void Main()
        {
            var grades = new Dictionary<string, int>();
            var isRunning = true;

            Console.WriteLine("Система учёта оценок (команды: добавить, анализ, выход)");

            while (isRunning)
            {
                Console.Write("\n> ");
                var command = Console.ReadLine();
                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("Ошибка! Введите корректную команду!");
                    continue;
                }

                switch (command.ToLower())
                {
                    case "добавить":
                        Console.Write("\nВведите имя студента: ");
                        var name = Console.ReadLine();
                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("Ошибка! Введите корректное имя студента!");
                            continue;
                        }

                        Console.Write("Введите балл студента: ");
                        if (!int.TryParse(Console.ReadLine(), out var mark))
                        {
                            Console.WriteLine("Ошибка! Введите корректную оценку студента!");
                            continue;
                        }
                        
                        if (grades.ContainsKey(name))
                        {
                            Console.WriteLine("Студент с таким именем уже существует!");
                            continue;
                        }

                        grades.Add(name, mark);

                        break;

                    case "анализ":
                        Console.Write("\nСредний балл: ");
                        Console.Write(grades.Select(value => value.Value).Average());

                        Console.Write("\nЛучший студент: ");
                        Console.Write(grades
                            .Where(student => student.Value == grades.Values.Max())
                            .Select(s => s.Key).First() + $" ({grades.Values.Max()})");

                        Console.Write("\nОтличников (≥80): ");
                        Console.WriteLine(grades.Count(student => student.Value >= 80));

                        break;

                    case "выход":
                        isRunning = false;
                        break;
                    
                    default:
                        Console.WriteLine("Неизвестная команда!");
                        break;
                }
            }
        }
    }
}