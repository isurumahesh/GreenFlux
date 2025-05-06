using FluentValidation;
using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Validators
{
    public class UpdateConnectorValidator : AbstractValidator<ConnectorUpdateDTO>
    {
        public UpdateConnectorValidator()
        {
            RuleFor(x => x.MaxCurrent).GreaterThan(0);
        }
    }
}