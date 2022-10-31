using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mock.Application.Repositories;
using Mock.Domain.Abstractions;
using Mock.Domain.Commands;
using Mock.Domain.Dto;
using Mock.Domain.Entities;

namespace MockWebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = _unitOfWork.CategoryRepository.GetById(id);
        if (entity == null)
        {
            return Task.FromResult<IActionResult>(BadRequest());
        }

        return Task.FromResult<IActionResult>(Ok(entity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] UpsertCategoryCommand request)
    {
        request.Id ??= Guid.NewGuid();
        var entity = new Category()
        {
            Id = request.Id.Value,
            Name = request.Name,
            Description = request.Description
        };

        _unitOfWork.CategoryRepository.Add(entity);
        var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
        return isSuccess ? Ok(entity.Id) : BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpsertCategoryCommand request)
    {
        if (!request.Id.HasValue)
        {
            return BadRequest();
        }

        var entity = _unitOfWork.CategoryRepository.GetById(request.Id.Value);
        if (entity == null)
        {
            return BadRequest();
        }

        entity.Name = request.Name;
        entity.Description = request.Description;

        var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
        return isSuccess ? Ok(entity.Id) : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        _unitOfWork.CategoryRepository.Delete(id);

        var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;

        return isSuccess ? Ok(id) : BadRequest();
    }

    [HttpGet("search")]
    public Task<IActionResult> SearchAsync([FromQuery] SearchCategoryCommand request)
    {
        var query = _unitOfWork.CategoryRepository.GetQuery();
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var searchText = request.SearchText.ToLower();
            query = query.Where(n =>
                n.Description != null &&
                (n.Description.ToLower().Contains(searchText) || n.Name.ToLower().Contains(searchText)));
        }

        var result = new BasePaginatedResult<Category>(query, request);

        return Task.FromResult<IActionResult>(Ok(result));
    }
}