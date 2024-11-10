namespace SmartPulseTask.Helpers
{
    public static class ContractDateParser
    {
        public static DateTime ParseContractDate(string contractName)
        {
            if (contractName.Length >= 10 && contractName.StartsWith("PH"))
            {
                int year = int.Parse("20" + contractName.Substring(2, 2));
                int month = int.Parse(contractName.Substring(4, 2));
                int day = int.Parse(contractName.Substring(6, 2));
                int hour = int.Parse(contractName.Substring(8, 2));
                return new DateTime(year, month, day, hour, 0, 0);
            }

            return DateTime.MinValue;
        }
    }
}