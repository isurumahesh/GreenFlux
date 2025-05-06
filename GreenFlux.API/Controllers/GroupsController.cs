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
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController(IGroupService groupService, IValidator<GroupCreateDTO> createValidator, IValidator<GroupUpdateDTO> updateValidator, IMapper mapper) : ControllerBase
    {
        // GET: api/<GroupsController>
        [HttpGet]
        [ProducesResponseType(typeof(List<GroupDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var groups = await groupService.GetAllGroups();
            return Ok(groups);
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var group = await groupService.GetGroup(id);
            if (group is null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // POST api/<GroupsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] GroupCreateDTO group)
        {
            ValidationResult result = await createValidator.ValidateAsync(group);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var savedGroup = await groupService.SaveGroup(group);
            return CreatedAtAction(nameof(GetById), new { id = savedGroup.Id }, savedGroup);
        }

        // PUT api/<GroupsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] GroupUpdateDTO group)
        {
            ValidationResult result = await updateValidator.ValidateAsync(group);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            var existingGroup = await groupService.GetGroup(id);
            if (existingGroup is null)
            {
                return NotFound();
            }

            await groupService.UpdateGroup(id, group);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] Guid id, JsonPatchDocument<GroupUpdateDTO> patchDocument)
        {
            var existingGroup = await groupService.GetGroup(id);
            if (existingGroup is null)
            {
                return NotFound();
            }

            var groupUpdateDTO = mapper.Map<GroupUpdateDTO>(existingGroup);

            patchDocument.ApplyTo(groupUpdateDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationResult result = await updateValidator.ValidateAsync(groupUpdateDTO);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return BadRequest(result);
            }

            await groupService.UpdateGroup(id, groupUpdateDTO);
            return NoContent();
        }

        // DELETE api/<GroupsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var existingGroup = await groupService.GetGroup(id);
            if (existingGroup is null)
            {
                return NotFound();
            }

            await groupService.DeleteGroup(id);
            return NoContent();
        }
    }
}