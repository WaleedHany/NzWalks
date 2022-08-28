using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NzWalks.API.Models.Domain;
using NzWalks.API.Models.DTO;
using NzWalks.API.Repositories;

namespace NzWalks.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class RegionsController : Controller
  {
    private readonly IRegionRepository _regionsRepository;
    private readonly IMapper _mapper;

    public RegionsController(IRegionRepository regionsRepository, IMapper mapper)
    {
      _regionsRepository = regionsRepository;
      _mapper = mapper;
    }

    [HttpGet]
    async public Task<IActionResult> GetAllRegionsAsync()
    {
      var regions = await _regionsRepository.GetAllAsync();
      //// Expose the regions DTO
      //var regionsDTO = new List<Models.DTO.Region>();
      //regions.ToList().ForEach(region =>
      //{
      //  var regionDTO = new Models.DTO.Region()
      //  {
      //    Id = region.Id,
      //    Code = region.Code,
      //    Name = region.Name,
      //    Area = region.Area,
      //    Latitude = region.Latitude,
      //    Longitude = region.Latitude,
      //    Population = region.Population
      //    //Walks = region.Walks
      //  };
      //  regionsDTO.Add(regionDTO);
      //});
      var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);
      return Ok(regionsDTO);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetRegionAsync")]
    async public Task<IActionResult> GetRegionAsync(Guid id)
    {
      var region = await _regionsRepository.GetAsync(id);
      if (region == null) return NotFound();

      var regionDTO = _mapper.Map<Models.DTO.Region>(region);
      return Ok(regionDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddRegionAsync(AddRegionRequest regionRequest)
    {
      // Validate the request
      if (!ValidateAddRegionAsync(regionRequest))
        return BadRequest(ModelState);

      // Request(DTO) to Domain Model
      var region = new Models.Domain.Region()
      {
        Code = regionRequest.Code,
        Name = regionRequest.Name,
        Area = regionRequest.Area,
        Latitude = regionRequest.Latitude,
        Longitude = regionRequest.Longitude,
        Population = regionRequest.Population
      };

      // Pass Details to Repository
      region = await _regionsRepository.AddAsync(region);

      // Convert Back to DTO
      var regionDTO = new Models.DTO.Region()
      {
        Id = region.Id,
        Code = region.Code,
        Name = region.Name,
        Area = region.Area,
        Latitude = region.Latitude,
        Longitude = region.Longitude,
        Population = region.Population
      };

      return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id},regionDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteRegionAsync(Guid id)
    {
      // Get region from database
      var region = await _regionsRepository.DeleteAsync(id);
      // if null, return NotFound
      if (region == null) return NotFound();
      // Convert to DTO
      var regionDTO = _mapper.Map<Models.DTO.Region>(region);
      // return Ok response
      return Ok(regionDTO);
    }


    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateRegionAsync
      ([FromRoute]Guid id, [FromBody]AddRegionRequest regionRequest)
    {
      if (!ValidateAddRegionAsync(regionRequest))
        return BadRequest(ModelState);
      // Request(DTO) to Domain Model
      var region = new Models.Domain.Region()
      {
        Code = regionRequest.Code,
        Name = regionRequest.Name,
        Area = regionRequest.Area,
        Latitude = regionRequest.Latitude,
        Longitude = regionRequest.Longitude,
        Population = regionRequest.Population
      };

      // Pass Details to Repository
      region = await _regionsRepository.UpdateAsync(id, region);

      // Convert Back to DTO
      var regionDTO = _mapper.Map<Models.DTO.Region>(region);
      // Return Ok response
      return Ok(regionDTO);
    }

    #region Private Methods

    private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest regionRequest)
    {
      if(regionRequest == null)
      {
        ModelState.AddModelError(nameof(regionRequest),
          $"Add region data is required.");
        return false;
      }

      if (string.IsNullOrWhiteSpace(regionRequest.Code))
      {
        ModelState.AddModelError(nameof(regionRequest.Code),
          $"{nameof(regionRequest.Code)} can not be null or empty or white space.");
      }
      if (string.IsNullOrWhiteSpace(regionRequest.Name))
      {
        ModelState.AddModelError(nameof(regionRequest.Name),
          $"{nameof(regionRequest.Name)} can not be null or empty or white space.");
      }

      if (regionRequest.Area <= 0)
      {
        ModelState.AddModelError(nameof(regionRequest.Area),
          $"{nameof(regionRequest.Area)} can not be less than or equal zero.");
      }

      if (regionRequest.Latitude <= 0)
      {
        ModelState.AddModelError(nameof(regionRequest.Latitude),
          $"{nameof(regionRequest.Latitude)} can not be less than or equal zero.");
      }
      if (regionRequest.Longitude <= 0)
      {
        ModelState.AddModelError(nameof(regionRequest.Longitude),
          $"{nameof(regionRequest.Longitude)} can not be less than or equal zero.");
      }
      if (regionRequest.Population < 0)
      {
        ModelState.AddModelError(nameof(regionRequest.Population),
          $"{nameof(regionRequest.Population)} can not be negative.");
      }

      if (ModelState.ErrorCount > 0) return false;
      return true;
    }

    #endregion
  }
}