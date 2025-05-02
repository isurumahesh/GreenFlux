using GreenFlux.Application.DTOs;
using GreenFlux.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFlux.API.Controllers
{
    [Route("api/groups/{groupId}/chargestations")]
    [ApiController]
    public class ChargeStationsController : ControllerBase
    {
        private readonly IChargeStationService _chargeStationService;
        private readonly IGroupService _groupService;
        private readonly IValidator<ChargeStationCreateDTO> createValidator;
        private readonly IValidator<ChargeStationUpdateDTO> updateValidator;
        private readonly IMapper mapper;

        public ChargeStationsController(IChargeStationService chargeStationService, IGroupService groupService,
            IValidator<ChargeStationCreateDTO> createValidator, IValidator<ChargeStationUpdateDTO> updateValidator, IMapper mapper)
        {
            this._chargeStationService = chargeStationService;
            this._groupService = groupService;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
            this.mapper = mapper;
        }

        // GET: api/<ChargeStationsController>
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid groupId)
        {
            var chargeStations = await _chargeStationService.GetAllChargeStations(groupId);
            return Ok(chargeStations);
        }

        // GET api/<ChargeStationsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid groupId, [FromRoute] Guid id)
        {
            var chargeStation = await _chargeStationService.GetChargeStation(groupId, id);
            if (chargeStation is null)
            {
                return NotFound();
            }

            return Ok(chargeStation);
        }

        // POST api/<ChargeStationsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] Guid groupId, [FromBody] ChargeStationCreateDTO chargeStation)
        {
            ValidationResult result = await createValidator.ValidateAsync(chargeStation);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var group = await _groupService.GetGroup(groupId);
            if (group is null)
            {
                return NotFound();
            }

            var newChargeStation = await _chargeStationService.SaveChargeStation(groupId, chargeStation);
            return CreatedAtAction(nameof(GetById), new { id = newChargeStation.Id, groupId = groupId }, newChargeStation);
        }

        // PUT api/<ChargeStationsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid groupId, [FromRoute] Guid id, [FromBody] ChargeStationUpdateDTO chargeStation)
        {
            ValidationResult result = await updateValidator.ValidateAsync(chargeStation);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var existingChargeStation = await _chargeStationService.GetChargeStation(groupId, id);
            if (existingChargeStation is null)
            {
                return NotFound();
            }

            await _chargeStationService.UpdateChargeStation(groupId, id, chargeStation);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid groupId, [FromRoute] Guid id, JsonPatchDocument<ChargeStationUpdateDTO> patchDocument)
        {
            var existingConnector = await _chargeStationService.GetChargeStation(groupId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }
          
            var chargeStationUpdateDTO = mapper.Map<ChargeStationUpdateDTO>(existingConnector);
        
            patchDocument.ApplyTo(chargeStationUpdateDTO, ModelState);
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationResult result = await updateValidator.ValidateAsync(chargeStationUpdateDTO);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            await _chargeStationService.UpdateChargeStation(groupId, id, chargeStationUpdateDTO);
            return NoContent();
        }

        // DELETE api/<ChargeStationsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid groupId, [FromRoute] Guid id)
        {
            var existingChargeStation = await _chargeStationService.GetChargeStation(groupId, id);
            if (existingChargeStation is null)
            {
                return NotFound();
            }

            await _chargeStationService.DeleteChargeStation(groupId, id);
            return NoContent();
        }
    }
}