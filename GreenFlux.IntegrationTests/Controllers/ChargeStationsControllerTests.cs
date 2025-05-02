using FluentAssertions;
using GreenFlux.Application.DTOs;
using GreenFlux.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace GreenFlux.IntegrationTests.Controllers
{
    public class ChargeStationsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        private Guid chargeStationId;
        private Guid groupId;

        public ChargeStationsControllerTests(CustomWebApplicationFactory<Program> factory)
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
            var chargeStation = dbContext.ChargeStations.Include(a => a.Group).First();
            chargeStationId = chargeStation.Id;
            groupId = chargeStation.Group.Id;
        }

        [Fact]
        public async Task Post_ReturnSuccess_WhenChargeStationCreateModelIsValid()
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

            var data = JsonConvert.SerializeObject(chargeStation);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/groups/{groupId}/chargestations", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnBadRequest_WhenChargeStationCreateModelIsInValid()
        {
            var chargeStation = new ChargeStationCreateDTO
            {
                Name = "Charge Station 1",
            };

            var data = JsonConvert.SerializeObject(chargeStation);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/groups/{groupId}/chargestations", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_ReturnChargeStationList_FromDatabase()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}/chargestations");

            var result = await response.Content.ReadFromJsonAsync<List<ChargeStationDTO>>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task Put_ReturnBadRequest_WhenChargeStationUpdateModelIsInValid()
        {
            var chargeStationUpdateDTO = new ChargeStationUpdateDTO { Name = "" };

            var data = JsonConvert.SerializeObject(chargeStationUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/groups/{groupId}/chargestations/{chargeStationId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ReturnNoContent_WhenChargeStationUpdateModelIsValid()
        {
            var chargeStationUpdateDTO = new ChargeStationUpdateDTO { Name = "ChargeStation A" };

            var data = JsonConvert.SerializeObject(chargeStationUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/groups/{groupId}/chargestations/{chargeStationId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Patch_ReturnBadRequest_WhenPatchDocumentIsInValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/name", value = "" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/groups/{groupId}/chargestations/{chargeStationId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Patch_ReturnNoContent_WhenPatchDocumentIsValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/name", value = "UpdatedName" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/groups/{groupId}/chargestations/{chargeStationId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Get_ReturnNotFound_WhenChargeStationIdIsInValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}/chargestations/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnSuccess_WhenChargeStationIsValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}/chargestations/{chargeStationId}");
            var result = await response.Content.ReadFromJsonAsync<ChargeStationDTO>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ReturnNotFound_WhenChargeStationIdIsInValid()
        {
            var response = await _httpClient.DeleteAsync($"api/groups/{groupId}/chargestations/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnSuccess_WhenChargeStationIsValid()
        {
            var response = await _httpClient.GetAsync($"api/groups/{groupId}/chargestations/{chargeStationId}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}