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
    [Route("api/chargestations/{chargeStationId}/connectors")]
    [ApiController]
    public class ConnectorsController : ControllerBase
    {
        private readonly IConnectorService _connectorService;
        private readonly IChargeStationService _chargeStationService;
        private readonly IValidator<ConnectorCreateDTO> createValidator;
        private readonly IValidator<ConnectorUpdateDTO> updateValidator;
        private readonly IMapper mapper;

        public ConnectorsController(IConnectorService connectorService, IChargeStationService chargeStationService,
            IValidator<ConnectorCreateDTO> createValidator, IValidator<ConnectorUpdateDTO> updateValidator, IMapper mapper)
        {
            this._connectorService = connectorService;
            this._chargeStationService = chargeStationService;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
            this.mapper = mapper;
        }

        // GET: api/<ConnectorsController>
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid chargeStationId)
        {
            var connectors = await _connectorService.GetAllConnectors(chargeStationId);
            return Ok(connectors);
        }

        // GET api/<ConnectorsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid chargeStationId, [FromRoute] int id)
        {
            var connector = await _connectorService.GetConnector(chargeStationId, id);

            if (connector is null)
            {
                return NotFound();
            }

            return Ok(connector);
        }

        // POST api/<ConnectorsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] Guid chargeStationId, [FromBody] ConnectorCreateDTO connector)
        {
            ValidationResult result = await createValidator.ValidateAsync(connector);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var chargeStation = await _chargeStationService.GetChargeStation(chargeStationId);
            if (chargeStation is null)
            {
                return NotFound();
            }

            var savedConnector = await _connectorService.SaveConnector(chargeStationId, connector);
            return CreatedAtAction(nameof(GetById), new { id = savedConnector.Id, chargeStationId = chargeStationId }, savedConnector);
        }

        // PUT api/<ConnectorsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid chargeStationId, [FromRoute] int id, [FromBody] ConnectorUpdateDTO connector)
        {
            ValidationResult result = await updateValidator.ValidateAsync(connector);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }


            var existingConnector = await _connectorService.GetConnector(chargeStationId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }

            await _connectorService.UpdateConnector(chargeStationId, id, connector);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid chargeStationId, [FromRoute] int id, JsonPatchDocument<ConnectorUpdateDTO> patchDocument)
        {
            var existingConnector = await _connectorService.GetConnector(chargeStationId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }

            var connectorUpdateDTO = mapper.Map<ConnectorUpdateDTO>(existingConnector);

            patchDocument.ApplyTo(connectorUpdateDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationResult result = await updateValidator.ValidateAsync(connectorUpdateDTO);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            await _connectorService.UpdateConnector(chargeStationId, id, connectorUpdateDTO);
            return NoContent();
        }

        // DELETE api/<ConnectorsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid chargeStationId, [FromRoute] int id)
        {
            var existingConnector = await _connectorService.GetConnector(chargeStationId, id);
            if (existingConnector is null)
            {
                return NotFound();
            }

            await _connectorService.DeleteConnector(chargeStationId, id);
            return NoContent();
        }
    }
}