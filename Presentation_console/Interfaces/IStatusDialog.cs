namespace Presentation_Console.Interfaces
{
    public interface IStatusDialog
    {
        Task CreateOptionAsync();
        Task DeleteOptionAsync();
        Task GetAllOptionAsync();
        Task GetByIdAsync();
        Task MenuOptions();
        Task UpdateOptionAsync();
    }
}