using CarRental.Application.Dtos.Customers;
using CarRental.Application.Services;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for managing customers in the car rental system.
/// </summary>
public class CustomerController(
    ICrudService<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto> service
) : CrudControllerBase<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto>(service);