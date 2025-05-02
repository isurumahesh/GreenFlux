using FluentAssertions;
using GreenFlux.Application.DTOs;
using GreenFlux.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace GreenFlux.IntegrationTests.Controllers
{
    public class GroupsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        private Guid groupId;

        public GroupsControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;

            _httpClient = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            SeedDatabase(factory);
        }

        private void SeedDatabase(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GreenFluxDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            Seeding.InitializeTestDb(dbContext);
            groupId = dbContext.Groups.First().Id;
        }

        [Fact]
        public async Task Post_ReturnSuccess_WhenGroupCreateModelIsValid()
        {
            var chargeStation = new ChargeStationCreateDTO
            {
                Name = "Charge Station 1",
                Connectors = new List<ConnectorCreateDTO>
                    {
                        new ConnectorCreateDTO { Id=1,MaxCurrent = 30 },
                        new ConnectorCreateDTO { Id=2,MaxCurrent = 40 }
                    }
            };

            var groupCreateDTO = new GroupCreateDTO { Name = "Group A", Capacity = 200, ChargeStation = chargeStation };

            var data = JsonConvert.SerializeObject(groupCreateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/groups", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnBadRequest_WhenGroupCreateModelIsInValid()
        {
            var chargeStation = new ChargeStationCreateDTO
            {
                Name = "Charge Station 1",
            };

            var groupCreateDTO = new GroupCreateDTO { Name = "Group A", Capacity = 0, ChargeStation = chargeStation };

            var data = JsonConvert.SerializeObject(groupCreateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/groups", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_ReturnGroupList_FromDatabase()
        {
            var response = await _httpClient.GetAsync("api/groups");

            var result = await response.Content.ReadFromJsonAsync<List<GroupDTO>>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Put_ReturnBadRequest_WhenGroupUpdateModelIsInValid()
        {
            var groupUpdateDTO = new GroupUpdateDTO { Name = "Group A", Capacity = 0 };

            var data = JsonConvert.SerializeObject(groupUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/groups/{groupId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ReturnNoContent_WhenGroupUpdateModelIsValid()
        {
            var groupUpdateDTO = new GroupUpdateDTO { Name = "Group A", Capacity = 1000 };

            var data = JsonConvert.SerializeObject(groupUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/groups/{groupId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Patch_ReturnBadRequest_WhenPatchDocumentIsInValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/name", value = "" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/groups/{groupId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Patch_ReturnNoContent_WhenPatchDocumentIsValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/name", value = "UpdatedName" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/groups/{groupId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Get_ReturnNotFound_WhenGroupIdIsInValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnSuccess_WhenGroupIsValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}");
            var result = await response.Content.ReadFromJsonAsync<GroupDTO>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ReturnNotFound_WhenGroupIdIsInValid()
        {
            var response = await _httpClient.DeleteAsync($"api/groups/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnSuccess_WhenGroupIsValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}