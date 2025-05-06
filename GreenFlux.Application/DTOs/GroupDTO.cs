namespace GreenFlux.Application.DTOs
{
    public record GroupDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int Capacity { get; init; }
        public List<ChargeStationDTO> ChargeStations { get; init; }
    }
}