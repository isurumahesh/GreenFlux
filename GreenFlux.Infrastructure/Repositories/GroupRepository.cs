using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using GreenFlux.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly GreenFluxDbContext _dbContext;

        public GroupRepository(GreenFluxDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Guid> Add(Group entity)
        {
            await _dbContext.Groups.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Group entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Group?> Get(Guid id)
        {
            return await _dbContext.Groups.FindAsync(id);
        }

        public async Task<Group?> GetGroupWithChargeStations(Guid id)
        {
            return await _dbContext.Groups.Include(a => a.ChargeStations)
                .ThenInclude(a => a.Connectors)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Group>> GetAll()
        {
            return await _dbContext.Groups
                .AsSplitQuery()
                .Include(a => a.ChargeStations)
                .ThenInclude(a => a.Connectors)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(Group entity)
        {
            _dbContext.Groups.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}