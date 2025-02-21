using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class StatusTypeService(StatusTypeRepository statusTypeRepository) : IStatusTypeService
{
    private readonly StatusTypeRepository _statusTypeRepository = statusTypeRepository;

    public async Task<IResult> CreateStatusTypeAsync(StatusTypeRegistrationForm statusRegistrationForm)
    {
        if (statusRegistrationForm == null)
            return Result.BadRequest("Invalid status registration.");

        await _statusTypeRepository.BeginTransactionAsync();

        try
        {
            var statusEntity = StatusTypeFactory.Create(statusRegistrationForm);

            if (await _statusTypeRepository.AlreadyExistsAsync(x => x.StatusName == statusRegistrationForm.StatusName))
            {
                await _statusTypeRepository.RollbackTransactionAsync();
                return Result.AlreadyExists("An status with this name already exists.");
            }

            var result = await _statusTypeRepository.CreateAsync(statusEntity);

            if (result != null)
            {
                await _statusTypeRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _statusTypeRepository.RollbackTransactionAsync();
                return Result.Error("Status could not be created");
            }
        }
        catch (Exception ex)
        {
            await _statusTypeRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> GetAllStatusTypesAsync()
    {
        var statusEntities = await _statusTypeRepository.GetAllAsync();

        if (statusEntities == null)
            return Result.NotFound("No statuses found.");

        var statusTypes = statusEntities.Select(StatusTypeFactory.Create);
        return Result<IEnumerable<StatusType>>.Ok(statusTypes);
    }

    public async Task<IResult> GetStatusTypeAsync(Expression<Func<StatusTypeEntity, bool>> expression)
    {
        var statusEntity = await _statusTypeRepository.GetAsync(expression);

        if (statusEntity == null)
            return Result.NotFound("Status not found.");

        var statusType = StatusTypeFactory.Create(statusEntity);
        return Result<StatusType>.Ok(statusType);
    }

    public async Task<IResult> UpdateStatusTypeAsync(int id, StatusTypeUpdateForm updateForm)
    {
        await _statusTypeRepository.BeginTransactionAsync();

        try
        {
            var existingEntity = await _statusTypeRepository.GetAsync(x => x.Id == id);
            if (existingEntity == null)
            {
                await _statusTypeRepository.RollbackTransactionAsync();
                return Result.NotFound("Status not found.");
            }

            var updatedEntity = StatusTypeFactory.Update(existingEntity, updateForm);

            var result = await _statusTypeRepository.UpdateAsync(x => x.Id == id, updatedEntity);

            if (result)
            {
                await _statusTypeRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _statusTypeRepository.RollbackTransactionAsync();
                return Result.Error("Status could not be updated.");
            }
        }
        catch (Exception ex)
        {
            await _statusTypeRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> DeleteStatusTypeAsync(int id)
    {
        var statusEntity = await _statusTypeRepository.GetAsync(x => x.Id == id);

        if (statusEntity == null)
            return Result.NotFound("Status not found");

        try
        {
            var result = await _statusTypeRepository.DeleteAsync(x => x.Id == id);
            return result ? Result.Ok() : Result.Error("Unable to delete Status");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }
}
