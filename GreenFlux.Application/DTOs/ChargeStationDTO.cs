namespace GreenFlux.Application.DTOs
{
    public record ChargeStationDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
        public List<ConnectorDTO> Connectors { get; init; }
    }
}