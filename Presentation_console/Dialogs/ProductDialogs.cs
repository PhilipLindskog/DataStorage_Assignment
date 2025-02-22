using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Presentation_Console.Interfaces;

namespace Presentation_Console.Dialogs;

public class ProductDialogs(IProductService productService) : IProductDialogs
{
    private readonly IProductService _productService = productService;

    public async Task MenuOptions()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("---------- SERVICE OPTIONS ----------");
            Console.WriteLine("1: Add new Service");
            Console.WriteLine("2: View all Service");
            Console.WriteLine("3: View specific Service");
            Console.WriteLine("4: Update Service");
            Console.WriteLine("5: Delete Service");
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
        Console.WriteLine("----------- CREATE SERVICE ----------");

        var service = ProductFactory.Create();

        Console.Write("Enter Service name: ");
        service.ProductName = Console.ReadLine()!;
        if (string.IsNullOrEmpty(service.ProductName))
        {
            Console.WriteLine("Status name cannot be empty");
            return;
        }

        Console.Write("Enter service price: ");
        service.Price = decimal.Parse(Console.ReadLine()!);

        var result = await _productService.CreateProductAsync(service);
        if (result != null)
        {
            Console.WriteLine("Service added successfully");
        }
        else
        {
            Console.WriteLine("Failed to add Service");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }

    public async Task GetAllOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("---------- ALL Services -----------");

        var services = await _productService.GetAllProductsAsync();
        if (services is Result<IEnumerable<Product>> { Success: true, Data: not null } serviceResult)
        {
            foreach (var service in serviceResult.Data)
            {
                Console.WriteLine($"ID: {service.Id}, Name: {service.ProductName} Price: {service.Price}");
                Console.WriteLine("-------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No Service found");
        }
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task GetByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("------------ VIEW SERVICE BY ID ----------");
        Console.Write("Enter Service ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var service = await _productService.GetProductAsync(x => x.Id == id);

        if (service is Result<Product> { Success: true, Data: not null } serviceResultById)
        {
            Console.WriteLine($"Service ID: {serviceResultById.Data.Id} Name: {serviceResultById.Data.ProductName} Price: {serviceResultById.Data.Price}");
        }
        else
        {
            Console.WriteLine("No Service by that ID found");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task UpdateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- UPDATE SERVICE BY ID -----------");
        Console.Write("Enter StatusType ID: ");

        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("Invalid ID. Try again.");
            return;
        }

        Console.Write("Enter new StatusType name: ");
        var newServiceName = Console.ReadLine();
        if (string.IsNullOrEmpty(newServiceName))
        {
            Console.WriteLine("Service name cannot be empty.");
            return;
        }

        Console.Write("Enter new price: ");
        var newPrice = decimal.Parse(Console.ReadLine()!);

        var updateForm = new ProductUpdateForm
        {
            Id = serviceId,
            ProductName = newServiceName,
            Price = newPrice
        };

        var result = await _productService.UpdateProductAsync(serviceId, updateForm);

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
        Console.WriteLine("----------- DELETE Service BY ID ----------");

        Console.Write("Enter Service ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var status = await _productService.GetProductAsync(x => x.Id == id);
        if (status.Success == false)
        {
            Console.WriteLine("Service not found.");
            return;
        }

        var result = await _productService.DeleteProductAsync(id);

        if (result.Success == true)
        {
            Console.WriteLine("Service deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Could not delete Service. Reason:{result.ErrorMessage}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
