using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Контроллер для управления моделями автомобилей
/// </summary>
[ApiController]
[Route("api/car-models")]
public class CarModelsController(
    IRepository<CarModel> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получает доступ ко всем моделям автомобилей
    /// </summary>
    /// <returns>Список всех моделей автомобилей</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarModelGetDto>>> GetAll()
    {
        var entities = await repo.GetAllAsync();
        var dtos = mapper.Map<IEnumerable<CarModelGetDto>>(entities);
        return Ok(dtos);
    }

    /// <summary>
    /// Получает модель автомобиля по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <returns>Модель автомобиля</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelGetDto>> Get(int id)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        var dto = mapper.Map<CarModelGetDto>(entity);
        return Ok(dto);
    }

    /// <summary>
    /// Создает новую модель автомобиля
    /// </summary>
    /// <param name="dto">Данные для создания модели</param>
    /// <returns>Созданная модель автомобиля</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CarModelGetDto>> Create([FromBody] CarModelEditDto dto)
    {
        var entity = mapper.Map<CarModel>(dto);
        var created = await repo.AddAsync(entity);
        var resultDto = mapper.Map<CarModelGetDto>(created);
        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет существующую модель автомобиля
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="dto">Обновленные данные модели</param>
    /// <returns>Обновленная модель автомобиля</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelGetDto>> Update(int id, [FromBody] CarModelEditDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);
        var resultDto = mapper.Map<CarModelGetDto>(entity);
        return Ok(resultDto);
    }

    /// <summary>
    /// Удалить модель автомобиля
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.DeleteAsync(id);
        return NoContent();
    }
}