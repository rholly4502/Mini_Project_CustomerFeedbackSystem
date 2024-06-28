// See https://aka.ms/new-console-template for more information
using SubmitFeedback.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DbFeedback = SubmitFeedback.Models;
using System;


var serviceProvider = new ServiceCollection()
    .AddDbContext<CustomerFeedbackSystemContext>(options => options.UseSqlServer("CustomerFeedbackSystem"))
    .BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<CustomerFeedbackSystemContext>();

while (true)
{
    Console.WriteLine("Customer Feedback System");
    Console.WriteLine("----------------------------");
    Console.WriteLine("1. Add Feedback");
    Console.WriteLine("2. Exit");
    Console.Write("Select an option: ");
    var option = Console.ReadLine();

    switch (option)
    {
        case "1":
            AddFeedback(dbContext);
            break;
        case "2":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            Pause();
            break;
    }
}

static void AddFeedback(CustomerFeedbackSystemContext dbContext)
{
    Console.WriteLine("--------");
    Console.WriteLine("Add Feedback");
    Console.WriteLine("--------");

    var feed = new Feedback();
    var customer = new Customer();

    Console.Write("Masukkan Nama      : ");
    customer.Name = Console.ReadLine();

    Console.Write("Masukkan Email     : ");
    customer.Email = Console.ReadLine();

    Console.Write("Masukkan Feedback  : ");
    feed.FeedbackText = Console.ReadLine();

    feed.Status = "Pending"; // Default value

    // Cek apakah customer dengan nama dan email tersebut sudah ada
    var existingCustomer = dbContext.Customers
                                    .SingleOrDefault(c => c.Name == customer.Name && c.Email == customer.Email);

    if (existingCustomer != null)
    {
        // Jika customer sudah ada, gunakan CustomerId yang ada
        feed.CustomerId = existingCustomer.CustomerId;
    }
    else
    {
        dbContext.Customers.Add(customer);
        dbContext.SaveChanges();
    }

    feed.CustomerId = customer.CustomerId;
    dbContext.Feedbacks.Add(feed);
    dbContext.SaveChanges();

    if (feed.FeedbackId > 0)
    {
        Console.WriteLine();
        Console.WriteLine($"Feedback added successfully:");
        Console.WriteLine($"{"Customer ID              :".PadRight(25)} {customer.CustomerId}");
        Console.WriteLine($"{"Customer Name            :".PadRight(25)} {customer.Name}");
        Console.WriteLine($"{"Feedback Text            :".PadRight(25)} {feed.FeedbackText}");
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Failed to add Feedback.");
    }
    Pause();
}
static void Pause()
{
    Console.WriteLine("Press any key to return to the main menu...");
    Console.ReadKey(true);
    Console.WriteLine();
}
Console.WriteLine("Hello, World!");
