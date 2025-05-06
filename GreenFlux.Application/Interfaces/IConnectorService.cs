using GreenFlux.Application.DTOs;

namespace GreenFlux.Application.Interfaces
{
    public interface IConnectorService
    {
        Task<ConnectorDTO> SaveConnector(Guid chargeStationId, ConnectorCreateDTO connectorDTO);

        Task UpdateConnector(Guid chargeStationId, int id, ConnectorUpdateDTO groupDTO);

        Task<ConnectorDTO> GetConnector(Guid chargeStationId, int id);

        Task<List<ConnectorDTO>> GetAllConnectors(Guid chargeStationId);

        Task DeleteConnector(Guid chargeStationId, int id);
    }
}