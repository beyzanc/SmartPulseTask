using SmartPulseTask.Entities;

namespace SmartPulseTask.Services.Interfaces
{
    public interface ITransactionApiService
    {
        Task<List<TransactionHistoryGipDataDto>> GetTransactionDataAsync(DateTime startDate, DateTime endDate);
    }
}
