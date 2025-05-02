namespace GreenFlux.Application.DTOs
{
    public record GroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
        public int Capacity { get; init; }
    }
}