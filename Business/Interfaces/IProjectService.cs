using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<IResult> CreateProjectAsync(ProjectRegistrationForm projectForm);
        Task<IResult> DeleteProjectAsync(int id);
        Task<IResult> GetAllProjectsAsync();
        Task<IResult> GetProjectsAsync(Expression<Func<ProjectEntity, bool>> expression);
        Task<IResult> UpdateProjectAsync(int id, ProjectUpdateForm updateForm);
    }
}