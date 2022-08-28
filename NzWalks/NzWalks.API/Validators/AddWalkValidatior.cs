using FluentValidation;
using NzWalks.API.Models.DTO;

namespace NzWalks.API.Validators
{
  public class AddWalkValidatior : AbstractValidator<AddWalksRequest>
  {
    public AddWalkValidatior()
    {
      RuleFor(x => x.Name).NotEmpty();
      RuleFor(x => x.Length).GreaterThan(0);
    }
  }
}
