namespace Presentation_Console.Interfaces
{
    public interface IProjectDialogs
    {
        Task CreateOptionAsync();
        Task DeleteOptionAsync();
        Task GetAllOptionAsync();
        Task GetByIdAsync();
        Task<(string customerName, string statusName, string userName, string userEmail, string productName, decimal price)> GetDetailsAsync(int customerId, int statusId, int userId, int productId);
        Task MenuOptions();
        Task UpdateOptionAsync();
    }
}