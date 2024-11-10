using SmartPulseTask.Entities;

namespace SmartPulseTask.Services.Interfaces
{
    public interface ITransactionCalculationService
    {
        Task<List<TransactionResult>> CalculateTransactionResultsAsync(DateTime startDate, DateTime endDate, string sortColumn = "DateTime", string sortDirection = "asc");
    }
}