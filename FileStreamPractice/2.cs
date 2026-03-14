using System;
using System.IO;

namespace FileStreamPractice
{
    internal class Program
    {
        private struct Product
        {
            public int Id;
            public string Name;
            public int Quantity;
            public decimal Price;
        }

        private static readonly string Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.dat");

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 - Добавление нового товара");
                Console.WriteLine("2 - Поиск по названию");
                Console.WriteLine("3 - Обновление количества");
                Console.WriteLine("4 - Экспорт в CSV");
                Console.WriteLine("5 - Выход");

                if (!int.TryParse(Console.ReadLine(), out var action))
                {
                    Console.WriteLine("Введите число!");
                    continue;
                }

                switch (action)
                {
                    case 1:
                        AddProduct();
                        break;

                    case 2:
                        SearchProduct();
                        break;

                    case 3:
                        UpdateQuantity();
                        break;

                    case 4:
                        ExportCsv();
                        break;

                    case 5:
                        return;
                }
            }
        }

        private static void AddProduct()
        {
            Console.Write("Название: ");
            var name = Console.ReadLine();

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
            {
                Console.WriteLine("Количество должно быть > 0");
                return;
            }

            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
            {
                Console.WriteLine("Цена должна быть > 0");
                return;
            }

            var id = GetNextId();

            var product = new Product
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                Price = price
            };

            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fs))
            {
                fs.Seek(0, SeekOrigin.End);

                bw.Write(product.Id);
                bw.Write(product.Name);
                bw.Write(product.Quantity);
                bw.Write(product.Price);
            }

            Console.WriteLine("Товар добавлен.");
        }

        private static int GetNextId()
        {
            var maxId = 0;

            if (!File.Exists(Path))
                return 1;

            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    var id = br.ReadInt32();
                    br.ReadString();
                    br.ReadInt32();
                    br.ReadDecimal();

                    if (id > maxId)
                        maxId = id;
                }
            }

            return maxId + 1;
        }

        private static void SearchProduct()
        {
            Console.Write("Введите название: ");
            var search = Console.ReadLine();

            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    var id = br.ReadInt32();
                    var name = br.ReadString();
                    var quantity = br.ReadInt32();
                    var price = br.ReadDecimal();

                    if (name.Equals(search, StringComparison.OrdinalIgnoreCase))
                        Console.WriteLine($"ID: {id} | {name} | {quantity} | {price}");
                }
            }
        }

        private static void UpdateQuantity()
        {
            Console.Write("Введите ID товара: ");

            if (!int.TryParse(Console.ReadLine(), out var id))
                return;

            Console.Write("Новое количество: ");

            if (!int.TryParse(Console.ReadLine(), out var newQuantity) || newQuantity <= 0)
                return;

            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (var br = new BinaryReader(fs))
            using (var bw = new BinaryWriter(fs))
            {
                while (fs.Position < fs.Length)
                {
                    var position = fs.Position;

                    var productId = br.ReadInt32();
                    var name = br.ReadString();
                    var price = br.ReadDecimal();

                    if (productId == id)
                    {
                        fs.Seek(position, SeekOrigin.Begin);

                        bw.Write(productId);
                        bw.Write(name);
                        bw.Write(newQuantity);
                        bw.Write(price);

                        Console.WriteLine("Количество обновлено.");
                        return;
                    }
                }
            }

            Console.WriteLine("Товар не найден.");
        }

        private static void ExportCsv()
        {
            var csvPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.csv");

            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            using (var sw = new StreamWriter(csvPath))
            {
                while (fs.Position < fs.Length)
                {
                    var id = br.ReadInt32();
                    var name = br.ReadString();
                    var quantity = br.ReadInt32();
                    var price = br.ReadDecimal();

                    sw.WriteLine($"{id};{name};{quantity};{price}");
                }
            }

            Console.WriteLine("Экспорт завершён.");
        }
    }
}