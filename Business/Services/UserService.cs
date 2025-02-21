using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(UserRepository userRepository) : IUserService
{
    private readonly UserRepository _userRepository = userRepository;

    public async Task<IResult> CreateUserAsync(UserRegistrationForm userForm)
    {
        if (userForm == null)
            return Result.BadRequest("Invalid user registration");

        await _userRepository.BeginTransactionAsync();

        try
        {
            var userEntity = UserFactory.Create(userForm);

            if (await _userRepository.AlreadyExistsAsync(x => x.Email == userForm.Email))
            {
                await _userRepository.RollbackTransactionAsync();
                return Result.AlreadyExists("A User with this email already exists.");
            }

            var result = await _userRepository.CreateAsync(userEntity);

            if (result != null)
            {
                await _userRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _userRepository.RollbackTransactionAsync();
                return Result.Error("User could not be created.");
            }
        }
        catch (Exception ex)
        {
            await _userRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> GetAllUsersAsync()
    {
        var userEntities = await _userRepository.GetAllAsync();

        if (userEntities == null)
            return Result.NotFound("No users found.");

        var users = userEntities.Select(UserFactory.Create);
        return Result<IEnumerable<User>>.Ok(users);
    }

    public async Task<IResult> GetUserAsync(Expression<Func<UserEntity, bool>> expression)
    {
        var userEntity = await _userRepository.GetAsync(expression);

        if (userEntity == null)
            return Result.NotFound("User not found.");

        var user = UserFactory.Create(userEntity);
        return Result<User>.Ok(user);
    }

    public async Task<IResult> UpdateUserAsync(int id, UserUpdateForm updateForm)
    {
        await _userRepository.BeginTransactionAsync();

        try
        {
            var existingEntity = await _userRepository.GetAsync(x => x.Id == id);
            if (existingEntity == null)
            {
                await _userRepository.RollbackTransactionAsync();
                return Result.NotFound("User not found");
            }

            var updatedEntity = UserFactory.Update(existingEntity, updateForm);

            var result = await _userRepository.UpdateAsync(x => x.Id == id, updatedEntity);

            if (result)
            {
                await _userRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _userRepository.RollbackTransactionAsync();
                return Result.Error("User could not be updated.");
            }
        }
        catch (Exception ex)
        {
            await _userRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> DeleteUserAsync(int id)
    {
        var userEntity = await _userRepository.GetAsync(x => x.Id == id);

        if (userEntity == null)
            return Result.NotFound("User not found.");

        try
        {
            var result = await _userRepository.DeleteAsync(x => x.Id == id);
            return result ? Result.Ok() : Result.Error("Unable to delete user");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }
}
