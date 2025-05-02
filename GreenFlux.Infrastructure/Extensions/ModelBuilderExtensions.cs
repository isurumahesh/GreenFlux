using GreenFlux.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var group1 = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Group A",
                Capacity = 500
            };

            var group2 = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Group B",
                Capacity = 300
            };

            var chargeStation1 = new ChargeStation
            {
                Id = Guid.NewGuid(),
                Name = "Charge Station 1",
                GroupId = group1.Id
            };

            var chargeStation2 = new ChargeStation
            {
                Id = Guid.NewGuid(),
                Name = "Charge Station 2",
                GroupId = group1.Id
            };

            var chargeStation3 = new ChargeStation
            {
                Id = Guid.NewGuid(),
                Name = "Charge Station 3",
                GroupId = group2.Id
            };

            var connector1 = new Connector
            {
                Id = 1,
                MaxCurrent = 100,
                ChargeStationId = chargeStation1.Id
            };

            var connector2 = new Connector
            {
                Id = 2,
                MaxCurrent = 80,
                ChargeStationId = chargeStation1.Id
            };

            var connector3 = new Connector
            {
                Id = 1,
                MaxCurrent = 120,
                ChargeStationId = chargeStation2.Id
            };

            var connector4 = new Connector
            {
                Id = 1,
                MaxCurrent = 60,
                ChargeStationId = chargeStation3.Id
            };

            var connector5 = new Connector
            {
                Id = 2,
                MaxCurrent = 90,
                ChargeStationId = chargeStation3.Id
            };

            modelBuilder.Entity<Group>().HasData(group1, group2);
            modelBuilder.Entity<ChargeStation>().HasData(chargeStation1, chargeStation2, chargeStation3);
            modelBuilder.Entity<Connector>().HasData(connector1, connector2, connector3, connector4, connector5);
        }
    }
}