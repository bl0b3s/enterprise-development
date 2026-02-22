using CarRental.Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace CarRental.Api.Controllers;

/// <summary>
/// Base controller providing CRUD operations for entities.
/// </summary>
/// <typeparam name="TDto">The response DTO type</typeparam>
/// <typeparam name="TCreateDto">The create DTO type</typeparam>
/// <typeparam name="TUpdateDto">The update DTO type</typeparam>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateDto, TUpdateDto>
    (ICrudService<TDto, TCreateDto, TUpdateDto> service) : ControllerBase
{

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A list of all entities</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        var entities = await service.GetAsync();
        return Ok(entities);
    }

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve</param>
    /// <returns>The entity record if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> GetById(int id)
    {
        var entity = await service.GetAsync(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    /// <summary>
    /// Creates a new entity record.
    /// </summary>
    /// <param name="createDto">The entity data to create</param>
    /// <returns>The newly created entity record</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = await service.CreateAsync(createDto);
        var id = GetIdFromDto(entity);

        return CreatedAtAction(nameof(GetById), new { id }, entity);
    }

    /// <summary>
    /// Updates an existing entity record.
    /// </summary>
    /// <param name="id">The ID of the entity to update</param>
    /// <param name="updateDto">The updated entity data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedEntity = await service.UpdateAsync(id, updateDto);
        if (updatedEntity == null)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deletes an entity record.
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Gets the ID from the DTO using reflection.
    /// </summary>
    private static int GetIdFromDto(TDto dto)
    {
        var property = dto?.GetType().GetProperty("Id")
                       ?? throw new InvalidOperationException($"DTO type {typeof(TDto).Name} does not have an 'Id' property.");

        if (property.PropertyType != typeof(int))
            throw new InvalidOperationException($"Id property in {typeof(TDto).Name} must be of type int.");

        var value = property.GetValue(dto);
        if (value is not int id)
            throw new InvalidOperationException($"Failed to retrieve Id from {typeof(TDto).Name}.");

        return id;
    }
}