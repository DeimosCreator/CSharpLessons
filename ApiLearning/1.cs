using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiLearning
{
    internal class Program
    {
        private static async Task Main()
        {
            var apiKey = "dff53d688ea715a2a37e504f821a267c";
            Console.Write("Введите город: ");
            var city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine("Ошибка: город не может быть пустым!");
                return;
            }

            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ru";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                var cityName = root.GetProperty("name").GetString();
                var temp = root.GetProperty("main").GetProperty("temp").GetDouble();
                var description = root.GetProperty("weather")[0].GetProperty("description").GetString();

                Console.WriteLine($"Погода в {cityName}: {temp}°C, {description}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка сети: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка данных: {ex.Message}");
            }
        }
    }
}