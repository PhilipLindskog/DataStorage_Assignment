using Business.Dtos;
using Business.Factories;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Interfaces;

public class CustomerService(CustomerRepository customerRepository) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;

    public async Task<IResult> CreateCustomerAsync(CustomerRegistrationForm registrationForm)
    {
        if (registrationForm == null)
            return Result.BadRequest("Invalid customer registration.");

        await _customerRepository.BeginTransactionAsync();

        try
        {
            var customerEntity = CustomerFactory.Create(registrationForm);

            if (await _customerRepository.AlreadyExistsAsync(x => x.CustomerName == registrationForm.CustomerName))
            {
                await _customerRepository.RollbackTransactionAsync();
                return Result.AlreadyExists("A Customer with this name already exists");
            }

            var result = await _customerRepository.CreateAsync(customerEntity);

            if (result != null)
            {
                await _customerRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _customerRepository.RollbackTransactionAsync();
                return Result.Error("Customer could not be created");
            }
        }
        catch (Exception ex)
        {
            await _customerRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> GetAllCustomersAsync()
    {
        var customerEntities = await _customerRepository.GetAllAsync();

        if (customerEntities == null)
            return Result.NotFound("No customers found.");

        var customers = customerEntities.Select(CustomerFactory.Create);
        return Result<IEnumerable<Customer>>.Ok(customers);
    }

    public async Task<IResult> GetCustomerAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        var customerEntity = await _customerRepository.GetAsync(expression);

        if (customerEntity == null)
            return Result.NotFound("Customer not found.");

        var customer = CustomerFactory.Create(customerEntity);
        return Result<Customer>.Ok(customer);
    }

    public async Task<IResult> UpdateCustomerAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        await _customerRepository.BeginTransactionAsync();

        try
        {
            var oldCustomerEntity = await _customerRepository.GetAsync(expression);
            if (oldCustomerEntity == null)
            {
                await _customerRepository.RollbackTransactionAsync();
                return Result.NotFound("Customer not found.");
            }

            var customerEntity = CustomerFactory.Create(oldCustomerEntity);

            if (customerEntity != null)
            {
                await _customerRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _customerRepository.RollbackTransactionAsync();
                return Result.Error("Customer could not be updated.");
            }
        }
        catch (Exception ex)
        {
            await _customerRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> DeleteCustomerAsync(int id)
    {
        var customerEntity = await _customerRepository.GetAsync(x => x.Id == id);

        if (customerEntity == null)
            return Result.NotFound("Customer not found.");

        try
        {
            var result = await _customerRepository.DeleteAsync(x => x.Id == id);
            return result ? Result.Ok() : Result.Error("Unable to delete customer.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }
}
