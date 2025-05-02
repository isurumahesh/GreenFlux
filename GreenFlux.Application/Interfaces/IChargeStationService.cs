using GreenFlux.Application.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace GreenFlux.Application.Interfaces
{
    public interface IChargeStationService
    {
        Task<ChargeStationDTO> SaveChargeStation(Guid groupId, ChargeStationCreateDTO chargeStationCreateDTO);

        Task UpdateChargeStation(Guid groupdId, Guid id, ChargeStationUpdateDTO chargeStationCreateDTO);

        Task<ChargeStationDTO> GetChargeStation(Guid groupdId, Guid id);

        Task<ChargeStationDTO> GetChargeStation(Guid id);

        Task<List<ChargeStationDTO>> GetAllChargeStations(Guid groupdId);

        Task DeleteChargeStation(Guid groupdId, Guid id);
    }
}