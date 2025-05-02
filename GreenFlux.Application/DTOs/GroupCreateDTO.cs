namespace GreenFlux.Application.DTOs
{
    public record GroupCreateDTO
    {
        public string Name { get; init; }
        public int Capacity { get; init; }
        public ChargeStationCreateDTO? ChargeStation { get; init; }
    }
}