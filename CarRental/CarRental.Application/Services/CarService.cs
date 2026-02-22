using AutoMapper;
using CarRental.Application.Dtos.Cars;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Service for managing Car entities.
/// Provides CRUD operations for Cars using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Car data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class CarService(IRepository<Car> repository, IMapper mapper)
    : BaseCrudService<Car, CarResponseDto, CarCreateDto, CarUpdateDto>(repository, mapper);
