using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Application.Dtos.Rentals;
using CarRental.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for managing rentals in the car rental system.
/// </summary>
public class RentalsController
    (
        ICrudService<RentalResponseDto, RentalCreateDto, RentalUpdateDto> service,
        ICrudService<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto> customerService,
        ICrudService<CarResponseDto, CarCreateDto, CarUpdateDto> carService
    )
    : CrudControllerBase<RentalResponseDto, RentalCreateDto, RentalUpdateDto>(service)
{
    /// <summary>
    /// Creates a new rental agreement.
    /// </summary>
    /// <param name="createDto">The rental data to create</param>
    /// <returns>The newly created rental record</returns>
    /// <response code="201">Returns the newly created rental</response>
    /// <response code="400">If the request data is invalid or customer/car does not exist</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<ActionResult<RentalResponseDto>> Create([FromBody] RentalCreateDto createDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customer = await customerService.GetAsync(createDto.CustomerId);
            if (customer == null)
            {
                return BadRequest(new { error = $"Customer with ID {createDto.CustomerId} does not exist" });
            }

            var car = await carService.GetAsync(createDto.CarId);
            if (car == null)
            {
                return BadRequest(new { error = $"Car with ID {createDto.CarId} does not exist" });
            }

            var entity = await service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Updates an existing rental agreement.
    /// </summary>
    /// <param name="id">The ID of the rental to update</param>
    /// <param name="updateDto">The updated rental data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update was successful</response>
    /// <response code="400">If the request data is invalid or customer/car does not exist</response>
    /// <response code="404">If the rental with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    public override async Task<IActionResult> Update(int id, [FromBody] RentalUpdateDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (updateDto.CustomerId != default)
            {
                var customer = await customerService.GetAsync(updateDto.CustomerId);
                if (customer == null)
                {
                    return BadRequest(new { error = $"Customer with ID {updateDto.CustomerId} does not exist" });
                }
            }

            if (updateDto.CarId != default)
            {
                var car = await carService.GetAsync(updateDto.CarId);
                if (car == null)
                {
                    return BadRequest(new { error = $"Car with ID {updateDto.CarId} does not exist" });
                }
            }

            var updatedEntity = await service.UpdateAsync(id, updateDto);
            if (updatedEntity == null) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }
}