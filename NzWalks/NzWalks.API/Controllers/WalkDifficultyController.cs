using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NzWalks.API.Models.DTO;
using NzWalks.API.Repositories;

namespace NzWalks.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WalkDifficultyController : Controller
  {
    private readonly IWalkDifficultyRepository _walkDifficultyRepository;
    private readonly IMapper _mapper;
    public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepo, IMapper mapper)
    {
      _walkDifficultyRepository = walkDifficultyRepo;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWalkDifficulties()
    {
      return Ok(await _walkDifficultyRepository.GetAllAsync());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetWalkDifficultyById(Guid id)
    {
      var walkDiff = await _walkDifficultyRepository.GetAsync(id);
      if (walkDiff == null) return NotFound();

      var walkDiffDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);
      return Ok(walkDiffDTO);
    }


    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateWalkDifficultyAsync
      ([FromRoute] Guid id, [FromBody] AddWalkDifficultyRequest walkDifficultyRequest)
    {
      if(!ValidateAddWalkDifficultyAsync(walkDifficultyRequest))
      {
        return BadRequest(ModelState);
      }
      // Request(DTO) to Domain Model
      var walkDiff = new Models.Domain.WalkDifficulty()
      {
        Code = walkDifficultyRequest.Code
      };

      // Pass Details to Repository
      walkDiff = await _walkDifficultyRepository.UpdateAsync(id, walkDiff);

      if (walkDiff == null) return NotFound();

      // Convert Back to DTO
      var walkDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);

      // Return Ok response
      return Ok(walkDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
    {
      // Get walk from database
      var walkDiff = await _walkDifficultyRepository.DeleteAsync(id);
      // if null, return NotFound
      if (walkDiff == null) return NotFound();
      // Convert to DTO
      var walkDiffDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);
      // return Ok response
      return Ok(walkDiffDTO);
    }

#region Validations
    private bool ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest walkDiffRequest)
    {
      if (walkDiffRequest == null)
      {
        ModelState.AddModelError(nameof(walkDiffRequest),
          $"Data is required.");
        return false;
      }

      if (string.IsNullOrWhiteSpace(walkDiffRequest.Code))
      {
        ModelState.AddModelError(nameof(walkDiffRequest.Code),
          $"{nameof(walkDiffRequest.Code)} is required.");
      }

      if (ModelState.ErrorCount > 0) return false;
      return true;
    }

#endregion
  }
}
