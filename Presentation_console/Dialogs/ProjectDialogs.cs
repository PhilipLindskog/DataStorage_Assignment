using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Presentation_Console.Interfaces;

namespace Presentation_Console.Dialogs;

public class ProjectDialogs(IProjectService projectService, ICustomerService customerService, IUserService userService, IStatusTypeService statusTypeService, IProductService productService) : IProjectDialogs
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IUserService _userService = userService;
    private readonly IStatusTypeService _statusTypeService = statusTypeService;
    private readonly IProductService _productService = productService;

    public async Task MenuOptions()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("---------- STATUS TYPE OPTIONS ----------");
            Console.WriteLine("1: Add new Project");
            Console.WriteLine("2: View all Projects");
            Console.WriteLine("3: View specific Project");
            Console.WriteLine("4: Update Project");
            Console.WriteLine("5: Delete Project");
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
        Console.WriteLine("----------- CREATE PROJECT ----------");

        var project = ProjectFactory.Create();

        Console.Write("Enter Project Title: ");
        project.Title = Console.ReadLine()!;
        if (string.IsNullOrEmpty(project.Title))
        {
            Console.WriteLine("Title cannot be empty");
            return;
        }

        Console.Write("Enter Project Description: ");
        project.Description = Console.ReadLine()!;

        Console.Write("Enter Project start date (yyyy-MM-dd): ");
        project.StartDate = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", null);

        Console.WriteLine("Enter Project end date (yyyy-MM-dd): ");
        project.EndDate = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", null);

        Console.Write("Enter customer ID: ");
        project.CustomerId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter Status ID: ");
        project.StatusId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter User ID: ");
        project.UserId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter Product ID: ");
        project.ProductId = int.Parse(Console.ReadLine()!);

        var result = await _projectService.CreateProjectAsync(project);
        if (result != null)
        {
            Console.WriteLine("Project added successfully");
        }
        else
        {
            Console.WriteLine("Failed to add Project");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }

    public async Task GetAllOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("---------- ALL PROJECTS -----------");

        var projects = await _projectService.GetAllProjectsAsync();
        if (projects is Result<IEnumerable<Project>> { Success: true, Data: not null } projectResult)
        {
            foreach (var project in projectResult.Data)
            {
                var (customerName, statusName, userName, userEmail, productName, productPrice) = await GetDetailsAsync(project.CustomerId, project.StatusId, project.UserId, project.ProductId);

                Console.WriteLine($"--- ID: {project.Id}, Title: {project.Title} ---");
                Console.WriteLine($"Description: {project.Description}");
                Console.WriteLine($"Start date: {project.StartDate} End date: {project.EndDate}");
                Console.WriteLine($"Customer: {customerName}");
                Console.WriteLine($"Service: {productName} Price: {productPrice}");
                Console.WriteLine($"Assigned User: {userName} Email: {userEmail}");
                Console.WriteLine($"Project status: {statusName}");
                Console.WriteLine("-------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No projects found.");
        }
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task GetByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("------------ VIEW PROJECT BY ID ----------");
        Console.Write("Enter project ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var project = await _projectService.GetProjectsAsync(x => x.Id == id);

        if (project is Result<Project> { Success: true, Data: not null } projectByIdResult)
        {
            var (customerName, statusName, userName, userEmail, productName, productPrice) = await GetDetailsAsync(projectByIdResult.Data.CustomerId, projectByIdResult.Data.StatusId, projectByIdResult.Data.UserId, projectByIdResult.Data.ProductId);

            Console.WriteLine($"--- ID: {projectByIdResult.Data.Id}, Title: {projectByIdResult.Data.Title} ---");
            Console.WriteLine($"Description: {projectByIdResult.Data.Description}");
            Console.WriteLine($"Start date: {projectByIdResult.Data.StartDate} End date: {projectByIdResult.Data.EndDate}");
            Console.WriteLine($"Customer: {customerName}");
            Console.WriteLine($"Service: {productName} Price: {productPrice}");
            Console.WriteLine($"Assigned User: {userName} Email: {userEmail}");
            Console.WriteLine($"Project status: {statusName}");
            Console.WriteLine("-------------------------------------------");
        }
        else
        {
            Console.WriteLine("No Project by that ID found");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task<(string customerName, string statusName, string userName, string userEmail, string productName, decimal price)> GetDetailsAsync(int customerId, int statusId, int userId, int productId)
    {
        var customerResult = await _customerService.GetCustomerAsync(x => x.Id == customerId);
        var statusResult = await _statusTypeService.GetStatusTypeAsync(x => x.Id == statusId);
        var userResult = await _userService.GetUserAsync(x => x.Id == userId);
        var productResult = await _productService.GetProductAsync(x => x.Id == productId);

        var customer = customerResult as Result<Customer>;
        var status = statusResult as Result<StatusType>;
        var user = userResult as Result<User>;
        var product = productResult as Result<Product>;

        string customerName = customer?.Success == true && customer.Data != null ? customer.Data.CustomerName : "No customer assigned";
        string statusName = status?.Success == true && status.Data != null ? status.Data.StatusName : "No status assigned";
        string userName = user?.Success == true && user.Data != null ? $"{user.Data.FirstName} {user.Data.LastName}" : "No user assigned";
        string userEmail = user?.Success == true && user.Data != null ? user.Data.Email : "No Email";
        string productName = product?.Success == true && product.Data != null ? product.Data.ProductName : "No service assigned";
        decimal price = product?.Success == true && product.Data != null ? product.Data.Price : 0;

        return (customerName, statusName, userName, userEmail, productName, price);
    }

    public async Task UpdateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- UPDATE PROJECT BY ID -----------");
        Console.Write("Enter Project ID: ");

        if (!int.TryParse(Console.ReadLine(), out int projectId))
        {
            Console.WriteLine("Invalid ID. Try again.");
            return;
        }

        Console.WriteLine("Enter new Project Title: ");
        var newProjectTitle = Console.ReadLine();
        if (string.IsNullOrEmpty(newProjectTitle))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Enter Project Description: ");
        var newProjectDescription = Console.ReadLine()!;

        Console.Write("Enter Project start date (yyyy-MM-dd): ");
        var newProjectStartDate = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", null);

        Console.WriteLine("Enter Project end date (yyyy-MM-dd): ");
        var newProjectEndDate = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", null);

        Console.Write("Enter customer ID: ");
        var newCustomerId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter Status ID: ");
        var newStatusId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter User ID: ");
        var newUserId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter Product ID: ");
        var newProductId = int.Parse(Console.ReadLine()!);

        var updateForm = new ProjectUpdateForm
        {
            Id = projectId,
            Title = newProjectTitle,
            Description = newProjectDescription,
            StartDate = newProjectStartDate,
            EndDate = newProjectEndDate,
            CustomerId = newCustomerId,
            StatusId = newStatusId,
            UserId = newUserId,
            ProductId = newProductId
        };

        var result = await _projectService.UpdateProjectAsync(projectId, updateForm);

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
        Console.WriteLine("----------- DELETE PROJECT BY ID ----------");

        Console.WriteLine("Enter Project ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var project = await _projectService.GetProjectsAsync(x => x.Id == id);
        if (project.Success == false)
        {
            Console.WriteLine("StatusType not found.");
            return;
        }

        var result = await _projectService.DeleteProjectAsync(id);

        if (result.Success == true)
        {
            Console.WriteLine("Project deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Could not delete Project. Reason:{result.ErrorMessage}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
