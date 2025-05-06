using FluentValidation;
using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Validators
{
    public class CreateChargeStationValidator : AbstractValidator<ChargeStationCreateDTO>
    {
        public CreateChargeStationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Connectors).NotEmpty().Must(x => x.Count >= 1 && x.Count <= 5);
            RuleForEach(x => x.Connectors).SetValidator(new CreateConnectorValidator());
        }
    }
}