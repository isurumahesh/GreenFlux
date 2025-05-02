using GreenFlux.Domain.Entities;

namespace GreenFlux.Domain.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        public Task<Group?> Get(Guid id);
        public Task<Group?> GetGroupWithChargeStations(Guid id);
        public Task<Guid> Add(Group entity);
        public Task<List<Group>> GetAll();
    }
}