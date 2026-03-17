using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiLearning;

internal class Repository(int starCount, string language)
{
    public int StarCount = starCount;
    public string Language = language;
}

internal class Program
{
    private static async Task Main()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "MyCSharpApp");
        client.Timeout = TimeSpan.FromSeconds(15);

        var repositories = new List<Repository>();

        Console.Write("Введите запрос: ");
        var query = Console.ReadLine();

        const int perPage = 30;
        
        var fileName = $"github_{query}.json";

        var isHaveFile = File.Exists(fileName);

        try
        {
            if (isHaveFile)
            {
                var json = await ReadFile(fileName);
                var (repos, total) = ParseRepositories(json);

                PrintStats(repos, total);
                return;
            }
            var totalCount = 0;

            for (int i = 0; i < 100 / perPage; i++)
            {
                var url = $"https://api.github.com/search/repositories?q={query}&page={i + 1}&per_page={perPage}";

                var response = await HttpRequest(client, url);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new ApiRateLimitExceededException("Превышен лимит запросов к API");

                var json = await response.Content.ReadAsStringAsync();

                var (repos, total) = ParseRepositories(json);

                totalCount = total;
                repositories.AddRange(repos);

                Console.WriteLine($"Загрузка: {(i + 1) * 25}%");

                if (i == 0)
                    await WriteFile(fileName, json);
            }
            
            if (totalCount == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }
            
            var starCount = (int)repositories.Select(repo => repo.StarCount).Average();
            var language = repositories.Select(repo => repo.Language)
                .GroupBy(lang => lang)
                .OrderByDescending(lang => lang.Count())
                .Select(lang => lang.Key)
                .FirstOrDefault();
            
            Console.WriteLine(
                $"Среднее кол-во звёзд: {starCount}, самый популярный язык: {language}, кол-во найденных репозиториев: {totalCount}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка запроса: {e.Message}");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Ошибка JSON: {e.Message}");
        }
        catch (CacheNotFoundException e)
        {
            Console.WriteLine(e);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Таймаут запроса");
        }
    }

    private static async Task<HttpResponseMessage> HttpRequest(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static Task<string> ReadFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new CacheNotFoundException("Файл кэша не найден", fileName);
        }
        return Task.FromResult(File.ReadAllText(fileName));
    }

    private static Task WriteFile(string fileName, string content)
    {
        File.WriteAllText(fileName, content);
        return Task.CompletedTask;
    }
    
    private static JsonDocument ReadJson(string jsonResponse)
    {
        return JsonDocument.Parse(jsonResponse);
    }
    
    private static (List<Repository>, int) ParseRepositories(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var totalCount = root.GetProperty("total_count").GetInt32();

        var repos = root.GetProperty("items")
            .EnumerateArray()
            .Select(repo => new Repository(
                repo.GetProperty("stargazers_count").GetInt32(),
                repo.GetProperty("language").GetString() ?? "Unknown"
            ))
            .ToList();

        return (repos, totalCount);
    }
    
    private static void PrintStats(List<Repository> repositories, int totalCount)
    {
        if (totalCount == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }

        var avgStars = (int)repositories.Average(r => r.StarCount);

        var topLanguage = repositories
            .GroupBy(r => r.Language)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

        Console.WriteLine(
            $"Среднее кол-во звёзд: {avgStars}, самый популярный язык: {topLanguage}, кол-во найденных репозиториев: {totalCount}");
    }
}

internal class CacheNotFoundException : Exception
{
    public string FileName { get; }

    public CacheNotFoundException(string message, string fileName)
        : base($"{message} (Файл: {fileName})")
    {
        FileName = fileName;
    }
}
public class ApiRateLimitExceededException : Exception
{
    public ApiRateLimitExceededException(string message) : base(message) { }
}
