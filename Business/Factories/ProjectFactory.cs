using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectRegistrationForm Create() => new();

    public static ProjectEntity Create(ProjectRegistrationForm form) => new()
    {
        Title = form.Title,
        Description = form.Description,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        CustomerId = form.CustomerId,
        StatusId = form.StatusId,
        UserId = form.UserId,
        ProductId = form.ProductId,
    };

    public static Project Create(ProjectEntity entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        CustomerId = entity.CustomerId,
        StatusId = entity.StatusId,
        UserId = entity.UserId,
        ProductId = entity.ProductId,
        CustomerName = entity.Customer.CustomerName,
        StatusName = entity.Status.StatusName,
        UserName = entity.User.FirstName,
        ProductName = entity.Product.ProductName
    };

    public static ProjectUpdateForm Create(Project project) => new()
    {
        Id = project.Id,
        Title = project.Title,
        Description = project.Description,
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        CustomerId = project.CustomerId,
        StatusId = project.StatusId,
        UserId = project.UserId,
        ProductId = project.ProductId
    };

    public static ProjectEntity Update(ProjectEntity entity, ProjectUpdateForm form)
    {
        entity.Id = form.Id;
        entity.Title = form.Title;
        entity.Description = form.Description;
        entity.StartDate = form.StartDate;
        entity.EndDate = form.EndDate;
        entity.CustomerId = form.CustomerId;
        entity.StatusId = form.StatusId;
        entity.UserId = form.UserId;
        entity.ProductId = form.ProductId;

        return entity;
    }
}
