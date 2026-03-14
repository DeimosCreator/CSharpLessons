using System;
using System.IO;

namespace FileStreamPractice
{
    internal class Program
    {
        public static void Main()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "diary.txt");

            try
            {
                if (!File.Exists(path)) File.Create(path).Close();

                while (true)
                {
                    Console.WriteLine("\nВыберите действие:");
                    Console.WriteLine("1 - Прочитать записи");
                    Console.WriteLine("2 - Добавить запись");
                    Console.WriteLine("3 - Очистить дневник");
                    Console.WriteLine("4 - Выход");

                    if (!int.TryParse(Console.ReadLine(), out var action))
                    {
                        Console.WriteLine("Введите число!");
                        continue;
                    }

                    switch (action)
                    {
                        case 1:
                            using (var reader = new StreamReader(path))
                            {
                                var content = reader.ReadToEnd();

                                if (string.IsNullOrWhiteSpace(content))
                                    Console.WriteLine("Дневник пуст.");
                                else
                                    Console.WriteLine(content);
                            }

                            break;

                        case 2:
                            Console.WriteLine("Введите новую запись:");

                            var text = Console.ReadLine();

                            using (var writer = new StreamWriter(path, true))
                            {
                                writer.WriteLine($"[{DateTime.Now}] {text}");
                            }

                            Console.WriteLine("Запись добавлена.");
                            break;

                        case 3:
                            Console.WriteLine("Вы уверены? (y/n)");

                            var confirm = Console.ReadLine();

                            if (confirm.ToLower() == "y")
                            {
                                using (var writer = new StreamWriter(path))
                                {
                                    writer.Write("");
                                }

                                Console.WriteLine("Дневник очищен.");
                            }

                            break;

                        case 4:
                            return;

                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка работы с файлом: {ex.Message}");
            }
        }
    }
}