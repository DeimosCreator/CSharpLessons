using System;
using System.IO;

namespace SpaceArchive
{
    internal class Program
    {
        public static void Main()
        {
            const string pathMissions = "missions.txt";
            const string pathPoints = "mission_stats.bin";

            var missionNames = new string[5];
            var missionPoints = new int[5];
            var totalPoints = 0;

            Console.WriteLine("Введите данные 5 миссий:");

            for (var i = 0; i < 5; i++)
            {
                Console.Write($"Введите название миссии {i + 1}: ");
                missionNames[i] = Console.ReadLine();

                if (string.IsNullOrEmpty(missionNames[i]))
                    missionNames[i] = $"Миссия {i + 1}";

                Console.Write($"Введите очки за миссию {i + 1}: ");

                if (!int.TryParse(Console.ReadLine(), out missionPoints[i]) || missionPoints[i] < 0)
                {
                    Console.WriteLine("Очки должны быть неотрицательным числом!");
                    return;
                }

                totalPoints += missionPoints[i];
            }

            var averagePoints = totalPoints / 5.0;

            using (var writer = new StreamWriter(pathMissions))
            {
                for (var i = 0; i < 5; i++) writer.WriteLine($"{missionNames[i]}: {missionPoints[i]}");
            }

            using (var writer = new BinaryWriter(File.Open(pathPoints, FileMode.Create)))
            {
                writer.Write(totalPoints);
                writer.Write(averagePoints);
            }

            Console.WriteLine("\nДанные сохранены.\n");

            try
            {
                Console.WriteLine("Архив миссий:");

                using (var reader = new StreamReader(pathMissions))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null) Console.WriteLine($"Миссия {line}");
                }

                using (var reader = new BinaryReader(File.Open(pathPoints, FileMode.Open)))
                {
                    var total = reader.ReadInt32();
                    var average = reader.ReadDouble();

                    Console.WriteLine($"\nАрхив миссий загружен!");
                    Console.WriteLine($"Всего очков: {total}");
                    Console.WriteLine($"Среднее: {average:F2}");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден!");
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка чтения файла!");
            }

            Console.ReadLine();
        }
    }
}