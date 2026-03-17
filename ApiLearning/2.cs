using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiLearning;

internal class User
{
    public string username { get; set; }
    public string email { get; set; }
    public Address address { get; set; }
}

internal class Address
{
    public string city { get; set; }
}

internal class Program
{
    private static async Task Main()
    {
        using var client = new HttpClient();
        var url = "https://jsonplaceholder.typicode.com/users";

        try
        {
            Console.Write("Введите имя пользователя: ");
            var username = Console.ReadLine();

            Console.Write("Введите почту: ");
            var email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new FormatException("Неверный формат email");

            Console.Write("Введите город проживания: ");
            var city = Console.ReadLine();

            var user = new User
            {
                username = username,
                email = email,
                address = new Address
                {
                    city = city
                }
            };

            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);

            var root = doc.RootElement;

            var id = root.GetProperty("id").GetInt32();

            Console.WriteLine($"ID пользователя: {id}");

            var address = root.GetProperty("address");
            var c = address.GetProperty("city").GetString();
            var street = address.TryGetProperty("street", out var st) ? st.GetString() : "неизвестна";
            var suite = address.TryGetProperty("suite", out var su) ? su.GetString() : "неизвестна";
            Console.WriteLine($"Адрес: {c}, {street}, {suite}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка запроса: {e.Message}");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Ошибка JSON: {e.Message}");
        }
        catch (FormatException e)
        {
            Console.WriteLine($"Ошибка формата: {e.Message}");
        }
    }
}