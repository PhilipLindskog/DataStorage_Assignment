namespace Presentation_Console.Interfaces
{
    public interface ICustomerDialogs
    {
        Task CreateOptionAsync();
        Task DeleteOptionAsync();
        Task GetAllOptionAsync();
        Task GetByIdAsync();
        Task MenuOptions();
        Task UpdateOptionAsync();
    }
}