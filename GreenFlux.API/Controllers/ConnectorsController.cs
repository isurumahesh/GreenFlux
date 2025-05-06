using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFlux.API.Controllers
{
    [Route("api/chargestations/{chargeStationId}/connectors")]
    [ApiController]
    public class ConnectorsController(IConnectorService connectorService, IChargeStationService chargeStationService,
            IValidator<ConnectorCreateDTO> createValidator, IValidator<ConnectorUpdateDTO> updateValidator, IMapper mapper) : ControllerBase
    {
        // GET: api/<ConnectorsController>
        [HttpGet]
        [ProducesResponseType(typeof(List<ConnectorDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromRoute] Guid chargeStationId)
        {
            var connectors = await connectorService.GetAllConnectors(chargeStationId);
            return Ok(connectors);
        }

        // GET api/<ConnectorsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConnectorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid chargeStationId, [FromRoute] int id)
        {
            var connector = await connectorService.GetConnector(chargeStationId, id);

            if (connector is null)
            {
                return NotFound();
            }

            return Ok(connector);
        }

        // POST api/<ConnectorsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromRoute] Guid chargeStationId, [FromBody] ConnectorCreateDTO connector)
        {
            ValidationResult result = await createValidator.ValidateAsync(connector);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var chargeStation = await chargeStationService.GetChargeStation(chargeStationId);
            if (chargeStation is null)
            {
                return NotFound();
            }

            var savedConnector = await connectorService.SaveConnector(chargeStationId, connector);
            return CreatedAtAction(nameof(GetById), new { id = savedConnector.Id, chargeStationId = chargeStationId }, savedConnector);
        }

        // PUT api/<ConnectorsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid chargeStationId, [FromRoute] int id, [FromBody] ConnectorUpdateDTO connector)
        {
            ValidationResult result = await updateValidator.ValidateAsync(connector);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var existingConnector = await connectorService.GetConnector(chargeStationId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }

            await connectorService.UpdateConnector(chargeStationId, id, connector);
            return NoContent();
        }

        // DELETE api/<ConnectorsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid chargeStationId, [FromRoute] int id)
        {
            var existingConnector = await connectorService.GetConnector(chargeStationId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }

            await connectorService.DeleteConnector(chargeStationId, id);
            return NoContent();
        }
    }
}