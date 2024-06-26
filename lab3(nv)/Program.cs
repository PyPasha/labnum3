using System;
using System.Collections.Generic;
using System.Linq;

public class Restaurant
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Type { get; set; }
    public double Rating { get; set; }
    public List<Dish> Dishes { get; set; }
    public string OpeningHours { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public bool IsOpen { get; set; }

    public Restaurant(string name, string address, string type, double rating, List<Dish> dishes, string openingHours, string phone, string website)
    {
        Name = name;
        Address = address;
        Type = type;
        Rating = rating;
        Dishes = dishes;
        OpeningHours = openingHours;
        Phone = phone;
        Website = website;
        IsOpen = true;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Ресторан: {Name}, Адреса: {Address}, Тип: {Type}, Рейтинг: {Rating}");
        Console.WriteLine("Меню:");     
    }

    public void UpdateRating(double newRating)
    {
        Rating = newRating;
    }

    public void ToggleOpenStatus()
    {
        IsOpen = !IsOpen;
    }
}

public class Dish
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public List<string> SpecificAttributes { get; set; }
    public int Calories { get; set; }
    public List<string> Allergens { get; set; }
    public bool IsVegetarian { get; set; }

    public Dish(string name, string description, double price, double weight, List<string> specificAttributes, int calories, List<string> allergens, bool isVegetarian)
    {
        Name = name;
        Description = description;
        Price = price;
        Weight = weight;
        SpecificAttributes = specificAttributes;
        Calories = calories;
        Allergens = allergens;
        IsVegetarian = isVegetarian;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Страва: {Name}, Опис: {Description}, Ціна: {Price}, Вага: {Weight}, Калорії: {Calories}, Веган: {IsVegetarian}");
        Console.WriteLine("Алергени: " + string.Join(", ", Allergens));
    }
}

public class Courier
{
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public double Rating { get; set; }
    public string TransportType { get; set; }
    public bool IsAvailable { get; set; }
    public int DeliveriesCompleted { get; set; }
    public int YearsOfExperience { get; set; }
    public List<string> LanguagesSpoken { get; set; }

    public Courier(string name, string contactInfo, double rating, string transportType, int yearsOfExperience, List<string> languagesSpoken)
    {
        Name = name;
        ContactInfo = contactInfo;
        Rating = rating;
        TransportType = transportType;
        IsAvailable = true;
        DeliveriesCompleted = 0;
        YearsOfExperience = yearsOfExperience;
        LanguagesSpoken = languagesSpoken;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Кур'єр: {Name}, Контакт: {ContactInfo}, Рейтинг: {Rating}, Транспорт: {TransportType}, Стаж: {YearsOfExperience}, Всього доставок: {DeliveriesCompleted},");
        Console.WriteLine("Мови: " + string.Join(", ", LanguagesSpoken));
    }

    public void CompleteDelivery()
    {
        DeliveriesCompleted++;
        Rating = Math.Min(Rating + 0.1, 5.0);
    }

    public void ToggleAvailability()
    {
        IsAvailable = !IsAvailable;
    }
}

public class Client
{
    public string Name { get; set; }
    public string DeliveryAddress { get; set; }
    public string ContactNumber { get; set; }
    public List<Order> OrderHistory { get; set; }
    public string Email { get; set; }
    public bool IsPremiumMember { get; set; }
    public double TotalSpent { get; set; }

    public Client(string name, string deliveryAddress, string contactNumber, string email, bool isPremiumMember)
    {
        Name = name;
        DeliveryAddress = deliveryAddress;
        ContactNumber = contactNumber;
        OrderHistory = new List<Order>();
        Email = email;
        IsPremiumMember = isPremiumMember;
        TotalSpent = 0;
    }

    public void AddOrderToHistory(Order order)
    {
        OrderHistory.Add(order);
        TotalSpent += order.TotalAmount;
    }

    public void DisplayOrderHistory()
    {
        Console.WriteLine($"Історія замовлень {Name}:");
        foreach (var order in OrderHistory)
        {
            Console.WriteLine($"Замовлення {order.Id}: Сума: {order.TotalAmount}, Статус: {order.Status}");
        }
    }
}

public class Order
{
    private static int orderCounter = 1;
    public int Id { get; private set; }
    public List<Dish> Dishes { get; set; }
    public double TotalAmount { get; set; }
    public string Status { get; set; }
    public Restaurant RestaurantInfo { get; set; }
    public Courier CourierInfo { get; set; }
    public Client ClientInfo { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime EstimatedDeliveryTime { get; set; }

    public Order(List<Dish> dishes, Restaurant restaurant, Courier courier, Client client)
    {
        Id = orderCounter++;
        Dishes = dishes;
        RestaurantInfo = restaurant;
        CourierInfo = courier;
        ClientInfo = client;
        TotalAmount = CalculateTotal();
        Status = "Створено";
        OrderTime = DateTime.Now;
        EstimatedDeliveryTime = OrderTime.AddMinutes(30);
    }

    private double CalculateTotal()
    {
        double total = 0;
        foreach (var dish in Dishes)
        {
            total += dish.Price;
        }
        return total;
    }

    public void UpdateStatus(string newStatus)
    {
        Status = newStatus;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Замовлення {Id}: Сума: {TotalAmount}, Статус: {Status}, Час: {OrderTime}, Час доставки: {EstimatedDeliveryTime}");
    }
}


public class DeliveryManager
{
    public List<Courier> AvailableCouriers { get; set; }
    public List<Order> Orders { get; set; }

    public DeliveryManager(List<Courier> couriers)
    {
        AvailableCouriers = couriers;
        Orders = new List<Order>();
    }

    public Courier AssignCourier()
    {
        var highestRatedCourier = AvailableCouriers
            .Where(c => c.IsAvailable)
            .OrderByDescending(c => c.Rating)
            .FirstOrDefault();

        if (highestRatedCourier != null)
        {
            foreach (var courier in AvailableCouriers)
            {
                if (courier != highestRatedCourier)
                    courier.ToggleAvailability();
            }

            return highestRatedCourier;
        }
        throw new Exception("Немає вільних кур'єрів.");
    }

    public void TrackOrder(Order order)
    {
        Console.WriteLine($"Замовлення {order.Id} для клієнта: {order.ClientInfo.Name}, Статус: {order.Status}");
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);
    }

    public void DisplayInfo()
    {
        Console.WriteLine("Інформація про доставку:");
        foreach (var order in Orders)
        {
            order.DisplayInfo();
        }
        var highestRatedCourier = AvailableCouriers
            .Where(c => c.IsAvailable)
            .OrderByDescending(c => c.Rating)
            .FirstOrDefault();
        if (highestRatedCourier != null)
        {
            highestRatedCourier.DisplayInfo();
        }
    }
}

public class MockTester
{
    private List<Client> Clients { get; set; }
    private DeliveryManager DeliveryManager { get; set; }

    public MockTester(List<Client> clients, DeliveryManager deliveryManager)
    {
        Clients = clients;
        DeliveryManager = deliveryManager;
    }

    public void CreateAndProcessOrder()
    {
        try
        {
            Random rand = new Random();
            var client = Clients[rand.Next(Clients.Count)];
            var dishes = new List<Dish>
            {
                new Dish("Pizza", "Cheese Pizza", 150.40, 500, new List<string> { "Cheese", "Margarita" }, 800, new List<string> { "Gluten", "Dairy" }, false),
                new Dish("Burger", "Beef Burger", 99.99, 274, new List<string> { "Beef", "Lettuce" }, 600, new List<string> { "Gluten" }, false)
            };
            var restaurant = new Restaurant("FastFood", "Shevchenko St.", "Fast Food", 4.5, dishes, "9:00 - 21:00", "+3800677773223", "www.fastfood.com");
            var courier = DeliveryManager.AssignCourier();
            var order = new Order(dishes, restaurant, courier, client);

            client.AddOrderToHistory(order);
            DeliveryManager.AddOrder(order);

            courier.CompleteDelivery();
            courier.ToggleAvailability();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

public class Payment
{
    public int PaymentId { get; private set; }
    private static int paymentCounter = 1;
    public Order Order { get; set; }
    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    public bool IsSuccessful { get; set; }

    public Payment(Order order, double amount, string paymentMethod)
    {
        PaymentId = paymentCounter++;
        Order = order;
        Amount = amount;
        PaymentMethod = paymentMethod;
        IsSuccessful = false;
    }

    public void ProcessPayment()
    {
        IsSuccessful = true;
        Console.WriteLine($"Платіж {PaymentId} успішно оброблений для замовлення {Order.Id}, Сума: {Amount}");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Платіж {PaymentId}, Замовлення {Order.Id}, Сума: {Amount}, Метод: {PaymentMethod}, Успішно: {IsSuccessful}");
    }
}

public class Promotion
{
    public string Code { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime ValidUntil { get; set; }

    public Promotion(string code, double discountPercentage, DateTime validUntil)
    {
        Code = code;
        DiscountPercentage = discountPercentage;
        ValidUntil = validUntil;
    }

    public double ApplyDiscount(double totalAmount)
    {
        if (DateTime.Now <= ValidUntil)
        {
            return totalAmount - (totalAmount * DiscountPercentage / 100);
        }
        return totalAmount;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Промокод: {Code}, Знижка: {DiscountPercentage}%, Дійсний: {ValidUntil}");
    }
}

public class Review
{
    public int ReviewId { get; private set; }
    private static int reviewCounter = 1;
    public Client Reviewer { get; set; }
    public Restaurant Restaurant { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime Date { get; set; }

    public Review(Client reviewer, Restaurant restaurant, string content, int rating)
    {
        ReviewId = reviewCounter++;
        Reviewer = reviewer;
        Restaurant = restaurant;
        Content = content;
        Rating = rating;
        Date = DateTime.Now;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Відгук {ReviewId} клієнта {Reviewer.Name} для {Restaurant.Name}: {Content}, Рейтинг: {Rating}, Дата: {Date}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var clients = new List<Client>
        {
            new Client("Maria", "Polkova St.", "+380699842347", "maria@mail.com", true),
            new Client("Egor", "Dotsenko St.", "+380668745691", "egor@mail.com", false),
            new Client("Viktor", "Yaroslavska St.", "+380688973691", "viktor@mail.com", false)
        };

        var couriers = new List<Courier>
        {
            new Courier("Anton Vasiliev", "+380669494556", 4.5, "Bike", 3, new List<string> { "Ukrainian", "English" }),
            new Courier("Valeriy Petrov", "+380675692391", 4.8, "Car", 5, new List<string> { "Ukrainian" })
        };

        var deliveryManager = new DeliveryManager(couriers);
        var mockTester = new MockTester(clients, deliveryManager);

        mockTester.CreateAndProcessOrder();

        var order = deliveryManager.Orders[0];

        order.RestaurantInfo.DisplayInfo();

        foreach (var dish in order.Dishes)
        {
            dish.DisplayInfo();
        }

        order.DisplayInfo();

        var promotion = new Promotion("SUMMER24", 15, DateTime.Now.AddDays(30));
        promotion.DisplayInfo();

        var payment = new Payment(order, order.TotalAmount, "Кредитна карта");
        payment.ProcessPayment();
        payment.DisplayInfo();


        order.CourierInfo.DisplayInfo();

        order.UpdateStatus("В процесі");
        deliveryManager.TrackOrder(order);
        order.UpdateStatus("Доставлено");
        deliveryManager.TrackOrder(order);


        var review = new Review(clients[1], order.RestaurantInfo, "Чудова їжа та сервіс!", 5);
        review.DisplayInfo();
    }
}
