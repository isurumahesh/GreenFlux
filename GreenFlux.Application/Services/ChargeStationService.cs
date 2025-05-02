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
    public class ChargeStationService : IChargeStationService
    {
        private readonly IChargeStationRepository chargeStationRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;

        public ChargeStationService(IChargeStationRepository chargeStationRepository,IGroupRepository groupRepository, IMapper mapper)
        {
            this.chargeStationRepository = chargeStationRepository;
            this.groupRepository = groupRepository;
            this.mapper = mapper;
        }

        public async Task<ChargeStationDTO> SaveChargeStation(Guid groupId, ChargeStationCreateDTO chargeStationDTO)
        {
            if (chargeStationDTO.Connectors.Count >= ChargeStationConstants.MaxConnectorCount || !chargeStationDTO.Connectors.Any())
            {
                throw new CustomException
                {
                    HttpStatusCode = HttpStatusCode.UnprocessableContent,
                    Message = ErrorMessages.ConnectorCount
                };
            }

            var totalConnectorsCurrent = chargeStationDTO.Connectors.Sum(a => a.MaxCurrent);
            var group = await groupRepository.Get(groupId);

            if (group.Capacity < totalConnectorsCurrent)
            {
                throw new CustomException
                {
                    HttpStatusCode = HttpStatusCode.UnprocessableContent,
                    Message = ErrorMessages.MaxCurrentIsHigh
                };
            }

            var chargeStation = mapper.Map<ChargeStation>(chargeStationDTO);
            chargeStation.GroupId = groupId;
            await chargeStationRepository.Add(chargeStation);
            var mappedChargeStation = mapper.Map<ChargeStationDTO>(chargeStation);
            return mappedChargeStation;
        }

        public async Task UpdateChargeStation(Guid groupId, Guid id, ChargeStationUpdateDTO chargeStationDTO)
        {
            var chargeStation = await chargeStationRepository.Get(groupId, id);
            var mappedChargeStation = mapper.Map(chargeStationDTO, chargeStation);
            await chargeStationRepository.Update(mappedChargeStation);
        }

        public async Task<ChargeStationDTO> GetChargeStation(Guid groupId, Guid id)
        {
            var chargeStation = await chargeStationRepository.Get(groupId, id);
            var mappedChargeStation = mapper.Map<ChargeStationDTO>(chargeStation);
            return mappedChargeStation;
        }

        public async Task<ChargeStationDTO> GetChargeStation(Guid id)
        {
            var chargeStation = await chargeStationRepository.Get(id);
            var mappedChargeStation = mapper.Map<ChargeStationDTO>(chargeStation);
            return mappedChargeStation;
        }

        public async Task<List<ChargeStationDTO>> GetAllChargeStations(Guid groupId)
        {
            var chargeStations = await chargeStationRepository.GetAll(groupId);
            var mappedChargeStations = mapper.Map<List<ChargeStationDTO>>(chargeStations);
            return mappedChargeStations;
        }

        public async Task DeleteChargeStation(Guid groupId, Guid chargeStationId)
        {           
            var deleteChargeStation = await chargeStationRepository.Get(groupId, chargeStationId);
            await chargeStationRepository.Delete(deleteChargeStation);
        }
       
    }
}