using FluentValidation;
using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Validators
{
    public class UpdateChargeStationValidator : AbstractValidator<ChargeStationUpdateDTO>
    {
        public UpdateChargeStationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}