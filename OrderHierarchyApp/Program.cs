using System;

namespace OrderHierarchyApp
{
    public class Order
    {
        public int OrderId { get; set; }
        public double TotalAmount { get; set; }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID заказа: {OrderId}; Стоимость заказа: {TotalAmount}");
        }

        public Order(int id, double totalAmount)
        {
            if (totalAmount < 0)
            {
                throw new Exception("Сумма должна быть больше 0");
            }
            OrderId = id;
            TotalAmount = totalAmount;
        }
    }

    public class OnlineOrder : Order
    {
        public string DeliveryAddress { get; set; }

        public OnlineOrder(int id, double totalAmount, string address) : base(id, totalAmount)
        {
            DeliveryAddress = address;
        }
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Адрес доставки: {DeliveryAddress}");
        }
    }

    public class InStoreOrder : Order
    {
        public string StoreLocation { get; set; }
        public InStoreOrder(int id, double totalAmount, string location) : base(id, totalAmount)
        {
            StoreLocation = location;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Локация магазина: {StoreLocation}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // позитивные тесты
            Order onlineOrder = new OnlineOrder(1, 2500.50, "Москва, ул. Ленина, 10");
            Order inStoreOrder = new InStoreOrder(2, 1200.00, "ТЦ Европа");

            onlineOrder.DisplayInfo();
            inStoreOrder.DisplayInfo();

            // негативный тест
            try
            {
                Order invalidOrder = new OnlineOrder(3, -100, "Ошибка");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // граничный тест
            Order boundaryOrder = new OnlineOrder(0, 500, "Граничный случай");
            boundaryOrder.DisplayInfo();
        }

    }
}
