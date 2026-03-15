using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsAndLINQ
{
    class Book
    {
        public string Title { get; }
        public string Author { get; }
        public int Year { get; }

        public Book(string title, string author, int year)
        {
            if (year < 1000 || year > DateTime.Now.Year)
                throw new ArgumentException("Некорректный год");

            Title = title;
            Author = author;
            Year = year;
        }
    }
    
    public class BookProgram
    {
        private static void Main()
        {
            var books = new Dictionary<string, List<Book>>();
            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Некорректный ввод!");
                    continue;
                }

                if (input.StartsWith("добавить"))
                {
                    try
                    {
                        input = input.Substring("добавить ".Length);
                        var data = input.Split(':');
                        if (!books.ContainsKey(data[0]))
                        {
                            books.Add(data[0], new List<Book>());
                        }

                        if (books[data[0]].Any(b => b.Title == data[1] && b.Author == data[2]))
                        {
                            Console.WriteLine("Такая книга уже существует!");
                        }
                        else
                        {
                            books[data[0]].Add(new Book(data[1], data[2], Convert.ToInt32(data[3])));
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Ошибка формата данных");
                    }
                }
                else if (input.StartsWith("поиск"))
                {
                    input = input.Substring("поиск ".Length);
                    var result = "";
                    var data = input.Split(':');
                    try
                    {
                        switch (data[0])
                        {
                            case "автор":
                                result = string.Join("\n", books
                                    .SelectMany(category => category.Value
                                        .Where(book => book.Author == data[1])
                                        .Select(book => $"[{category.Key}] \"{book.Title}\" ({book.Author}, {book.Year})")));
                                break;
                            case "категория":
                                result = string.Join("\n", books[data[1]]
                                    .Select(info => $"[{data[1]}] \"{info.Title}\" ({info.Author}, {info.Year})"));
                                break;
                            case "год":
                                IEnumerable<Book> filtered;

                                if (data[1].StartsWith(">"))
                                {
                                    int year = int.Parse(data[1].Substring(1));
                                    filtered = books.SelectMany(c => c.Value).Where(b => b.Year > year);
                                }
                                else if (data[1].StartsWith("<"))
                                {
                                    int year = int.Parse(data[1].Substring(1));
                                    filtered = books.SelectMany(c => c.Value).Where(b => b.Year < year);
                                }
                                else
                                {
                                    int year = int.Parse(data[1]);
                                    filtered = books.SelectMany(c => c.Value).Where(b => b.Year == year);
                                }

                                result = string.Join("\n", books
                                    .SelectMany(category => category.Value
                                        .Where(book => filtered.Contains(book))
                                        .Select(book => $"[{category.Key}] \"{book.Title}\" ({book.Author}, {book.Year})")));
                                break;
                            default:
                                Console.WriteLine("Неизвестный фильтр!");
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Книга с {data[0]} '{data[1]}' не найдена");
                    }

                    Console.WriteLine(string.IsNullOrEmpty(result)
                        ? $"Книга с {data[0]} '{data[1]}' не найдена"
                        : result);
                }
                else if (input.StartsWith("статистика"))
                {
                    Console.WriteLine("Категории:");
                    Console.Write(string.Join("\n", books.Select(book => $"\t{book.Key}: {book.Value.Count} книг")));
                    
                    Console.Write("\nСамая старая книга: ");
                    Console.Write(books.Values
                        .SelectMany(list => list)
                        .OrderBy(b => b.Year)
                        .Select(b => $"'{b.Title}' ({b.Year})")
                        .FirstOrDefault());
                    
                    Console.Write("\nСамый продуктивный автор: ");
                    Console.Write(books.Values
                        .SelectMany(list => list)
                        .GroupBy(b => b.Author)
                        .OrderByDescending(b => b.Count())
                        .Select(b => $"{b.Key} ({b.Count()} книг)")
                        .FirstOrDefault());
                    
                    Console.Write("\nСредний год издания: ");
                    Console.WriteLine(string.Join("\n",
                        books.Select(category =>
                            $"\t{category.Key}: {category.Value.Average(b => b.Year)}"
                        )
                    ));
                }
                else if (input == "выход") break;
            }
        }
    }
}