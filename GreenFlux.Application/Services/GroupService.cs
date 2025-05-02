using AutoMapper;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Exceptions;
using GreenFlux.Application.Interfaces;
using GreenFlux.Domain.Constants;
using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.Net;


namespace GreenFlux.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IChargeStationRepository chargeStationRepository;
        private readonly IMapper mapper;

        public GroupService(IGroupRepository groupRepository, IChargeStationRepository chargeStationRepository, IMapper mapper)
        {
            this.groupRepository = groupRepository;
            this.chargeStationRepository = chargeStationRepository;
            this.mapper = mapper;
        }

        public async Task<GroupDTO> SaveGroup(GroupCreateDTO groupDTO)
        {
            if(groupDTO.ChargeStation is not null)
            {
                if (groupDTO.ChargeStation.Connectors.Count >= ChargeStationConstants.MaxConnectorCount || !groupDTO.ChargeStation.Connectors.Any())
                {
                    throw new CustomException
                    {
                        HttpStatusCode = HttpStatusCode.UnprocessableContent,
                        Message = ErrorMessages.ConnectorCount
                    };
                }

                var totalConnectorsCurrent = groupDTO.ChargeStation.Connectors.Sum(a => a.MaxCurrent);

                if (groupDTO.Capacity < totalConnectorsCurrent)
                {
                    throw new CustomException
                    {
                        HttpStatusCode = HttpStatusCode.UnprocessableContent,
                        Message = ErrorMessages.MaxCurrentIsHigh
                    };
                }
            }
        
            var group = mapper.Map<Group>(groupDTO);
            await groupRepository.Add(group);
            var groupDto = mapper.Map<GroupDTO>(group);
            return groupDto;
        }

        public async Task UpdateGroup(Guid groupId, GroupUpdateDTO groupDTO)
        {
            await VerifyCapacity(groupId, groupDTO.Capacity);
            var group = await groupRepository.Get(groupId);        
            var mappedGroup = mapper.Map(groupDTO, group);
            await groupRepository.Update(mappedGroup);
        }

        public async Task<GroupDTO> GetGroup(Guid id)
        {
            var group = await groupRepository.Get(id);
            var mappedGroup = mapper.Map<GroupDTO>(group);
            return mappedGroup;
        }

        public async Task<List<GroupDTO>> GetAllGroups()
        {           
            var groups = await groupRepository.GetAll();
            var mappedGroups = mapper.Map<List<GroupDTO>>(groups);
            return mappedGroups;
        }

        public async Task DeleteGroup(Guid id)
        {
            var deleteGroup = await groupRepository.Get(id);
            await groupRepository.Delete(deleteGroup);
        }

        private async Task VerifyCapacity(Guid groupId, int capacity)
        {
            var chargeStations = await chargeStationRepository.GetAll(groupId);

            if (!chargeStations.Any()) return;

            var totalAmps = 0;
            foreach (var item in chargeStations)
            {
                totalAmps = totalAmps + item.Connectors.Sum(a => a.MaxCurrent);
            }

            if (capacity < totalAmps)
            {
                throw new CustomException
                {
                    HttpStatusCode = HttpStatusCode.UnprocessableContent,
                    Message = ErrorMessages.CapacityTooLow
                };
            }

        }
    }
}