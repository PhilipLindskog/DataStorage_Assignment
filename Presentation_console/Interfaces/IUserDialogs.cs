namespace Presentation_Console.Interfaces
{
    public interface IUserDialogs
    {
        Task CreateOptionAsync();
        Task DeleteOptionAsync();
        Task GetAllOptionAsync();
        Task GetByIdAsync();
        Task MenuOptions();
        Task UpdateOptionAsync();
    }
}