namespace GreenFlux.Domain.Entities
{
    public class Group : AuditEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<ChargeStation> ChargeStations { get; set; } = new();

        public int GetCurrentOfAllConnectors()
        {
            try
            {
                var totalAmps = 0;

                if (!ChargeStations.Any()) return totalAmps;

                foreach (var item in ChargeStations)
                {
                    totalAmps = totalAmps + item.Connectors.Sum(a => a.MaxCurrent);
                }
                return totalAmps;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}