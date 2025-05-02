using FluentValidation;
using GreenFlux.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
