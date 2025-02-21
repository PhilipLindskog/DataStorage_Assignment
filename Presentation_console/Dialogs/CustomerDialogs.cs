using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Presentation_Console.Interfaces;

namespace Presentation_Console.Dialogs;

public class CustomerDialogs(ICustomerService customerService) : ICustomerDialogs
{
    private readonly ICustomerService _customerService = customerService;

    public async Task MenuOptions()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("---------- CUSTOMER OPTIONS ----------");
            Console.WriteLine("1: Add new Customer");
            Console.WriteLine("2: View all Customer");
            Console.WriteLine("3: View specific Customer");
            Console.WriteLine("4: Update Customer");
            Console.WriteLine("5: Delete Customer");
            Console.WriteLine("B: Go back to main menu");
            Console.WriteLine("");
            Console.Write("Enter option: ");

            var option = Console.ReadLine()!;

            switch (option.ToLower())
            {
                case "1":
                    await CreateOptionAsync();
                    break;

                case "2":
                    await GetAllOptionAsync();
                    break;

                case "3":
                    await GetByIdAsync();
                    break;

                case "4":
                    await UpdateOptionAsync();
                    break;

                case "5":
                    await DeleteOptionAsync();
                    break;

                case "b":
                    return;

                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        } while (true);
    }

    public async Task CreateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- CREATE CUSTOMER  ----------");

        var customer = CustomerFactory.Create();

        Console.Write("Enter Customer name: ");
        customer.CustomerName = Console.ReadLine()!;

        if (string.IsNullOrEmpty(customer.CustomerName))
        {
            Console.WriteLine("Customer name cannot be empty");
            return;
        }

        var result = await _customerService.CreateCustomerAsync(customer);
        if (result != null)
        {
            Console.WriteLine("Customer added successfully");
        }
        else
        {
            Console.WriteLine("Failed to add Customer");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }

    public async Task GetAllOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("---------- ALL CUSTOMERS -----------");

        var customers = await _customerService.GetAllCustomersAsync();
        if (customers is Result<IEnumerable<Customer>> { Success: true, Data: not null } customerResult)
        {
            foreach (var customer in customerResult.Data)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.CustomerName}");
                Console.WriteLine("-------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No Customers found");
        }
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    public async Task GetByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("------------ VIEW CUSTOMER BY ID ----------");
        Console.Write("Enter Customer ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var customer = await _customerService.GetCustomerAsync(x => x.Id == id);

        if (customer is Result<Customer> { Success: true, Data: not null } customerResultById)
        {
            Console.WriteLine($"Customer with ID: {customerResultById.Data.Id} Name: {customerResultById.Data.CustomerName}");
        }
        else
        {
            Console.WriteLine("No Customer by that ID found");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task UpdateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- UPDATE CUSTOMER BY ID -----------");
        Console.Write("Enter Customer ID: ");

        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid ID. Try again.");
            return;
        }

        Console.WriteLine("Enter new Customer name: ");
        var newCustomerName = Console.ReadLine();

        if (string.IsNullOrEmpty(newCustomerName))
        {
            Console.WriteLine("Customer name cannot be empty.");
            return;
        }

        var updateForm = new CustomerUpdateForm
        {
            Id = statusId,
            CustomerName = newCustomerName
        };

        var result = await _customerService.UpdateCustomerAsync(statusId, updateForm);

        if (result.Success)
        {
            Console.WriteLine($"ID: {updateForm.Id} was updated successfully.");
        }
        else
        {
            Console.WriteLine("Failed to update!");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task DeleteOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- DELETE CUSTOMER BY ID ----------");

        Console.WriteLine("Enter Customer ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var customer = await _customerService.GetCustomerAsync(x => x.Id == id);
        if (customer.Success == false)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        var result = await _customerService.DeleteCustomerAsync(id);

        if (result.Success == true)
        {
            Console.WriteLine("Customer deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Could not delete Customer. Reason:{result.ErrorMessage}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
