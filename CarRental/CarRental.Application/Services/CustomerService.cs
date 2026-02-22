using AutoMapper;
using CarRental.Application.Dtos.Customers;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Service for managing Customer entities.
/// Provides CRUD operations for Customers using the underlying repository.
/// </summary>
/// <param name="repository">The repository for Customer data access.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
public class CustomerService(IRepository<Customer> repository, IMapper mapper)
    : BaseCrudService<Customer, CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto>(repository, mapper);