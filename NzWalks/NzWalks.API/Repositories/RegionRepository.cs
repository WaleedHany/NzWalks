using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using NzWalks.API.Models.Domain;

namespace NzWalks.API.Repositories
{
  public class RegionRepository : IRegionRepository
  {
    private readonly NzWalksDbContext _nzWalksDbContext;
    public RegionRepository(NzWalksDbContext nzWalksDbContext)
    {
      this._nzWalksDbContext = nzWalksDbContext;
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
      return await _nzWalksDbContext.Regions.ToListAsync();
    }

    public async Task<Region> GetAsync(Guid id)
    {
      return await _nzWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> AddAsync(Region region)
    {
      region.Id = Guid.NewGuid();
      await _nzWalksDbContext.AddAsync(region);
      await _nzWalksDbContext.SaveChangesAsync();
      return region;
    }

    public async Task<Region> DeleteAsync(Guid id)
    {
      var region = await _nzWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
      if (region == null) return null;
      // Delete region from data base
      _nzWalksDbContext.Regions.Remove(region);
      await _nzWalksDbContext.SaveChangesAsync();
      return region;
    }

    public async Task<Region> UpdateAsync(Guid id, Region region)
    {
      var existingRegion = await _nzWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
      if (existingRegion == null) return null;

      existingRegion.Code = region.Code;
      existingRegion.Name = region.Name;
      existingRegion.Area = region.Area;
      existingRegion.Latitude = region.Latitude;
      existingRegion.Longitude = region.Longitude;
      existingRegion.Population = region.Population;

      await _nzWalksDbContext.SaveChangesAsync();
      return existingRegion;
    }
  }
}
