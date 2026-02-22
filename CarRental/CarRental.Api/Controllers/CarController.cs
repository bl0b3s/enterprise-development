using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for managing cars in the car rental system.
/// </summary>
public class CarsController(
    ICrudService<CarResponseDto, CarCreateDto, CarUpdateDto> service,
    ICrudService<ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto> modelGenerationService
) : CrudControllerBase<CarResponseDto, CarCreateDto, CarUpdateDto>(service)
{
    public override async Task<ActionResult<CarResponseDto>> Create([FromBody] CarCreateDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var generation = await modelGenerationService.GetAsync(createDto.ModelGenerationId);
        if (generation == null)
            return BadRequest(new { error = $"Model generation with ID {createDto.ModelGenerationId} does not exist" });

        var entity = await service.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    public override async Task<IActionResult> Update(int id, [FromBody] CarUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var generation = await modelGenerationService.GetAsync(updateDto.ModelGenerationId);
        if (generation == null)
            return BadRequest(new { error = $"Model generation with ID {updateDto.ModelGenerationId} does not exist" });

        var updated = await service.UpdateAsync(id, updateDto);
        if (updated == null) return NotFound();

        return NoContent();
    }
}