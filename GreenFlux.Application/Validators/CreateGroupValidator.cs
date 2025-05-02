using FluentValidation;
using GreenFlux.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Application.Validators
{
    public class CreateGroupValidator : AbstractValidator<GroupCreateDTO>
    {  
        public CreateGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Capacity).GreaterThan(0);
            When(x => x.ChargeStation != null, () =>
            {
                RuleFor(x => x.ChargeStation)
                    .SetValidator(new CreateChargeStationValidator());
            });
        }
    }
}
