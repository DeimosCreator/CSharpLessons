using System;

class Program
{
    public class Config
    {
        public int Value;
    }

    public struct BigData
    {
        public byte[] Payload;
    }

    static void Main()
    {
        var config = new Config { Value = 1 };
        var bigData = new BigData { Payload = new byte[10_000] };

        long memoryBefore = GC.GetTotalMemory(true);
        Console.WriteLine($"Память до операций: {memoryBefore} байт");

        ModifyState(config, bigData);
        Console.WriteLine($"Состояние config после ModifyState: {config.Value}"); // 2

        ReplaceObject(ref config, out bigData);
        Console.WriteLine($"Новый config после ReplaceObject: {config.Value}"); // 100
        Console.WriteLine($"Размер BigData после ReplaceObject: {bigData.Payload.Length}");

        CreateGarbage();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long memoryAfter = GC.GetTotalMemory(false);
        Console.WriteLine($"Память после GC: {memoryAfter} байт");
        Console.WriteLine($"Разница памяти: {memoryAfter - memoryBefore} байт");
    }

    static void ModifyState(Config config, BigData data)
    {
        config.Value += 1;
    }

    static void ReplaceObject(ref Config config, out BigData data)
    {
        config = new Config { Value = 100 };
        data = new BigData { Payload = new byte[20_000_000] }; 
    }

    static void CreateGarbage()
    {
        for (int i = 0; i < 50; i++)
        {
            byte[] garbage = new byte[5_000_000];
        }
    }
}
