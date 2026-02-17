using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Контроллер для управления клиентами
/// </summary>
[ApiController]
[Route("api/clients")]
public class ClientsController(
    IRepository<Client> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получает всех клиентов
    /// </summary>
    /// <returns>Список всех клиентов</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientGetDto>>> GetAll()
    {
        var entities = await repo.GetAllAsync();
        var dtos = mapper.Map<IEnumerable<ClientGetDto>>(entities);
        return Ok(dtos);
    }

    /// <summary>
    /// Получает клиента по ID
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <returns>Клиент</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientGetDto>> Get(int id)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        var dto = mapper.Map<ClientGetDto>(entity);
        return Ok(dto);
    }

    /// <summary>
    /// Создает нового клиента
    /// </summary>
    /// <param name="dto">Данные для создания клиента</param>
    /// <returns>Созданный клиент</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ClientGetDto>> Create([FromBody] ClientEditDto dto)
    {
        var entity = mapper.Map<Client>(dto);
        var created = await repo.AddAsync(entity);
        var resultDto = mapper.Map<ClientGetDto>(created);
        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Обновляет существующий клиент
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    /// <param name="dto">Обновленные данные о клиентах</param>
    /// <returns>Обновленный клиент</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientGetDto>> Update(int id, [FromBody] ClientEditDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity == null) return NotFound();
        mapper.Map(dto, entity);
        await repo.UpdateAsync(entity);
        var resultDto = mapper.Map<ClientGetDto>(entity);
        return Ok(resultDto);
    }

    /// <summary>
    /// Удаляет клиента
    /// </summary>
    /// <param name="id">Идентификатор клиента</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.DeleteAsync(id);
        return NoContent();
    }
}