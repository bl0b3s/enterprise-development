using Bogus;
using CarRental.Application.Contracts.Dto;

namespace CarRental.Generator.Kafka.Host.Generator;

/// <summary>
/// Генератор тестовых DTO записей об аренде для отправки в Kafka
/// </summary>
public class RentalGenerator
{
    /// <summary>
    /// Сгенерировать список DTO для создания или обновления записей об аренде
    /// </summary>
    /// <param name="count">Количество генерируемых DTO</param>
    /// <returns>Список DTO для создания или обновления записей об аренде</returns>
    public static IList<RentalEditDto> Generate(int count) =>
        new Faker<RentalEditDto>()
            .CustomInstantiator(f => new RentalEditDto(
                RentalDate: f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(1)),
                RentalHours: f.Random.Int(1, 24),
                CarId: f.Random.Int(1, 15),
                ClientId: f.Random.Int(1, 15)
            ))
            .Generate(count);
}