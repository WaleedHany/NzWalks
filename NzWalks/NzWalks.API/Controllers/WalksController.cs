using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NzWalks.API.Models.DTO;
using NzWalks.API.Repositories;

namespace NzWalks.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WalksController : Controller
  {
    private readonly IWalkRepository _walksRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _region;
    private readonly IWalkDifficultyRepository _walkDifficulty;

    public WalksController(IWalkRepository walksRepository, IMapper mapper, IRegionRepository region, IWalkDifficultyRepository walkDiff)
    {
      _walksRepository = walksRepository;
      _mapper = mapper;
      _region = region;
      _walkDifficulty = walkDiff;
    }

    [HttpGet]
    async public Task<IActionResult> GetAllWalksAsync()
    {
      var walks = await _walksRepository.GetAllAsync();
      var walksDTO = _mapper.Map<List<Models.DTO.Walk>>(walks);
      return Ok(walksDTO);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkAsync")]
    async public Task<IActionResult> GetWalkAsync(Guid id)
    {
      var walks = await _walksRepository.GetAsync(id);
      if (walks == null) return NotFound();

      var walksDTO = _mapper.Map<Models.DTO.Walk>(walks);
      return Ok(walksDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddWalkAsync(AddWalksRequest walkRequest)
    {
      if(!(await ValidateAddWalkAsync(walkRequest)))
      {
        return BadRequest(ModelState);
      }

      // Request(DTO) to Domain Model
      var walk = new Models.Domain.Walk()
      {
        Name = walkRequest.Name,
        Length = walkRequest.Length,
        RegionId = walkRequest.RegionId,
        WalkDifficultyId = walkRequest.WalkDifficultyId,
      };

      // Pass Details to Repository
      walk = await _walksRepository.AddAsync(walk);

      // Convert Back to DTO
      var walksDTO = _mapper.Map<Models.DTO.Walk>(walk);

      return CreatedAtAction(nameof(GetWalkAsync), new { id = walksDTO.Id }, walksDTO);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateWalkAsync
      ([FromRoute] Guid id, [FromBody] AddWalksRequest walkRequest)
    {
      if (!(await ValidateAddWalkAsync(walkRequest)))
      {
        return BadRequest(ModelState);
      }

      // Request(DTO) to Domain Model
      var walk = new Models.Domain.Walk()
      {
        Name = walkRequest.Name,
        Length = walkRequest.Length,
        RegionId = walkRequest.RegionId,
        WalkDifficultyId = walkRequest.WalkDifficultyId,
      };

      // Pass Details to Repository
      walk = await _walksRepository.UpdateAsync(id, walk);

      if (walk == null) return NotFound();

      // Convert Back to DTO
      var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);

      // Return Ok response
      return Ok(walkDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalkAsync(Guid id)
    {
      // Get walk from database
      var walk = await _walksRepository.DeleteAsync(id);
      // if null, return NotFound
      if (walk == null) return NotFound();
      // Convert to DTO
      var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);
      // return Ok response
      return Ok(walkDTO);
    }

    #region Private Methods

    private async Task<bool> ValidateAddWalkAsync(AddWalksRequest walkRequest)
    {
      //if(walkRequest == null)
      //{
      //  ModelState.AddModelError(nameof(walkRequest),
      //    $"Must have a value.");
      //  return false;
      //}

      //if (string.IsNullOrWhiteSpace(walkRequest.Name))
      //{
      //  ModelState.AddModelError(nameof(walkRequest.Name),
      //    $"{nameof(walkRequest.Name)} can not be null or empty or white space.");
      //}

      //if (walkRequest.Length <= 0)
      //{
      //  ModelState.AddModelError(nameof(walkRequest.Length),
      //    $"{nameof(walkRequest.Length)} can not be less than or equal zero.");
      //}

      var region = await _region.GetAsync(walkRequest.RegionId);
      if (region == null)
      {
        ModelState.AddModelError(nameof(walkRequest.RegionId),
          $"{nameof(walkRequest.RegionId)} is invalid.");
      }

      var walkDiff = await _walkDifficulty.GetAsync(walkRequest.WalkDifficultyId);
      if(walkDiff == null)
      {
        ModelState.AddModelError(nameof(walkRequest.WalkDifficultyId),
            $"{nameof(walkRequest.WalkDifficultyId)} is invalid.");
      }

      if (ModelState.ErrorCount > 0) return false;
      return true;
    }

    #endregion
  }
}
