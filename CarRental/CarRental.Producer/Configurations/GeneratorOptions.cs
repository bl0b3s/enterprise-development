namespace CarRental.Producer.Configurations;

public class GeneratorOptions
{
    public int BatchSize { get; set; } = 5;
    public int PayloadLimit { get; set; } = 20;
    public int WaitTime { get; set; } = 3;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 5;
    public int GrpcTimeoutSeconds { get; set; } = 30;
    public DataOptions Data { get; set; } = new();
}

public class DataOptions
{
    public RangeOptions CustomerIdRange { get; set; } = new(1, 7);
    public RangeOptions CarIdRange { get; set; } = new(1, 6);
    public RangeOptions HoursRange { get; set; } = new(2, 168);
}

public record RangeOptions(int Min, int Max);