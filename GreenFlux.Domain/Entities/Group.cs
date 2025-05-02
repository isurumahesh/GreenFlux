namespace GreenFlux.Domain.Entities
{
    public class Group : AuditEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<ChargeStation> ChargeStations { get; set; } = new();
    }
}