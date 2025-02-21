using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserRegistrationForm Create() => new();

    public static UserEntity Create(UserRegistrationForm userForm) => new()
    {
        FirstName = userForm.FirstName,
        LastName = userForm.LastName,
        Email = userForm.Email
    };

    public static User Create(UserEntity userEntity) => new()
    {
        Id = userEntity.Id,
        FirstName = userEntity.FirstName,
        LastName = userEntity.LastName,
        Email = userEntity.Email
    };

    public static UserUpdateForm Create(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email
    };

    public static UserEntity Update(UserEntity entity, UserUpdateForm form)
    {
        entity.Id = form.Id;
        entity.FirstName = form.FirstName;
        entity.LastName = form.LastName;
        entity.Email = form.Email;

        return entity;
    }
}
