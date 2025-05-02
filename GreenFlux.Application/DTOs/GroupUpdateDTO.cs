namespace GreenFlux.Application.DTOs
{
    public record GroupUpdateDTO
    {
        public string Name { get; init; }
        public int Capacity { get; init; }
    }
}