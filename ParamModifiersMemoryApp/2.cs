using System;
using System.Diagnostics;

public class Program
{
    public struct SensorData
    {
        public int SensorId;
        public string SensorName;
        public double Temperature;
        public double Humidity;
        public double Pressure;
        public double Altitude;
        public double Latitude;
        public double Longitude;
        public long Timestamp;
        public bool IsActive;
        public string Location;
        public double BatteryLevel;
    }

    static void Main()
    {
        var data = new SensorData
        {
            SensorId = 1,
            SensorName = "TempSensor",
            Temperature = 23.5,
            Humidity = 55.2,
            Pressure = 1013.1,
            Altitude = 250.0,
            Latitude = 55.7558,
            Longitude = 37.6173,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsActive = true,
            Location = "Moscow",
            BatteryLevel = 98.5
        };

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 1_000_000; i++)
        {
            ProcessData(data);
        }
        sw.Stop();
        Console.WriteLine($"Время с копированием: {sw.ElapsedMilliseconds} мс");

        sw.Restart();
        for (int i = 0; i < 1_000_000; i++)
        {
            ProcessDataOptimized(in data);
        }
        sw.Stop();
        Console.WriteLine($"Время с 'in': {sw.ElapsedMilliseconds} мс");
    }

    private static double ProcessData(SensorData data)
    {
        return data.Temperature + data.Humidity + data.Pressure;
    }

    private static double ProcessDataOptimized(in SensorData data)
    {
        //data.Temperature = 0;

        return data.Temperature + data.Humidity + data.Pressure;
    }
}

