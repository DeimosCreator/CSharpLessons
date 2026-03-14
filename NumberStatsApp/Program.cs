using System;
using System.IO;

namespace NumberStatsApp
{
    internal class Program
    {
        private static string NumPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "numbers.txt");
        private static string SumPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sum.bin");

        public static void Main()
        {
            int count;
            var sum = 0;
            Console.WriteLine("Введите кол-во чисел: ");
            try
            {
                count = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Введите корректное кол-во чисел");
                return;
            }

            Console.WriteLine($"Введите числа: ");
            try
            {
                using (var streamWriter = new StreamWriter(NumPath))
                {
                    for (var i = 0; i < count; i++)
                    {
                        var t = Convert.ToInt32(Console.ReadLine());
                        streamWriter.WriteLine(t);
                        sum += t;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {NumPath} не найден");
                return;
            }
            catch (IOException)
            {
                Console.WriteLine($"Нет доступа к файлу {NumPath}");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Введите корректное число");
                throw;
            }

            try
            {
                using (var binaryWriter = new BinaryWriter(File.Open(SumPath, FileMode.Create)))
                {
                    binaryWriter.Write(sum);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {SumPath} не найден");
                return;
            }
            catch (IOException)
            {
                Console.WriteLine($"Нет доступа к файлу {SumPath}");
                return;
            }

            try
            {
                Console.WriteLine("Числа из файла numbers.txt:");
                using (var streamReader = new StreamReader(NumPath))
                {
                    Console.WriteLine(streamReader.ReadToEnd().TrimEnd());
                }

                Console.WriteLine("\nСумма из файла sum.bin:");
                using (var binaryReader = new BinaryReader(File.Open(SumPath, FileMode.Open)))
                {
                    var readSum = binaryReader.ReadInt32();
                    Console.WriteLine(readSum);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка при чтении: Файл не найден. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода при чтении. {ex.Message}");
            }
        }
    }
}