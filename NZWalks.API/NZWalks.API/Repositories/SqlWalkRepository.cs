using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SqlWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id=Guid.NewGuid();

            await nZWalksDbContext.Walk.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walk.FindAsync(id);
            if (existingWalk != null)
            {
                return null;
            }

            nZWalksDbContext.Walk.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await
                nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walk.FindAsync(id);

            if (existingWalk != null)
            {
                //existingWalk.Id=walk.Id;
                existingWalk.Length=walk.Length;
                existingWalk.Name=walk.Name;
                existingWalk.RegionId=walk.RegionId;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await nZWalksDbContext.SaveChangesAsync();

                return existingWalk;
            }

            return null;
        }
    }
}
