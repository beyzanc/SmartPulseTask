namespace SmartPulseTask.Entities
{
    public class TransactionResult
    {
        public required string ContractName { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public decimal TotalTransactionQuantity { get; set; }
        public decimal WeightedAveragePrice { get; set; }
    }
}
