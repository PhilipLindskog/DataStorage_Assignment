using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation_Console.Dialogs;
using Presentation_Console.Interfaces;

var service = new ServiceCollection()
    .AddDbContext<DataContext>(x => x.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_database.mdf;Integrated Security=True").UseLazyLoadingProxies())
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IStatusTypeRepository, StatusTypeRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IProjectRepository, ProjectRepository>()

    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IStatusTypeService, StatusTypeService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IProjectService, ProjectService>()

    .AddScoped<ICustomerDialogs, CustomerDialogs>()
    .AddScoped<IProductDialogs, ProductDialogs>()
    .AddScoped<IStatusDialog, StatusDialog>()
    .AddScoped<IUserDialogs, UserDialogs>()
    .AddScoped<IProjectDialogs, ProjectDialogs>()
    .AddScoped<IMainDialog, MainDialog>();

var serviceProvider = service.BuildServiceProvider();
var mainDialog = serviceProvider.GetRequiredService<IMainDialog>();
await mainDialog.MainMenuAsync();


