using FluentValidation;
using GreenFlux.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
