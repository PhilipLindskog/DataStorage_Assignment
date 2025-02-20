using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<IResult> CreateUserAsync(UserRegistrationForm userForm);
        Task<IResult> DeleteUserAsync(int id);
        Task<IResult> GetAllUsersAsync();
        Task<IResult> GetUserAsync(Expression<Func<UserEntity, bool>> expression);
        Task<IResult> UpdateUserAsync(Expression<Func<UserEntity, bool>> expression);
    }
}