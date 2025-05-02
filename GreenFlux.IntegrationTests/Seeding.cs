using GreenFlux.Domain.Entities;
using GreenFlux.Infrastructure.Data;

namespace GreenFlux.IntegrationTests
{
    public class Seeding
    {
        public static void InitializeTestDb(GreenFluxDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Groups.Any()) return;

            var group1Id = Guid.NewGuid();
            var chargeStation1Id = Guid.NewGuid();

            var group1 = new Group
            {
                Id = group1Id,
                Name = "Group 1",
                Capacity = 120
            };

            var chargeStation1 = new ChargeStation
            {
                Id = chargeStation1Id,
                Name = "Charge Station 1",
                GroupId = group1Id,
                Group = group1
            };

            var connectors1 = new List<Connector>
        {
            new Connector
            {
                Id = 1,
                MaxCurrent = 20,
                ChargeStationId = chargeStation1Id,
                ChargeStation = chargeStation1
            },
            new Connector
            {
                Id = 2,
                MaxCurrent = 25,
                ChargeStationId = chargeStation1Id,
                ChargeStation = chargeStation1
            }
        };

            var group2Id = Guid.NewGuid();
            var chargeStation2Id = Guid.NewGuid();

            var group2 = new Group
            {
                Id = group2Id,
                Name = "Group 2",
                Capacity = 150
            };

            var chargeStation2 = new ChargeStation
            {
                Id = chargeStation2Id,
                Name = "Charge Station 2",
                GroupId = group2Id,
                Group = group2
            };

            var connectors2 = new List<Connector>
        {
            new Connector
            {
                Id = 3,
                MaxCurrent = 30,
                ChargeStationId = chargeStation2Id,
                ChargeStation = chargeStation2
            },
            new Connector
            {
                Id = 4,
                MaxCurrent = 35,
                ChargeStationId = chargeStation2Id,
                ChargeStation = chargeStation2
            }
        };

            context.Groups.AddRange(group1, group2);
            context.ChargeStations.AddRange(chargeStation1, chargeStation2);
            context.Connectors.AddRange(connectors1);
            context.Connectors.AddRange(connectors2);

            context.SaveChanges();
        }
    }
}