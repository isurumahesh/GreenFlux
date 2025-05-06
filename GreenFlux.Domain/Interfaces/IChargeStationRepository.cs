using GreenFlux.Domain.Entities;

namespace GreenFlux.Domain.Interfaces
{
    public interface IChargeStationRepository
    {
        public Task<ChargeStation?> Get(Guid groupdId, Guid id);

        public Task<ChargeStation?> Get(Guid id);

        public Task<List<ChargeStation>> GetAll(Guid groupdId);

        public Task Delete(ChargeStation chargeStation);

        public Task Update(ChargeStation chargeStation);

        public Task<Guid> Add(ChargeStation entity);
    }
}