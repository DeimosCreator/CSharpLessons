using System;

namespace DayOfWeekApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ввод
            Console.Write("Введите номер дня (1–7): ");
            int numDay = Convert.ToInt32(Console.ReadLine());

            string strDay;

            // проверка
            switch (numDay)
            {
                case 1: strDay = "Понедельник"; break;
                case 2: strDay = "Вторник"; break;
                case 3: strDay = "Среда"; break;
                case 4: strDay = "Четверг"; break;
                case 5: strDay = "Пятница"; break;
                case 6: strDay = "Суббота"; break;
                case 7: strDay = "Воскресенье"; break;

                default:
                    Console.WriteLine("Неверный номер дня!");
                    return;
            }
            
            // вывод
            Console.WriteLine($"День недели: {strDay}");
        }
    }
}