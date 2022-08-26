using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using NzWalks.API.Models.Domain;

namespace NzWalks.API.Repositories
{
  public class WalkRepository : IWalkRepository
  {
    private readonly NzWalksDbContext _nzWalksDbContext;
    public WalkRepository(NzWalksDbContext nzWalksDbContext)
    {
      this._nzWalksDbContext = nzWalksDbContext;
    }


    public async Task<Walk> AddAsync(Walk walk)
    {
      walk.Id = Guid.NewGuid();
      await _nzWalksDbContext.AddAsync(walk);
      await _nzWalksDbContext.SaveChangesAsync();
      return walk;
    }

    public async Task<Walk> DeleteAsync(Guid id)
    {
      var walk = await _nzWalksDbContext.Walks.FindAsync(id);
      if (walk == null) return null;
      // Delete region from data base
      _nzWalksDbContext.Walks.Remove(walk);
      await _nzWalksDbContext.SaveChangesAsync();
      return walk;
    }

    public async Task<IEnumerable<Walk>> GetAllAsync()
    {
      return await 
        _nzWalksDbContext.
        Walks.
        Include(x => x.Region).
        Include(x => x.WalkDifficulty).
        ToListAsync();
    }

    public async Task<Walk> GetAsync(Guid id)
    {
      var walk = await
        _nzWalksDbContext.
        Walks.
        Include(x => x.Region).
        Include(x => x.WalkDifficulty).
        FirstOrDefaultAsync(x => x.Id == id);
      if (walk == null) return null;
      return walk;
    }

    public async Task<Walk> UpdateAsync(Guid id, Walk walk)
    {
      var existingWalk = await _nzWalksDbContext.Walks.FindAsync(id);
      if (existingWalk == null) return null;

      existingWalk.RegionId = walk.RegionId;
      existingWalk.Name = walk.Name;
      existingWalk.Length = walk.Length;
      existingWalk.WalkDifficultyId = walk.WalkDifficultyId;

      await _nzWalksDbContext.SaveChangesAsync();
      return existingWalk;
    }
  }
}
