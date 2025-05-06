using FluentValidation;
using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Validators
{
    public class UpdateGroupValidator : AbstractValidator<GroupUpdateDTO>
    {
        public UpdateGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Capacity).GreaterThan(0);
        }
    }
}