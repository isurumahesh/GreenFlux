using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Interfaces;
using GreenFlux.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFlux.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService groupService;
        private readonly IValidator<GroupCreateDTO> createValidator;
        private readonly IValidator<GroupUpdateDTO> updateValidator;
        private readonly IMapper mapper;

        public GroupsController(IGroupService groupService, IValidator<GroupCreateDTO> createValidator, IValidator<GroupUpdateDTO> updateValidator, IMapper mapper)
        {
            this.groupService = groupService;
            this.createValidator = createValidator;
            this.updateValidator = updateValidator;
            this.mapper = mapper;

        }

        // GET: api/<GroupsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var groups = await groupService.GetAllGroups();
            return Ok(groups);
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
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