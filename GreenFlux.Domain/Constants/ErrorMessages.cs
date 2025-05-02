using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenFlux.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string CapacityTooLow = "Group Amps can't be lower than the sum of the max current of the connectors that belong to group.";
        public const string MaxCurrentIsHigh = "Max current in amps of all connectore are greater than group capacity";
        public const string ConnectorCount = "Charge station can only have connectors from 1 to 5";
    }
}
