using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(ProjectRepository projectRepository) : IProjectService
{
    private readonly ProjectRepository _projectRepository = projectRepository;

    public async Task<IResult> CreateProjectAsync(ProjectRegistrationForm projectForm)
    {
        if (projectForm == null)
            return Result.BadRequest("Invalid project registration.");

        await _projectRepository.BeginTransactionAsync();

        try
        {
            var projectEntity = ProjectFactory.Create(projectForm);

            if (await _projectRepository.AlreadyExistsAsync(x => x.Title == projectForm.Title))
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.AlreadyExists("A Project with this title already exists.");
            }

            var result = await _projectRepository.CreateAsync(projectEntity);

            if (result != null)
            {
                await _projectRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.Error("Project could not be created.");
            }
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> GetAllProjectsAsync()
    {
        var projectEntities = await _projectRepository.GetAllAsync();

        if (projectEntities == null)
            return Result.NotFound("No projects found.");

        var projects = projectEntities.Select(ProjectFactory.Create);
        return Result<IEnumerable<Project>>.Ok(projects);
    }

    public async Task<IResult> GetProjectsAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        var projectEntity = await _projectRepository.GetAsync(expression);

        if (projectEntity == null)
            return Result.NotFound("Project not found.");

        var project = ProjectFactory.Create(projectEntity);
        return Result<Project>.Ok(project);
    }

    public async Task<IResult> UpdateProjectAsync(int id, ProjectUpdateForm updateForm)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            var existingEntity = await _projectRepository.GetAsync(x => x.Id == id);
            if (existingEntity == null)
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.NotFound("Project not found.");
            }

            var updatedEntity = ProjectFactory.Update(existingEntity, updateForm);

            var result = await _projectRepository.UpdateAsync(x => x.Id == id, updatedEntity);

            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
                return Result.Ok();
            }
            else
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.Error("Project could not be updated.");
            }
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }

    public async Task<IResult> DeleteProjectAsync(int id)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

        if (projectEntity == null)
            return Result.NotFound("Project not found.");

        try
        {
            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            return result ? Result.Ok() : Result.Error("Unable to delete project");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error(ex.Message);
        }
    }
}
