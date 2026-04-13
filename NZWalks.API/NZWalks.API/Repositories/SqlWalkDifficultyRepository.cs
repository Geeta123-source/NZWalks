using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace NZWalks.API.Repositories
{
    public class SqlWalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SqlWalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Models.Domain.WalkDifficulty> AddAsync(Models.Domain.WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<Models.Domain.WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDifficulty != null)
            {
                nZWalksDbContext.WalkDifficulty.Remove(existingWalkDifficulty);
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalkDifficulty;
            }

            return null;

        }

        public async Task<IEnumerable<Models.Domain.WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<Models.Domain.WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Models.Domain.WalkDifficulty> UpdateAsync(Guid Id, Models.Domain.WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty=await nZWalksDbContext.WalkDifficulty.FindAsync(Id);
            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code=walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
