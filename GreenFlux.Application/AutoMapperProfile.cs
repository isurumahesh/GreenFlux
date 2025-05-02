using AutoMapper;
using GreenFlux.Application.DTOs;
using GreenFlux.Domain.Entities;

namespace GreenFlux.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GroupCreateDTO, Group>()
           .ForMember(dest => dest.ChargeStations, opt => opt.MapFrom(src =>
               src.ChargeStation != null
                   ? new List<ChargeStation> { MapChargeStation(src.ChargeStation) }
                   : new List<ChargeStation>()));

            CreateMap<GroupUpdateDTO, Group>().ReverseMap();
            CreateMap<GroupDTO, GroupUpdateDTO>();
            CreateMap<ConnectorCreateDTO, Connector>();
            CreateMap<ChargeStationCreateDTO, ChargeStation>();

            CreateMap<Group, GroupDTO>();
            CreateMap<Connector, ConnectorDTO>();
            CreateMap<ConnectorDTO, ConnectorUpdateDTO>();
            CreateMap<Connector, ConnectorUpdateDTO>().ReverseMap();
            CreateMap<ChargeStation, ChargeStationDTO>();
            CreateMap<ChargeStationDTO, ChargeStationUpdateDTO>();
            CreateMap<ChargeStation, ChargeStationUpdateDTO>().ReverseMap();
        }

        private ChargeStation MapChargeStation(ChargeStationCreateDTO chargeStationCreateDto)
        {

            return new ChargeStation
            {
                Name = chargeStationCreateDto.Name,
                Connectors = chargeStationCreateDto.Connectors.Select(c => new Connector
                {
                    Id = c.Id,
                    MaxCurrent = c.MaxCurrent
                }).ToList()
            };
        }
    }
}