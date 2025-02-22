using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Presentation_Console.Interfaces;
using System.Linq.Expressions;

namespace Presentation_Console.Dialogs;

public class StatusDialog(IStatusTypeService statusTypeService) : IStatusDialog
{
    private readonly IStatusTypeService _statusTypeService = statusTypeService;

    public async Task MenuOptions()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("---------- STATUS TYPE OPTIONS ----------");
            Console.WriteLine("1: Add new StatusType");
            Console.WriteLine("2: View all StatusTypes");
            Console.WriteLine("3: View specific StatusType");
            Console.WriteLine("4: Update StatusType");
            Console.WriteLine("5: Delete StatusType");
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
        Console.WriteLine("----------- CREATE STATUSTYPE ----------");

        var status = StatusTypeFactory.Create();

        Console.Write("Enter StatusType name: ");
        status.StatusName = Console.ReadLine()!;

        if (string.IsNullOrEmpty(status.StatusName))
        {
            Console.WriteLine("Status name cannot be empty");
            return;
        }

        var result = await _statusTypeService.CreateStatusTypeAsync(status);
        if (result != null)
        {
            Console.WriteLine("StatusType added successfully");
        }
        else
        {
            Console.WriteLine("Failed to add StatusType");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");

        Console.ReadKey();
    }

    public async Task GetAllOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("---------- ALL STATUSTYPES -----------");

        var statuses = await _statusTypeService.GetAllStatusTypesAsync();
        if (statuses is Result<IEnumerable<StatusType>> { Success: true, Data: not null } statusTypeResult)
        {
            foreach (var status in statusTypeResult.Data)
            {
                Console.WriteLine($"ID: {status.Id}, Name: {status.StatusName}");
                Console.WriteLine("-------------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("No StatusTypes found");
        }
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task GetByIdAsync()
    {
        Console.Clear();
        Console.WriteLine("------------ VIEW STATUSTYPE BY ID ----------");
        Console.Write("Enter StatusType ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var status = await _statusTypeService.GetStatusTypeAsync(x => x.Id == id);

        if (status is Result<StatusType> { Success: true, Data: not null } statusTypeResultById)
        {
            Console.WriteLine($"StatusType with ID: {statusTypeResultById.Data.Id} Name: {statusTypeResultById.Data.StatusName}");
        }
        else
        {
            Console.WriteLine("No StatusType by that ID found");
        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public async Task UpdateOptionAsync()
    {
        Console.Clear();
        Console.WriteLine("----------- UPDATE STATUSTYPE BY ID -----------");
        Console.Write("Enter StatusType ID: ");

        if (!int.TryParse(Console.ReadLine(), out int statusId))
        {
            Console.WriteLine("Invalid ID. Try again.");
            return;
        }

        Console.Write("Enter new StatusType name: ");
        var newStatusName = Console.ReadLine();

        if (string.IsNullOrEmpty(newStatusName))
        {
            Console.WriteLine("StatusType name cannot be empty.");
            return;
        }

        var updateForm = new StatusTypeUpdateForm
        {
            Id = statusId,
            StatusName = newStatusName,
        };

        var result = await _statusTypeService.UpdateStatusTypeAsync(statusId, updateForm);

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
        Console.WriteLine("----------- DELETE STATUSTYPE BY ID ----------");

        Console.Write("Enter StatusType ID: ");

        var id = int.Parse(Console.ReadLine()!);

        var status = await _statusTypeService.GetStatusTypeAsync(x => x.Id == id);
        if (status.Success == false)
        {
            Console.WriteLine("StatusType not found.");
            return;
        }

        var result = await _statusTypeService.DeleteStatusTypeAsync(id);

        if (result.Success == true)
        {
            Console.WriteLine("StatusType deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Could not delete StatusType. Reason:{result.ErrorMessage}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
