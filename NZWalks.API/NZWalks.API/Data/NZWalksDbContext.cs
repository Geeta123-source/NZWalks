using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using System.Data;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext :DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
                
        }

        public DbSet<Region> Regions {  get; set; }

        public DbSet<Walk> Walk { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

    }
}
