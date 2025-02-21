using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IStatusTypeService
    {
        Task<IResult> CreateStatusTypeAsync(StatusTypeRegistrationForm statusRegistrationForm);
        Task<IResult> DeleteStatusTypeAsync(int id);
        Task<IResult> GetAllStatusTypesAsync();
        Task<IResult> GetStatusTypeAsync(Expression<Func<StatusTypeEntity, bool>> expression);
        Task<IResult> UpdateStatusTypeAsync(int id, StatusTypeUpdateForm updateForm);
    }
}