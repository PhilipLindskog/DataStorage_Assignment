using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class StatusTypeFactory
{
    public static StatusTypeRegistrationForm Create() => new();

    public static StatusTypeEntity Create(StatusTypeRegistrationForm form) => new()
    {
        StatusName = form.StatusName
    };

    public static StatusType Create(StatusTypeEntity entity) => new()
    {
        Id = entity.Id,
        StatusName = entity.StatusName
    };

    public static StatusTypeUpdateForm Create(StatusType status) => new()
    {
        Id = status.Id,
        StatusName = status.StatusName
    };

    public static StatusTypeEntity Update(StatusTypeEntity entity, StatusTypeUpdateForm form)
    {
        entity.Id = form.Id;
        entity.StatusName = form.StatusName;

        return entity;
    }


}
