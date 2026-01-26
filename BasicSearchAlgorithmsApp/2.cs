using System;
using System.Collections.Generic;

namespace BasicSearchAlgorithmsApp
{
    public class Program
    {
        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year { get; set; }
        }

        static int SimpleSubstringSearch(string text, string pattern)
        {
            for (int i = 0; i <= text.Length - pattern.Length; i++)
            {
                int j;
                for (j = 0; j < pattern.Length; j++)
                    if (text[i + j] != pattern[j]) break;

                if (j == pattern.Length) return i;
            }
            return -1;
        }

        static List<Book> FindByAuthor(Book[] books, string author)
        {
            List<Book> result = new List<Book>();

            foreach (var book in books)
            {
                if (book.Author.ToLower().Contains(author.ToLower()))
                    result.Add(book);
            }

            return result;
        }

        static int FindFirstByYear(Book[] books, int year)
        {
            int left = 0;
            int right = books.Length - 1;
            int result = -1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (books[mid].Year == year)
                {
                    result = mid;
                    right = mid - 1;
                }
                else if (books[mid].Year < year)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return result;
        }

        static void Main()
        {
            var books = new Book[]
            {
                new Book { Id=1, Title="C# Basics", Author="John Smith", Year=2020 },
                new Book { Id=2, Title="Advanced C#", Author="Sarah Connor", Year=2023 },
                new Book { Id=3, Title="Algorithms", Author="John Doe", Year=2019 }
            };

            var johnsBooks = FindByAuthor(books, "John");

            Console.WriteLine("Книги Джона:");
            foreach (var book in johnsBooks)
                Console.WriteLine($"  - {book.Title} ({book.Author})");

            Array.Sort(books, (a, b) => a.Year.CompareTo(b.Year));
            int index2023 = FindFirstByYear(books, 2023);

            if (index2023 != -1)
                Console.WriteLine($"Первая книга 2023 года: {books[index2023].Title}");
            else
                Console.WriteLine("Книга 2023 года не найдена");

            int titlePos = SimpleSubstringSearch("Advanced C#", "Advanced");
            Console.WriteLine($"Подстрока 'Advanced' найдена на позиции: {titlePos}");
        }
    }
}
