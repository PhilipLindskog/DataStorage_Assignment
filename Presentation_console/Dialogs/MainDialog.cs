using Presentation_Console.Interfaces;

namespace Presentation_Console.Dialogs;

public class MainDialog(IStatusDialog statusDialog, IProjectDialogs projectDialogs, ICustomerDialogs customerDialogs, IProductDialogs productDialogs, IUserDialogs userDialogs) : IMainDialog
{

    public async Task MainMenuAsync()
    {
        var isRunning = true;

        do
        {
            Console.Clear();
            Console.WriteLine("---------- MAIN MENU ----------");
            Console.WriteLine("1: Manage Projects");
            Console.WriteLine("2: Manage Customers");
            Console.WriteLine("3: Manage Services");
            Console.WriteLine("4: Manage Users");
            Console.WriteLine("5: Manage StatusTypes");
            Console.WriteLine("Q: Quit Application");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("");
            Console.Write("Enter Option: ");

            var option = Console.ReadLine()!;

            switch (option.ToLower())
            {
                case "1":
                    await projectDialogs.MenuOptions();
                    break;

                case "2":
                    await customerDialogs.MenuOptions();
                    break;

                case "3":
                    await productDialogs.MenuOptions();
                    break;

                case "4":
                    await userDialogs.MenuOptions();
                    break;

                case "5":
                    await statusDialog.MenuOptions();
                    break;

                case "q":
                    var answer = QuitApplication();
                    if (answer == true)
                        isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
        while (isRunning);
    }

    public bool QuitApplication()
    {
        Console.Clear();
        Console.WriteLine("------------ QUIT APPLICATION ----------");
        Console.WriteLine("Do you wish to quit the application?");
        Console.Write("Press Y/N: ");

        var answer = Console.ReadLine()!;

        if (answer.ToLower() == "y")
        {
            return true;
        }
        return false;
    }
}
