using CarRental.Application.Contracts.Dto;
using Confluent.Kafka;
using System.Text.Json;

namespace CarRental.Infrastructure.Kafka.Deserializers;

/// <summary>
/// JSON десериализатор Kafka сообщений для списка DTO записей об аренде машин
/// </summary>
public class RentalValueDeserializer : IDeserializer<IList<RentalEditDto>>
{
    /// <summary>
    /// Десериализовать список DTO записей об аренде из массива байт
    /// </summary>
    /// <param name="data">байты значения Kafka сообщения</param>
    /// <param name="isNull">Признак отсутствия значения</param>
    /// <param name="context">Контекст десериализации</param>
    /// <returns>Список DTO записей об аренде</returns>
    public IList<RentalEditDto> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull || data.IsEmpty)
            return [];

        return JsonSerializer.Deserialize<IList<RentalEditDto>>(data) ?? [];
    }
}