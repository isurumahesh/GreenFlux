namespace GreenFlux.Domain.Entities
{
    public class ChargeStation : AuditEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public List<Connector> Connectors { get; set; }
    }
}