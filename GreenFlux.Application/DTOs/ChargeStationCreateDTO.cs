namespace GreenFlux.Application.DTOs
{
    public record ChargeStationCreateDTO
    {
        public string Name { get; init; }
        public List<ConnectorCreateDTO> Connectors { get; init; } = new();
    }
}