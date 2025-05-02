using FluentValidation;
using GreenFlux.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Application.Validators
{
    public class CreateConnectorValidator:AbstractValidator<ConnectorCreateDTO>
    {
        public CreateConnectorValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, 5);
            RuleFor(x => x.MaxCurrent).GreaterThan(0);
        }
    }
}
