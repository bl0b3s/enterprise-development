using CarRental.Application.Dtos.CarModels;
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for managing model generations in the car rental system.
/// </summary>
public class ModelGenerationController(
    ICrudService<ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto> service,
    ICrudService<CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto> carModelService
) : CrudControllerBase<ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto>(service)
{
    public override async Task<ActionResult<ModelGenerationResponseDto>> Create([FromBody] ModelGenerationCreateDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var carModel = await carModelService.GetAsync(createDto.CarModelId);
        if (carModel == null)
            return BadRequest(new { error = $"Car model with ID {createDto.CarModelId} does not exist" });

        var entity = await service.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    public override async Task<IActionResult> Update(int id, [FromBody] ModelGenerationUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var carModel = await carModelService.GetAsync(updateDto.CarModelId);
        if (carModel == null)
            return BadRequest(new { error = $"Car model with ID {updateDto.CarModelId} does not exist" });

        var updated = await service.UpdateAsync(id, updateDto);
        if (updated == null) return NotFound();

        return NoContent();
    }
}