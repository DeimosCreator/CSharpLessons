using System;

public class Book
{
    private string title;
    private string author;

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public string Title
    {
        get => title;
        set => title = string.IsNullOrWhiteSpace(value)
            ? throw new Exception("Title не может быть пустым")
            : value;
    }

    public string Author
    {
        get => author;
        set => author = string.IsNullOrWhiteSpace(value)
            ? throw new Exception("Автор не может быть пустым")
            : value;
    }


    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Название: {Title}, автор {Author}");
    }
}

public class FictionBook : Book
{
    private protected string genre;


    public string Genre
    {
        get => genre;
        set => genre = string.IsNullOrWhiteSpace(value)
            ? throw new Exception("Жанр не может быть пустым")
            : value;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Жанр: {Genre}");
    }
    public FictionBook(string title, string author, string _genre) : base(title, author)
    {
        Genre = _genre;
    }
}

public class NonFictionBook : Book
{
    private protected string topic;

    public string Topic
    {
        get => topic;
        set => topic = string.IsNullOrWhiteSpace(value)
            ? throw new Exception("Тема не может быть пустой")
            : value;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Тема: {Topic}");
    }
    public NonFictionBook(string title, string author, string _topic) : base(title, author)
    {
        Topic = _topic;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Позитивный тест ===");
        try
        {
            Book[] books = {
                new FictionBook("Harry Potter", "J.K. Rowling", "Fantasy"),
                new NonFictionBook("Sapiens", "Yuval Noah Harari", "History")
            };

            foreach (var book in books)
            {
                book.DisplayInfo();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Негативный тест (пустое название) ===");
        try
        {
            var invalidBook = new FictionBook("", "Some Author", "Mystery");
            invalidBook.DisplayInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Граничный тест (пустой автор) ===");
        try
        {
            var edgeBook = new NonFictionBook("Edge Case Book", "", "Science");
            edgeBook.DisplayInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
