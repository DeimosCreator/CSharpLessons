using System;
using System.Xml.Serialization;

public class Vehicle
{
    private int _speed;
    public int Speed
    {
        get => _speed;
        set => _speed = value < 0 ? throw new Exception("Скорость должна быть больше 0") : value;
    }

    public virtual void DisplayInfo() => Console.WriteLine($"Скорость: {Speed} км/ч");
}

public class Car : Vehicle
{
    public string Brand { get; set; }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Автомобиль. {Brand}.");
    }
}

public class Bicycle : Vehicle
{
    public int PedalCadence { get; set; }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Велосипед. Каденс: {PedalCadence} об/мин.");
    }
}

public class ElectricScooter : Vehicle
{
    private int batteryLevel;

    public int BatteryLevel
    {
        get => batteryLevel; set => batteryLevel = value < 0 || value > 100 ? throw new Exception("Заряд батареи должен быть от 0 до 100.") : value;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Самокат. Заряд: {BatteryLevel}%.");
    }
}

public class Program
{
    static void Main()
    {
        Vehicle vehicle = new Car { Brand = "Toyota", Speed = 120 };
        vehicle.DisplayInfo();

        vehicle = new Bicycle { PedalCadence = 90, Speed = 25 };
        vehicle.DisplayInfo();

        vehicle = new ElectricScooter { BatteryLevel = 80, Speed = 30 };
        vehicle.DisplayInfo();
    }
}