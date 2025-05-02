namespace GreenFlux.Application.DTOs
{
    public record ConnectorDTO
    {
        public int Id { get; init; }
        public int MaxCurrent { get; init; }
    }
}