using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> CreateCustomerAsync(CustomerRegistrationForm registrationForm);
        Task<IResult> DeleteCustomerAsync(int id);
        Task<IResult> GetAllCustomersAsync();
        Task<IResult> GetCustomerAsync(Expression<Func<CustomerEntity, bool>> expression);
        Task<IResult> UpdateCustomerAsync(int id, CustomerUpdateForm updateForm);
    }
}