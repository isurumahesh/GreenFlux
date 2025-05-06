using FluentValidation;
using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Validators
{
    public class CreateConnectorValidator : AbstractValidator<ConnectorCreateDTO>
    {
        public CreateConnectorValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, 5);
            RuleFor(x => x.MaxCurrent).GreaterThan(0);
        }
    }
}