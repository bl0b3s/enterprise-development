using AutoMapper;
using CarRental.Application.Dtos.Rentals;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Service for managing Rental entities.
/// Provides CRUD operations for Rentals using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Rental data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class RentalService(IRepository<Rental> repository, IMapper mapper)
    : BaseCrudService<Rental, RentalResponseDto, RentalCreateDto, RentalUpdateDto>(repository, mapper);
