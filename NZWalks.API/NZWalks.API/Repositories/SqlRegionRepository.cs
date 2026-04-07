using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SqlRegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;

        }

        public async Task<Region> DeleteAsync(Guid id)
        {
           var region= await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            nZWalksDbContext.Regions.Remove(region);
            nZWalksDbContext.SaveChangesAsync().Wait();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid Id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingregion = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (existingregion == null)
            {
                return null;
            }

            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.Area= region.Area;
            existingregion.Long = region.Long;
            existingregion.Population= region.Population;
            existingregion.lat= region.lat;

            await nZWalksDbContext.SaveChangesAsync();

            return region;

        }
    }
}
