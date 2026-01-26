using System;

namespace StudentSearchApp
{
    internal class Program
    {
        public struct Student
        {
            public string Name;
            public int Grade;
        }

        private static int SearchStudent(in Student[] students, string targetName)
        {
            for (int i = 0; i < students.Length; i++)
            {
                if (targetName == students[i].Name) return i;
            }
            return -1;
        }

        static void Main()
        {
            Student[] students =
            {
                new Student { Name = "Alex", Grade = 5 },
                new Student { Name = "Maria", Grade = 4 },
                new Student { Name = "John", Grade = 3 },
                new Student { Name = "Anna", Grade = 5 }
            };

            Console.Write("Введите имя студента для поиска: ");
            string targetName = Console.ReadLine();

            int index = SearchStudent(in students, targetName);

            if (index != -1)
                Console.WriteLine($"Студент {students[index].Name} найден по индексу: {index}");
            else
                Console.WriteLine("Студент не найден");
        }

    }
}
