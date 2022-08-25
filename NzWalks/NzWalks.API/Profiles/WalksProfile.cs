using AutoMapper;

namespace NzWalks.API.Profiles
{
  public class WalksProfile : Profile
  {
    public WalksProfile()
    {
      CreateMap<Models.Domain.Walk, Models.DTO.Walk>().ReverseMap();
      CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
    }
  }
}
