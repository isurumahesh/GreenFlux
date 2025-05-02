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
    public class GroupServiceTests
    {
        private readonly Mock<IGroupRepository> mockGroupRepository;
        private readonly Mock<IChargeStationRepository> mockChargeStationRepository;
        private readonly GroupService groupService;
        private readonly IMapper mapper;

        public GroupServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            mapper = mappingConfig.CreateMapper();

            mockGroupRepository = new Mock<IGroupRepository>();
            mockChargeStationRepository = new Mock<IChargeStationRepository>();
            groupService = new GroupService(mockGroupRepository.Object, mockChargeStationRepository.Object, mapper);
        }

        [Fact]
        public async Task Should_ReturnGroupDTO_WhenValidDataIsProvided()
        {
            var groupId = Guid.NewGuid();
            var groupCreateDTO = new GroupCreateDTO { Name = "Group A", Capacity = 100 };
            mockGroupRepository.Setup(repo => repo.Add(It.IsAny<Group>())).ReturnsAsync(groupId);

            var result = await groupService.SaveGroup(groupCreateDTO);

            Assert.NotNull(result);
            Assert.Equal(groupCreateDTO.Capacity, result.Capacity);
            Assert.Equal(groupCreateDTO.Name, result.Name);
        }

        [Fact]
        public async Task Should_UpdateGroup_WhenValidDataIsProvided()
        {
            var groupId = Guid.NewGuid();
            var groupUpdateDTO = new GroupUpdateDTO { Name = "Group A", Capacity = 200 };
            var group = new Group { Id = groupId, Name = "Original Group", Capacity = 100 };
            var chargeStations = new List<ChargeStation>
            {
                new ChargeStation
                {
                    GroupId= groupId,
                    Connectors=new List<Connector>
                    {
                        new Connector { MaxCurrent = 30 },
                        new Connector { MaxCurrent = 40 }
                    }
                }
            };
            mockGroupRepository.Setup(repo => repo.Get(groupId)).ReturnsAsync(group);
            mockGroupRepository.Setup(repo => repo.Update(It.IsAny<Group>())).Returns(Task.CompletedTask);
            mockChargeStationRepository.Setup(repo => repo.GetAll(groupId)).ReturnsAsync(chargeStations);

            await groupService.UpdateGroup(groupId, groupUpdateDTO);

            mockChargeStationRepository.Verify(repo => repo.GetAll(groupId), Times.Once);
            mockGroupRepository.Verify(repo => repo.Get(groupId), Times.Once);
        }

        [Fact]
        public async Task Should_ThrowCustomException_WhenCapacityIsTooLow()
        {
            var groupUpdateDTO = new GroupUpdateDTO { Name = "Group A", Capacity = 50 };
            var groupId = Guid.NewGuid();
            var chargeStations = new List<ChargeStation>
            {
                new ChargeStation
                {
                    GroupId= groupId,
                    Connectors=new List<Connector>
                    {
                        new Connector { MaxCurrent = 30 },
                        new Connector { MaxCurrent = 40 }
                    }
                }
            };

            mockChargeStationRepository.Setup(repo => repo.GetAll(groupId)).ReturnsAsync(chargeStations);

            var exception = await Assert.ThrowsAsync<CustomException>(() => groupService.UpdateGroup(groupId, groupUpdateDTO));
            Assert.Equal(ErrorMessages.CapacityTooLow, exception.Message);
        }

        [Fact]
        public async Task Should_UpdateGroup_WhenNoConnectorsExist()
        {
            var groupId = Guid.NewGuid();
            var groupUpdateDTO = new GroupUpdateDTO { Name = "Group A", Capacity = 200 };
            var group = new Group { Id = groupId, Name = "Original Group", Capacity = 100 };
            mockGroupRepository.Setup(repo => repo.Get(groupId)).ReturnsAsync(group);
            mockChargeStationRepository.Setup(repo => repo.GetAll(groupId)).ReturnsAsync(new List<ChargeStation>());

            await groupService.UpdateGroup(groupId, groupUpdateDTO);

            mockChargeStationRepository.Verify(repo => repo.GetAll(groupId), Times.Once);
            mockGroupRepository.Verify(repo => repo.Get(groupId), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnGroup_WhenGroupdIdIsValid()
        {
            var groupId = Guid.NewGuid();
            var group = new Group { Id = groupId, Name = "Group A", Capacity = 100 };
            mockGroupRepository.Setup(repo => repo.Get(groupId)).ReturnsAsync(group);

            var result = await groupService.GetGroup(groupId);

            Assert.NotNull(result);
            Assert.Equal("Group A", result.Name);
            Assert.Equal(100, result.Capacity);

            mockGroupRepository.Verify(repo => repo.Get(groupId), Times.Once);
        }

        [Fact]
        public async Task Should_ReturnGroups_WhenGroupsExists()
        {
            var groups = new List<Group>
            {
                new Group { Id = Guid.NewGuid(), Name = "Group A", Capacity = 100 }
            };

            mockGroupRepository.Setup(repo => repo.GetAll()).ReturnsAsync(groups);

            var result = await groupService.GetAllGroups();

            Assert.NotNull(result);
            Assert.Equal("Group A", result.First().Name);
            Assert.Equal(100, result.First().Capacity);

            mockGroupRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Should_DeleteGroup_WhenGroupIdIsValid()
        {
            var groupId = Guid.NewGuid();
            var group = new Group { Id = groupId, Name = "Group A", Capacity = 100 };

            mockGroupRepository.Setup(repo => repo.Get(groupId)).ReturnsAsync(group);
            mockGroupRepository.Setup(repo => repo.Delete(group)).Returns(Task.CompletedTask);

            await groupService.DeleteGroup(groupId);

            mockGroupRepository.Verify(repo => repo.Get(groupId), Times.Once);
            mockGroupRepository.Verify(repo => repo.Delete(group), Times.Once);
        }
    }
}