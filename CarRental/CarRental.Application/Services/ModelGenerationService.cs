using AutoMapper;
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Service for managing ModelGeneration entities.
/// Provides CRUD operations for ModelGenerations using the underlying repository.
/// </summary>
/// <param name="repository">The repository for ModelGeneration data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class ModelGenerationService(IRepository<ModelGeneration> repository, IMapper mapper)
    : BaseCrudService<ModelGeneration, ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto>(repository, mapper);
