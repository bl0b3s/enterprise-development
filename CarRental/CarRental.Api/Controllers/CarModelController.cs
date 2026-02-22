using CarRental.Application.Dtos.CarModels;
using CarRental.Application.Services;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for managing car models in the car rental system.
/// </summary>
public class CarModelController(
    ICrudService<CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto> service
) : CrudControllerBase<CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto>(service);