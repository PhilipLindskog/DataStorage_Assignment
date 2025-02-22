using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Presentation_Console.Interfaces;

namespace Presentation_Console.Dialogs;

public class UserDialogs(IUserService userService) : IUserDialogs
{
    private readonly IUserService _userService = userService;

    public async Task MenuOptions()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("---------- USER OPTIONS ----------");
            Console.WriteLine("1: Add new User");
            Console.WriteLine("2: View all User");
            Console.WriteLine("3: View specific User");
            Console.WriteLine("4: Update User");
            Console.WriteLine("5: Delete User");
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
        Console.WriteLine("----------- CREATE USER ----------");

        var user = UserFactory.Create();

        Console.Write("Enter User first name: ");
        user.FirstName = Console.ReadLine()!;
        if (string.IsNullOrEmpty(user.FirstName))
        {
            Console.WriteLine("First name cannot be empty");
            return;
        }

        Console.Write("Enter User last name: ");
        user.LastName = Console.ReadLine()!;
        if (string.IsNullOrEmpty(user.LastName))
        {
            Console.WriteLine("Last name cannot be empty");
            return;
        }

        Console.Write("Enter User E-mail: ");
        user.Email = Console.ReadLine()!;
        if (string.IsNullOrEmpty(user.Email))
        {
            Console.WriteLine("Email name cannot be empty");
            return;
        }

        var result = await _userService.CreateUserAsync(user);
        if (result != null)
        {
            Console.WriteLine("User added successfully");
        }
        else
        {
            Console.WriteLine("Failed to add User");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }

    public async Task GetAllOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("---------- ALL USERS -----------");

        var users = await _userService.GetAllUsersAsync();
        if (users is Result<IEnumerable<User>> { Success: true, Data: not null } userResult)
        {
            foreach (var user in userResult.Data)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.FirstName} {user.LastName} Email: {user.Email}");
                Console.WriteLine("-------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No Users found");
        }
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task GetByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("------------ VIEW USER BY ID ----------");
        Console.Write("Enter User ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var user = await _userService.GetUserAsync(x => x.Id == id);

        if (user is Result<User> { Success: true, Data: not null } userResultById)
        {
            Console.WriteLine($"User with ID: {userResultById.Data.Id} Name: {userResultById.Data.FirstName} {userResultById.Data.LastName} Email: {userResultById.Data.Email}");
        }
        else
        {
            Console.WriteLine("No User by that ID found");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task UpdateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- UPDATE USER BY ID -----------");
        Console.Write("Enter User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid ID. Try again.");
            return;
        }

        Console.Write("Enter new first name: ");
        var newFirstName = Console.ReadLine();
        if (string.IsNullOrEmpty(newFirstName))
        {
            Console.WriteLine("First name cannot be empty.");
            return;
        }

        Console.Write("Enter new last name: ");
        var newLasttName = Console.ReadLine();
        if (string.IsNullOrEmpty(newLasttName))
        {
            Console.WriteLine("Last name cannot be empty.");
            return;
        }

        Console.Write("Enter new E-mail: ");
        var newEmail = Console.ReadLine();
        if (string.IsNullOrEmpty(newEmail))
        {
            Console.WriteLine("Email cannot be empty.");
            return;
        }

        var updateForm = new UserUpdateForm
        {
            Id = userId,
            FirstName = newFirstName,
            LastName = newLasttName,
            Email = newEmail
        };

        var result = await _userService.UpdateUserAsync(userId, updateForm);

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
        Console.WriteLine("----------- DELETE USER BY ID ----------");

        Console.Write("Enter User ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var user = await _userService.GetUserAsync(x => x.Id == id);
        if (user.Success == false)
        {
            Console.WriteLine("User not found.");
            return;
        }

        var result = await _userService.DeleteUserAsync(id);

        if (result.Success == true)
        {
            Console.WriteLine("User deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Could not delete User. Reason:{result.ErrorMessage}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
