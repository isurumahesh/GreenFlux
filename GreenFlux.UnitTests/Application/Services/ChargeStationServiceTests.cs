using AutoMapper;
using GreenFlux.Application;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Exceptions;
using GreenFlux.Application.Services;
using GreenFlux.Domain.Constants;
using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Moq;

namespace GreenFlux.UnitTests.Application.Services
{
    public class ChargeStationServiceTests
    {
        private readonly Mock<IGroupRepository> mockGroupRepository;
        private readonly Mock<IChargeStationRepository> mockChargeStationRepository;
        private readonly ChargeStationService chargeStationService;
        private readonly IMapper mapper;

        public ChargeStationServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            mapper = mappingConfig.CreateMapper();

            mockGroupRepository = new Mock<IGroupRepository>();
            mockChargeStationRepository = new Mock<IChargeStationRepository>();
            chargeStationService = new ChargeStationService(mockChargeStationRepository.Object, mockGroupRepository.Object, mapper);
        }

        [Fact]
        public async Task Should_ReturnChargeStationDTO_WhenChargeStationIsSuccessfullySaved()
        {
            var groupId = Guid.NewGuid();
            var chargeStationId = Guid.NewGuid();
            var chargeStationDto = new ChargeStationCreateDTO { Name = "ChargeStation 1", Connectors = new List<ConnectorCreateDTO> { new ConnectorCreateDTO { MaxCurrent = 20 } } };
            var group = new Group { Id = groupId, Name = "Group A", Capacity = 100 };
            mockGroupRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(group);
            mockChargeStationRepository.Setup(repo => repo.Add(It.IsAny<ChargeStation>())).ReturnsAsync(chargeStationId);

            var result = await chargeStationService.SaveChargeStation(groupId, chargeStationDto);

            Assert.NotNull(result);
            Assert.Equal(chargeStationDto.Name, result.Name);
        }

        [Fact]
        public async Task Should_ThrowCustomException_WhenNoConnectorsAreAvailable()
        {
            var groupId = Guid.NewGuid();
            var chargeStationId = Guid.NewGuid();
            var chargeStationDto = new ChargeStationCreateDTO { Name = "ChargeStation 1" };
            mockChargeStationRepository.Setup(repo => repo.Add(It.IsAny<ChargeStation>())).ReturnsAsync(chargeStationId);

            var exception = await Assert.ThrowsAsync<CustomException>(() => chargeStationService.SaveChargeStation(groupId, chargeStationDto));
            Assert.Equal(ErrorMessages.ConnectorCount, exception.Message);
        }

        [Fact]
        public async Task UpdateChargeStation_WithValidData_UpdatesChargeStation()
        {
            var groupId = Guid.NewGuid();
            var chargeStationId = Guid.NewGuid();
            var existingChargeStation = new ChargeStation { Id = chargeStationId, GroupId = groupId, Name = "Station 1" };
            var chargeStationUpdateDTO = new ChargeStationUpdateDTO { Name = "Station new" };

            mockChargeStationRepository.Setup(repo => repo.Get(groupId, chargeStationId))
                .ReturnsAsync(existingChargeStation);

            await chargeStationService.UpdateChargeStation(groupId, chargeStationId, chargeStationUpdateDTO);

            mockChargeStationRepository.Verify(repo => repo.Update(It.IsAny<ChargeStation>()), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnChargeStation_WhenIdIsValid()
        {
            var chargeStationId = Guid.NewGuid();
            var existingChargeStation = new ChargeStation { Id = chargeStationId, Name = "Station 1" };
            var expectedDTO = new ChargeStationDTO { Id = chargeStationId, Name = "Station 1" };

            mockChargeStationRepository.Setup(repo => repo.Get(It.IsAny<Guid>()))
                .ReturnsAsync(existingChargeStation);

            var result = await chargeStationService.GetChargeStation(chargeStationId);

            Assert.NotNull(result);
            Assert.Equal(expectedDTO.Name, result.Name);
        }

        [Fact]
        public async Task Should_ReturnChargeStations_WhenChargeStationsExists()
        {
            var groupId = Guid.NewGuid();
            var chargeStations = new List<ChargeStation>
                {
                    new ChargeStation { Id = Guid.NewGuid(), GroupId = groupId, Name = "Station 1" },
                    new ChargeStation { Id = Guid.NewGuid(), GroupId = groupId, Name = "Station 2" }
                };
            var expectedDTOs = chargeStations.Select(cs => new ChargeStationDTO { Id = cs.Id, Name = cs.Name }).ToList();

            mockChargeStationRepository.Setup(repo => repo.GetAll(It.IsAny<Guid>()))
                .ReturnsAsync(chargeStations);

            var result = await chargeStationService.GetAllChargeStations(groupId);

            Assert.Equal(2, result.Count);
            Assert.Equal("Station 1", result[0].Name);
        }

        [Fact]
        public async Task Should_DeletesChargeStation_WithValidId()
        {
            var groupId = Guid.NewGuid();
            var chargeStationId = Guid.NewGuid();
            var existingChargeStation = new ChargeStation { Id = chargeStationId, GroupId = groupId };

            mockChargeStationRepository.Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingChargeStation);

            await chargeStationService.DeleteChargeStation(groupId, chargeStationId);

            mockChargeStationRepository.Verify(repo => repo.Delete(existingChargeStation), Times.Once);
        }
    }
}