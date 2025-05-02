using GreenFlux.Domain.Entities;

namespace GreenFlux.Domain.Interfaces
{
    public interface IConnectorRepository : IRepository<Connector>
    {
        public Task<Connector?> Get(int id, Guid chargeStationId);
        public Task<int> Add(Connector entity);
        public Task<List<Connector>> GetAll(Guid chargeStationId);
    }
}