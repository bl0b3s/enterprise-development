using AutoMapper;
using CarRental.Application.Dtos.CarModels;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Service for managing CarModel entities.
/// Provides CRUD operations for CarModels using the underlying repository.
/// </summary>
/// <param name="repository">The repository for CarModel data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class CarModelService(IRepository<CarModel> repository, IMapper mapper)
    : BaseCrudService<CarModel, CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto>(repository, mapper);