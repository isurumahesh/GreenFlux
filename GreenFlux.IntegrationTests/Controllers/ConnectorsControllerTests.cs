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
    public class ConnectorsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        private Guid chargeStationId;
        private int connectorId;

        public ConnectorsControllerTests(CustomWebApplicationFactory<Program> factory)
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
            var chargeStation = dbContext.ChargeStations.Include(a => a.Connectors).First();
            chargeStationId = chargeStation.Id;
            connectorId = chargeStation.Connectors.First().Id;
        }

        [Fact]
        public async Task Post_ReturnSuccess_WhenConnectorCreateModelIsValid()
        {
            var connectorCreateDTO = new ConnectorCreateDTO { Id = 3, MaxCurrent = 20 };

            var data = JsonConvert.SerializeObject(connectorCreateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/chargestations/{chargeStationId}/connectors", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnBadRequest_WhenConnectorCreateModelIsInValid()
        {
            var connectorCreateDTO = new ConnectorCreateDTO { Id = 8, MaxCurrent = 20 };

            var data = JsonConvert.SerializeObject(connectorCreateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/chargestations/{chargeStationId}/connectors", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_ReturnConnectorList_FromDatabase()
        {
            var response = await _httpClient.GetAsync($"api/chargestations/{chargeStationId}/connectors");

            var result = await response.Content.ReadFromJsonAsync<List<ConnectorDTO>>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Put_ReturnBadRequest_WhenConnectorUpdateModelIsInValid()
        {
            var connectorUpdateDTO = new ConnectorUpdateDTO { MaxCurrent = 0 };

            var data = JsonConvert.SerializeObject(connectorUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ReturnNoContent_WhenConnectorUpdateModelIsValid()
        {
            var connectorUpdateDTO = new ConnectorUpdateDTO { MaxCurrent = 30 };

            var data = JsonConvert.SerializeObject(connectorUpdateDTO);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Patch_ReturnBadRequest_WhenPatchDocumentIsInValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/maxcurrent", value = "" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Patch_ReturnNoContent_WhenPatchDocumentIsValid()
        {
            var patchDocument = new[] { new { op = "replace", path = "/maxcurrent", value = "30" } };

            var data = JsonConvert.SerializeObject(patchDocument);
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Get_ReturnNotFound_WhenConnectorIdIsInValid()
        {
            var response = await _httpClient.GetAsync($"api/chargestations/{Guid.NewGuid()}/connectors/100");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnSuccess_WhenConnectorIsValid()
        {
            var response = await _httpClient.GetAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}");
            var result = await response.Content.ReadFromJsonAsync<ConnectorDTO>();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ReturnNotFound_WhenConnectorIdIsInValid()
        {
            var response = await _httpClient.DeleteAsync($"api/chargestations/{Guid.NewGuid()}/connectors/100");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnSuccess_WhenConnectorIsValid()
        {
            var response = await _httpClient.GetAsync($"api/chargestations/{chargeStationId}/connectors/{connectorId}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}