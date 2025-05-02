namespace GreenFlux.Domain.Entities
{
    public class Connector : AuditEntity
    {
        public int Id { get; set; }
        public int MaxCurrent { get; set; }
        public Guid ChargeStationId { get; set; }
        public ChargeStation ChargeStation { get; set; }
    }
}