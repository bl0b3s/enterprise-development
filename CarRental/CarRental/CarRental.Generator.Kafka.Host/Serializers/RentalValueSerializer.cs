using CarRental.Application.Contracts.Dto;
using Confluent.Kafka;
using System.Text.Json;

namespace CarRental.Generator.Kafka.Host.Serializers;

/// <summary>
/// JSON сериализатор Kafka значения для списка DTO записей об аренде
/// </summary>
public class RentalValueSerializer : ISerializer<IList<RentalEditDto>>
{
    /// <summary>
    /// Сериализовать список DTO записей об аренде в JSON массив байт
    /// </summary>
    /// <param name="data">Список DTO записей об аренде</param>
    /// <param name="context">Контекст сериализации</param>
    /// <returns>JSON в UTF-8</returns>
    public byte[] Serialize(IList<RentalEditDto> data, SerializationContext context)
        => JsonSerializer.SerializeToUtf8Bytes(data);
}