using AutoMapper;
using GreenFlux.Application;
using GreenFlux.Application.DTOs;
using GreenFlux.Application.Exceptions;
using GreenFlux.Application.Services;
using GreenFlux.Domain.Constants;
using GreenFlux.Domain.Entities;
using GreenFlux.Domain.Interfaces;
using Moq;

namespace GreenFlux.UnitTests.Application.Services
{
    public class ConnectorServiceTests
    {
        private readonly Mock<IConnectorRepository> mockConnectorRepository;
        private readonly Mock<IChargeStationRepository> mockChargeStationRepository;
        private readonly ConnectorService connectorService;
        private readonly IMapper mapper;

        public ConnectorServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            mapper = mappingConfig.CreateMapper();

            mockConnectorRepository = new Mock<IConnectorRepository>();
            mockChargeStationRepository = new Mock<IChargeStationRepository>();
            connectorService = new ConnectorService(mockConnectorRepository.Object, mockChargeStationRepository.Object, mapper);
        }

        [Fact]
        public async Task Should_ReturnConnectorDTO_WhenConnectorIsSavedSuccessfully()
        {
            var chargeStationId = Guid.NewGuid();
            var connectorId = 1;
            var connectorDTO = new ConnectorCreateDTO { MaxCurrent = 100 };
            var group = new Group { Capacity = 50, Id = Guid.NewGuid() };
            var connectors = new List<Connector>
                {
                    new Connector { Id=1, MaxCurrent = 20 },
                    new Connector { Id=2, MaxCurrent = 20 }
                };
            var chargeStation = new ChargeStation { Id = chargeStationId, Group = group, Connectors = connectors };

            mockChargeStationRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(chargeStation);
            mockConnectorRepository.Setup(repo => repo.Add(It.IsAny<Connector>())).ReturnsAsync(connectorId);

            var result = await connectorService.SaveConnector(chargeStationId, connectorDTO);

            Assert.NotNull(result);
            Assert.Equal(connectorDTO.MaxCurrent, result.MaxCurrent);
        }

        [Fact]
        public async Task Should_UpdateConnector_WhenMaxCurrentIsValid()
        {
            var chargeStationId = Guid.NewGuid();
            var id = 1;
            var connectorDTO = new ConnectorUpdateDTO { MaxCurrent = 20 };
            var existingConnector = new Connector { Id = id, MaxCurrent = 30, ChargeStationId = chargeStationId };

            mockConnectorRepository.Setup(repo => repo.Get(id, chargeStationId)).ReturnsAsync(existingConnector);
            mockConnectorRepository.Setup(repo => repo.Update(existingConnector)).Returns(Task.CompletedTask);

            await connectorService.UpdateConnector(chargeStationId, id, connectorDTO);

            mockConnectorRepository.Verify(repo => repo.Get(id, chargeStationId), Times.Once);
            mockConnectorRepository.Verify(repo => repo.Update(It.IsAny<Connector>()), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowCustomException_WhenMaxCurrentIsInValid()
        {
            var chargeStationId = Guid.NewGuid();
            var id = 1;
            var connectorDTO = new ConnectorUpdateDTO { MaxCurrent = 40 };
            var existingConnector = new Connector { Id = id, MaxCurrent = 20, ChargeStationId = chargeStationId };

            var group = new Group { Capacity = 50, Id = Guid.NewGuid() };
            var connectors = new List<Connector>
                {
                    new Connector { Id=id, MaxCurrent = 20 },
                    new Connector { Id=2, MaxCurrent = 20 }
                };
            var chargeStation = new ChargeStation { Id = chargeStationId, Group = group, Connectors = connectors };

            mockChargeStationRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(chargeStation);
            mockConnectorRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(existingConnector);
            mockChargeStationRepository.Setup(repo => repo.GetAll(It.IsAny<Guid>())).ReturnsAsync(new List<ChargeStation> { chargeStation });

            var exception = await Assert.ThrowsAsync<CustomException>(() => connectorService.UpdateConnector(chargeStationId, id, connectorDTO));
            Assert.Equal(ErrorMessages.MaxCurrentIsHigh, exception.Message);
        }

        [Fact]
        public async Task Should_UpdateConnector_WhenGroupCapacityIsHigh()
        {
            var chargeStationId = Guid.NewGuid();
            var id = 1;
            var connectorDTO = new ConnectorUpdateDTO { MaxCurrent = 40 };
            var existingConnector = new Connector { Id = id, MaxCurrent = 20, ChargeStationId = chargeStationId };

            var group = new Group { Capacity = 100, Id = Guid.NewGuid() };
            var connectors = new List<Connector>
                {
                    new Connector { Id=id, MaxCurrent = 20 },
                    new Connector { Id=2, MaxCurrent = 20 }
                };
            var chargeStation = new ChargeStation { Id = chargeStationId, Group = group, Connectors = connectors };

            mockChargeStationRepository.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(chargeStation);
            mockConnectorRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(existingConnector);
            mockChargeStationRepository.Setup(repo => repo.GetAll(It.IsAny<Guid>())).ReturnsAsync(new List<ChargeStation> { chargeStation });

            await connectorService.UpdateConnector(chargeStationId, id, connectorDTO);

            mockConnectorRepository.Verify(repo => repo.Update(It.IsAny<Connector>()), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnConnectors_WhenConnectorsExists()
        {
            // Arrange
            var chargeStationId = Guid.NewGuid();
            var connectors = new List<Connector>
            {
                 new Connector { Id = 1, MaxCurrent = 30, ChargeStationId = chargeStationId },
                 new Connector { Id = 2, MaxCurrent = 40, ChargeStationId = chargeStationId }
            };
            var connectorDTOs = new List<ConnectorDTO>
            {
                new ConnectorDTO { Id = 1, MaxCurrent = 30 },
                new ConnectorDTO { Id = 2, MaxCurrent = 40 }
            };

            mockConnectorRepository.Setup(repo => repo.GetAll(It.IsAny<Guid>())).ReturnsAsync(connectors);

            var result = await connectorService.GetAllConnectors(chargeStationId);

            Assert.Equal(connectorDTOs, result);
            mockConnectorRepository.Verify(repo => repo.GetAll(chargeStationId), Times.Once);
        }

        [Fact]
        public async Task Should_DeleteConnector_WhenConnectorIsValid()
        {
            var chargeStationId = Guid.NewGuid();
            var id = 1;
            var connector = new Connector { Id = id, MaxCurrent = 30, ChargeStationId = chargeStationId };

            mockConnectorRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(connector);
            mockConnectorRepository.Setup(repo => repo.Delete(It.IsAny<Connector>())).Returns(Task.CompletedTask);

            await connectorService.DeleteConnector(chargeStationId, id);

            mockConnectorRepository.Verify(repo => repo.Get(id, chargeStationId), Times.Once);
            mockConnectorRepository.Verify(repo => repo.Delete(connector), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnConnector_WhenConnectorIdIsValid()
        {
            var chargeStationId = Guid.NewGuid();
            var id = 1;
            var connector = new Connector { Id = id, MaxCurrent = 30, ChargeStationId = chargeStationId };
            var connectorDTO = new ConnectorDTO { Id = id, MaxCurrent = 30 };

            mockConnectorRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(connector);

            var result = await connectorService.GetConnector(chargeStationId, id);

            Assert.Equal(connectorDTO, result);
            mockConnectorRepository.Verify(repo => repo.Get(id, chargeStationId), Times.Once);
        }
    }
}