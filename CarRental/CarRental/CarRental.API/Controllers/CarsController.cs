using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers;

/// <summary>
/// Контроллер для управления автомобилями
/// </summary>
[ApiController]
[Route("api/cars")]
public class CarsController(
    IRepository<Car> repo,
    IRepository<ModelGeneration> modelGenerationRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получает все машины
    /// </summary>
    /// <returns>Список всех автомобилей</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarGetDto>>> GetAll()
    {
        var entities = await repo.GetAllAsync(
            include: query => query
                .Include(c => c.ModelGeneration)
                    .ThenInclude(mg => mg.Model));
        var dtos = mapper.Map<IEnumerable<CarGetDto>>(entities);
        return Ok(dtos);
    }

    /// <summary>
    /// Получает машину по ID
    /// </summary>
    /// <param name="id">Идентификатор автомобиля</param>
    /// <returns>Машина</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarGetDto>> Get(int id)
    {
        var entity = await repo.GetByIdAsync(id,
            include: query => query
                .Include(c => c.ModelGeneration)
                    .ThenInclude(mg => mg.Model));
        if (entity == null) return NotFound();
        var dto = mapper.Map<CarGetDto>(entity);
        return Ok(dto);
    }

    /// <summary>
    /// Создает новый автомобиль
    /// </summary>
    /// <param name="dto">Данные о создании автомобиля</param>
    /// <returns>Созданный автомобиль</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarGetDto>> Create([FromBody] CarEditDto dto)
    {
        var modelGeneration = await modelGenerationRepo.GetByIdAsync(dto.ModelGenerationId);
        if (modelGeneration == null)
            return BadRequest($"Model generation with Id {dto.ModelGenerationId} does not exist.");

        var entity = mapper.Map<Car>(dto);
        var created = await repo.AddAsync(entity);

        // Загружаем связанные данные для DTO
        var carWithIncludes = await repo.GetByIdAsync(created.Id,
            include: query => query
                .Include(c => c.ModelGeneration)
                    .ThenInclude(mg => mg.Model));
        var resultDto = mapper.Map<CarGetDto>(carWithIncludes);

        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет существующий автомобиль
    /// </summary>
    /// <param name="id">Идентификатор автомобиля</param>
    /// <param name="dto">Обновленные данные об автомобиле</param>
    /// <returns>Обновленный автомобиль</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarGetDto>> Update(int id, [FromBody] CarEditDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        var modelGeneration = await modelGenerationRepo.GetByIdAsync(dto.ModelGenerationId);
        if (modelGeneration == null)
            return BadRequest($"Model generation with Id {dto.ModelGenerationId} does not exist.");

        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);

        var updatedWithIncludes = await repo.GetByIdAsync(entity.Id,
            include: query => query
                .Include(c => c.ModelGeneration)
                    .ThenInclude(mg => mg.Model));
        var resultDto = mapper.Map<CarGetDto>(updatedWithIncludes);

        return Ok(resultDto);
    }

    /// <summary>
    /// Удаляет автомобиль
    /// </summary>
    /// <param name="id">Идентификатор автомобиля</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.DeleteAsync(id);
        return NoContent();
    }
}