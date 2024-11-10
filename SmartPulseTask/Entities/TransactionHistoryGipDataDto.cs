namespace SmartPulseTask.Entities
{
    public class TransactionHistoryResponse
    {
        public List<TransactionHistoryGipDataDto> Items { get; set; }
    }

    public class TransactionHistoryGipDataDto
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string ContractName { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}