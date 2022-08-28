using FluentValidation;
using NzWalks.API.Models.DTO;

namespace NzWalks.API.Validators
{
  public class AddWalkDifficultyValidatior : AbstractValidator<AddWalkDifficultyRequest>
  {
    public AddWalkDifficultyValidatior()
    {
      RuleFor(x => x.Code).NotEmpty();
    }
  }
}
