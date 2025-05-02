using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using GreenFlux.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Infrastructure.Repositories
{
    public class ChargeStationRepository : IChargeStationRepository
    {
        private readonly GreenFluxDbContext _dbContext;
        public ChargeStationRepository(GreenFluxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(ChargeStation entity)
        {
            await _dbContext.ChargeStations.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(ChargeStation entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChargeStation?> Get(Guid groupId, Guid id)
        {
            return await _dbContext.ChargeStations.SingleOrDefaultAsync(a => a.GroupId == groupId && a.Id == id);
        }

        public async Task<ChargeStation?> Get(Guid id)
        {
            return await _dbContext.ChargeStations.Include(a => a.Connectors).SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ChargeStation>> GetAll(Guid groupId)
        {
            return await _dbContext.ChargeStations.Include(a => a.Connectors).Where(a => a.GroupId == groupId).ToListAsync();
        }

        public async Task Update(ChargeStation entity)
        {
            _dbContext.ChargeStations.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}