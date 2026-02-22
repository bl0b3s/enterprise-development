using AutoMapper;
using CarRental.Domain.Models;
using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Application.Dtos.CarModels;
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Application.Dtos.Rentals;

namespace CarRental.Application.Profiles;

/// <summary>
/// AutoMapper profile configuration for mapping between domain models and DTOs.
/// Defines all object-to-object mappings used in the application.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MappingProfile class and configures all mappings.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Car, CarResponseDto>();
        CreateMap<Customer, CustomerResponseDto>();
        CreateMap<CarModel, CarModelResponseDto>();
        CreateMap<ModelGeneration, ModelGenerationResponseDto>();
        CreateMap<Rental, RentalResponseDto>();

        CreateMap<CarCreateDto, Car>();
        CreateMap<CustomerCreateDto, Customer>()
            .ForMember(dest => dest.DriverLicenseNumber,
                       opt => opt.MapFrom(src => NormalizeDriverLicense(src.DriverLicenseNumber)));
        CreateMap<CarModelCreateDto, CarModel>();
        CreateMap<ModelGenerationCreateDto, ModelGeneration>();
        CreateMap<RentalCreateDto, Rental>();

        CreateMap<CarUpdateDto, Car>();
        CreateMap<CustomerUpdateDto, Customer>()
            .ForMember(dest => dest.DriverLicenseNumber,
                       opt => opt.MapFrom(src => NormalizeDriverLicense(src.DriverLicenseNumber)));
        CreateMap<CarModelUpdateDto, CarModel>();
        CreateMap<ModelGenerationUpdateDto, ModelGeneration>();
        CreateMap<RentalUpdateDto, Rental>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }

    private static string? NormalizeDriverLicense(string? license)
    {
        if (string.IsNullOrWhiteSpace(license)) return null;

        var cleaned = new string(license.Where(c => char.IsLetterOrDigit(c)).ToArray());

        return cleaned.ToUpperInvariant();
    }
}