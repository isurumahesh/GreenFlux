using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFlux.API.Controllers
{
    [Route("api/groups/{groupId}/chargestations")]
    [ApiController]
    public class ChargeStationsController(IChargeStationService chargeStationService, IGroupService groupService,
            IValidator<ChargeStationCreateDTO> createValidator, IValidator<ChargeStationUpdateDTO> updateValidator, IMapper mapper) : ControllerBase
    {
        // GET: api/<ChargeStationsController>
        [HttpGet]
        [ProducesResponseType(typeof(List<ChargeStationDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid groupId)
        {
            var chargeStations = await chargeStationService.GetAllChargeStations(groupId);
            return Ok(chargeStations);
        }

        // GET api/<ChargeStationsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ChargeStationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid groupId, [FromRoute] Guid id)
        {
            var chargeStation = await chargeStationService.GetChargeStation(groupId, id);
            if (chargeStation is null)
            {
                return NotFound();
            }

            return Ok(chargeStation);
        }

        // POST api/<ChargeStationsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromRoute] Guid groupId, [FromBody] ChargeStationCreateDTO chargeStation)
        {
            ValidationResult result = await createValidator.ValidateAsync(chargeStation);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var group = await groupService.GetGroup(groupId);
            if (group is null)
            {
                return NotFound();
            }

            var newChargeStation = await chargeStationService.SaveChargeStation(groupId, chargeStation);
            return CreatedAtAction(nameof(GetById), new { id = newChargeStation.Id, groupId = groupId }, newChargeStation);
        }

        // PUT api/<ChargeStationsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid groupId, [FromRoute] Guid id, [FromBody] ChargeStationUpdateDTO chargeStation)
        {
            ValidationResult result = await updateValidator.ValidateAsync(chargeStation);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var existingChargeStation = await chargeStationService.GetChargeStation(groupId, id);
            if (existingChargeStation is null)
            {
                return NotFound();
            }

            await chargeStationService.UpdateChargeStation(groupId, id, chargeStation);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] Guid groupId, [FromRoute] Guid id, JsonPatchDocument<ChargeStationUpdateDTO> patchDocument)
        {
            var existingConnector = await chargeStationService.GetChargeStation(groupId, id);
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

            await chargeStationService.UpdateChargeStation(groupId, id, chargeStationUpdateDTO);
            return NoContent();
        }

        // DELETE api/<ChargeStationsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid groupId, [FromRoute] Guid id)
        {
            var existingChargeStation = await chargeStationService.GetChargeStation(groupId, id);
            if (existingChargeStation is null)
            {
                return NotFound();
            }

            await chargeStationService.DeleteChargeStation(groupId, id);
            return NoContent();
        }
    }
}