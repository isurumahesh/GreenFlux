using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using GreenFlux.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Infrastructure.Repositories
{
    public class ConnectorRepository : IConnectorRepository
    {
        private readonly GreenFluxDbContext _dbContext;

        public ConnectorRepository(GreenFluxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(Connector entity)
        {
            await _dbContext.Connectors.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Connector entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Connector?> Get(int id, Guid chargeStationId)
        {
            return await _dbContext.Connectors.FindAsync(id, chargeStationId);
        }

        public async Task<List<Connector>> GetAll(Guid chargeStationId)
        {
            return await _dbContext.Connectors.Where(a => a.ChargeStationId == chargeStationId).ToListAsync();
        }

        public async Task Update(Connector entity)
        {
            _dbContext.Connectors.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}