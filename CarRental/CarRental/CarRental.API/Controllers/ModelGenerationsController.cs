using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers;

/// <summary>
/// Контроллер для управления поколениями моделей
/// </summary>
[ApiController]
[Route("api/model-generations")]
public class ModelGenerationsController(
    IRepository<ModelGeneration> repo,
    IRepository<CarModel> carModelRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получает доступ ко всем поколениям моделей
    /// </summary>
    /// <returns>Список всех поколений моделей</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ModelGenerationGetDto>>> GetAll()
    {
        var entities = await repo.GetAllAsync(
            include: query => query.Include(mg => mg.Model));
        var dtos = mapper.Map<IEnumerable<ModelGenerationGetDto>>(entities);
        return Ok(dtos);
    }

    /// <summary>
    /// Получает поколение модели по ID
    /// </summary>
    /// <param name="id">Идентификатор поколения</param>
    /// <returns>Генерация модели</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ModelGenerationGetDto>> Get(int id)
    {
        var entity = await repo.GetByIdAsync(id,
            include: query => query.Include(mg => mg.Model));
        if (entity == null) return NotFound();
        var dto = mapper.Map<ModelGenerationGetDto>(entity);
        return Ok(dto);
    }

    /// <summary>
    /// Создает новое поколение моделей
    /// </summary>
    /// <param name="dto">Данные для создания генерации</param>
    /// <returns>Созданная генерация модели</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ModelGenerationGetDto>> Create([FromBody] ModelGenerationEditDto dto)
    {
        var carModel = await carModelRepo.GetByIdAsync(dto.ModelId);
        if (carModel == null)
            return BadRequest($"Car model with Id {dto.ModelId} does not exist.");

        var entity = mapper.Map<ModelGeneration>(dto);
        var created = await repo.AddAsync(entity);

        // Загружаем связанные данные для DTO
        var generationWithIncludes = await repo.GetByIdAsync(created.Id,
            include: query => query.Include(mg => mg.Model));
        var resultDto = mapper.Map<ModelGenerationGetDto>(generationWithIncludes);

        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет существующую генерацию моделей
    /// </summary>
    /// <param name="id">Идентификатор поколения</param>
    /// <param name="dto">Обновленные данные о поколении</param>
    /// <returns>Обновленное поколение моделей</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ModelGenerationGetDto>> Update(int id, [FromBody] ModelGenerationEditDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        var carModel = await carModelRepo.GetByIdAsync(dto.ModelId);
        if (carModel == null)
            return BadRequest($"Car model with Id {dto.ModelId} does not exist.");

        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);

        // Загружаем связанные данные для DTO
        var updatedWithIncludes = await repo.GetByIdAsync(entity.Id,
            include: query => query.Include(mg => mg.Model));
        var resultDto = mapper.Map<ModelGenerationGetDto>(updatedWithIncludes);

        return Ok(resultDto);
    }

    /// <summary>
    /// Удаляет поколение модели
    /// </summary>
    /// <param name="id">Идентификатор поколения</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.DeleteAsync(id);
        return NoContent();
    }
}