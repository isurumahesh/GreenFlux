namespace GreenFlux.Application.DTOs
{
    public record ConnectorCreateDTO
    {
        public int MaxCurrent { get; init; }
        public int Id { get; init; }
    }
}