using AutoMapper;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Exceptions;
using GreenFlux.Application.Interfaces;
using GreenFlux.Domain.Constants;
using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.Net;
using System.Text.RegularExpressions;

namespace GreenFlux.Application.Services
{
    public class ConnectorService : IConnectorService
    {
        private readonly IConnectorRepository connectorRepository;
        private readonly IChargeStationRepository chargeStationRepository;
        private readonly IMapper mapper;

        public ConnectorService(IConnectorRepository connectorRepository, IChargeStationRepository chargeStationRepository, IMapper mapper)
        {
            this.connectorRepository = connectorRepository;
            this.chargeStationRepository = chargeStationRepository;
            this.mapper = mapper;
        }

        public async Task<ConnectorDTO> SaveConnector(Guid chargeStationId, ConnectorCreateDTO connectorDTO)
        {
            var chargeStation= await chargeStationRepository.Get(chargeStationId);

            if (chargeStation.Connectors.Count == ChargeStationConstants.MaxConnectorCount)
            {
                throw new CustomException
                {
                    HttpStatusCode = HttpStatusCode.UnprocessableContent,
                    Message = ErrorMessages.ConnectorCount
                };
            }

            var connector = mapper.Map<Connector>(connectorDTO);
            connector.ChargeStationId = chargeStationId;
            await connectorRepository.Add(connector);
            var mappedConnector = mapper.Map<ConnectorDTO>(connector);
            return mappedConnector;
        }

        public async Task UpdateConnector(Guid chargeStationId, int id, ConnectorUpdateDTO connectorDTO)
        {
            var connector = await connectorRepository.Get(id, chargeStationId);
            await VerifyCapacity(chargeStationId, connector.MaxCurrent, connectorDTO.MaxCurrent);
            var mappedConnector = mapper.Map(connectorDTO, connector);     
            await connectorRepository.Update(mappedConnector);
        }

        public async Task<ConnectorDTO> GetConnector(Guid chargeStationId, int id)
        {
            var connector = await connectorRepository.Get(id, chargeStationId);
            var mappedConnector = mapper.Map<ConnectorDTO>(connector);
            return mappedConnector;
        }

        public async Task<List<ConnectorDTO>> GetAllConnectors(Guid chargeStationId)
        {
            var connectors = await connectorRepository.GetAll(chargeStationId);
            var mappedConnectors = mapper.Map<List<ConnectorDTO>>(connectors);
            return mappedConnectors;
        }

        public async Task DeleteConnector(Guid chargeStationId, int id)
        {
            var deleteConnector = await connectorRepository.Get(id, chargeStationId);
            await connectorRepository.Delete(deleteConnector);
        }

        private async Task VerifyCapacity(Guid chargeStationId, int existingCapacity, int newCapacity)
        {
            if (newCapacity <= existingCapacity)
            {
                return;
            }

            var capacityDifference = newCapacity - existingCapacity;
            var chargeStation = await chargeStationRepository.Get(chargeStationId);
            var totalCapacity = chargeStation.Group.Capacity;
            var chargeStations = await chargeStationRepository.GetAll(chargeStation.GroupId);

            if (!chargeStations.Any()) return;

            var totalAmps = 0;
            foreach (var item in chargeStations)
            {
                totalAmps = totalAmps + item.Connectors.Sum(a => a.MaxCurrent);
            }

            if (totalCapacity < (totalAmps + capacityDifference))
            {
                throw new CustomException
                {
                    HttpStatusCode = HttpStatusCode.UnprocessableContent,
                    Message = ErrorMessages.MaxCurrentIsHigh
                };
            }
        }
    }
}