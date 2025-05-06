namespace GreenFlux.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string CapacityTooLow = "Group capacity can't be lower than the sum of the max current of the connectors that belong to group.";
        public const string MaxCurrentIsHigh = "Max current in Amps of all connectors are greater than group capacity";
        public const string ConnectorCount = "Charge station can only have connectors from 1 to 5";
        public const string ConnectorId = "There is an already connector with given Id";
    }
}