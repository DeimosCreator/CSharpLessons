using System;

public class Program
{
    public static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    public static void Calculate(int x, int y, out int sum, out int product)
    {
        sum = x + y;
        product = x * y;
    }

    static void Main()
    {
        int num1 = 10, num2 = 20;

        Swap(ref num1, ref num2);
        Console.WriteLine($"После Swap: num1={num1}, num2={num2}");

        Calculate(5, 7, out int s, out int p);
        Console.WriteLine($"Сумма: {s}, Произведение: {p}");
    }
}

