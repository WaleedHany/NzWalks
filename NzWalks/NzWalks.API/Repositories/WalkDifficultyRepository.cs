using Microsoft.EntityFrameworkCore;
using NzWalks.API.Data;
using NzWalks.API.Models.Domain;

namespace NzWalks.API.Repositories
{
  public class WalkDifficultyRepository : IWalkDifficultyRepository
  {
    private readonly NzWalksDbContext _nzWalksDbContext;
    public WalkDifficultyRepository(NzWalksDbContext nzWalksDbContext)
    {
      this._nzWalksDbContext = nzWalksDbContext;
    }
    public Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
    {
      throw new NotImplementedException();
    }

    public async Task<WalkDifficulty> DeleteAsync(Guid id)
    {
      var walk = await _nzWalksDbContext.WalkDifficulty.FindAsync(id);
      if (walk == null) return null;
      // Delete region from data base
      _nzWalksDbContext.WalkDifficulty.Remove(walk);
      await _nzWalksDbContext.SaveChangesAsync();
      return walk;
    }

    public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
    {
      return await
         _nzWalksDbContext.WalkDifficulty.
         ToListAsync();
    }

    public async Task<WalkDifficulty> GetAsync(Guid id)
    {
      var walkDiff = await
        _nzWalksDbContext.
        WalkDifficulty.
        FirstOrDefaultAsync(x => x.Id == id);
      if (walkDiff == null) return null;
      return walkDiff;
    }

    public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
    {
      var existingWalkDiff = await _nzWalksDbContext.WalkDifficulty.FindAsync(id);
      if (existingWalkDiff == null) return null;

      existingWalkDiff.Code = walkDifficulty.Code;


      await _nzWalksDbContext.SaveChangesAsync();
      return existingWalkDiff;
    }

  }
}
