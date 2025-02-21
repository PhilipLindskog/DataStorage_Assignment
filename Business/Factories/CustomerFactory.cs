using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerRegistrationForm Create() => new();

    public static CustomerEntity Create(CustomerRegistrationForm registrationForm) => new()
    {
        CustomerName = registrationForm.CustomerName
    };

    public static Customer Create(CustomerEntity customerEntity) => new()
    {
        Id = customerEntity.Id,
        CustomerName = customerEntity.CustomerName
    };

    public static CustomerUpdateForm Create(Customer customer) => new()
    {
        Id = customer.Id,
        CustomerName = customer.CustomerName
    };

    public static CustomerEntity Update(CustomerEntity entity, CustomerUpdateForm updateForm)
    {
        entity.Id = updateForm.Id;
        entity.CustomerName = updateForm.CustomerName;

        return entity;
    }
}
