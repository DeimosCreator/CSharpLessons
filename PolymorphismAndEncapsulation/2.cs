using System;
using System.Diagnostics;
using System.Text;

public interface IPaymentMethod
{
    bool ProcessPayment(double amount);
}

public class CreditCard : IPaymentMethod
{
    private string _cardNumber;
    private int _cvv;

    public CreditCard(string cardNumber, int cVV)
    {
        if (cVV < 100 || cVV > 999)
            throw new Exception("CVV должен быть из 3 цифр");

        CardNumber = cardNumber;
        CVV = cVV;
    }

    public string CardNumber { get => _cardNumber; set => _cardNumber = value; }
    public int CVV { get => _cvv; set => _cvv = value; }

    public bool ProcessPayment(double amount)
    {
        return amount > 0;
    }
}

public class PayPal : IPaymentMethod
{
    private string _email;
    private string _password;

    public PayPal(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email
    {
        get => _email;
        set => _email = !value.Contains("@") ? throw new Exception("Неверный формат почты") : value;
    }

    public string Password
    {
        get => _password;
        private set => _password = value;
    }


    public bool ProcessPayment(double amount)
    {
        return amount > 1;
    }
}

public class CryptoWallet : IPaymentMethod
{
    private string address;

    public CryptoWallet(string address)
    {
        Address = address;
    }

    public string Address
    {
        get => Encoding.UTF8.GetString(Convert.FromBase64String(address));
        set => address = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }

    public bool ProcessPayment(double amount)
    {
        return amount % 1 == 0;
    }
}

public class PaymentProcessor
{
    public void ExecutePayment(IPaymentMethod method, double amount)
    {
        if (method.ProcessPayment(amount))
        {
            Console.WriteLine("Платеж успешен!");
        }
        else
        {
            Console.WriteLine("Ошибка платежа!");
        }
    }
}

public class Program
{
    static void Main()
    {
        var processor = new PaymentProcessor();

        var card = new CreditCard("1234-5678-9012-3456", 123);
        processor.ExecutePayment(card, 5000);

        var paypal = new PayPal("user@example.com", "pass123");
        processor.ExecutePayment(paypal, 0.5);

        var bitcoin = new CryptoWallet("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
        processor.ExecutePayment(bitcoin, 150.99);
    }
}