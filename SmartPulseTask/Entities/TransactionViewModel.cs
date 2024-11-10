namespace SmartPulseTask.Entities
{
    public class TransactionViewModel
    {
        public List<TransactionResult> Transactions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}