using Microsoft.EntityFrameworkCore;
using NzWalks.API.Models.Domain;

namespace NzWalks.API.Data
{
  public class NzWalksDbContext:DbContext
  {
    public NzWalksDbContext(DbContextOptions<NzWalksDbContext> options):base(options)
    {

    }

    public DbSet<Region> Regions { get; set; }
    public DbSet<Walk> Walks { get; set; }
    public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
  }
}
