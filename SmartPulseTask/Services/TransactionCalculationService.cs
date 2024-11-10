using SmartPulseTask.Helpers;
using SmartPulseTask.Entities;
using SmartPulseTask.Services.Interfaces;

namespace SmartPulseTask.Services
{
    public class TransactionCalculationService : ITransactionCalculationService
    {
        private readonly ITransactionApiService _apiService;

        public TransactionCalculationService(ITransactionApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<TransactionResult>> CalculateTransactionResultsAsync(DateTime startDate, DateTime endDate, string sortColumn = "DateTime", string sortDirection = "asc")
        {
            try
            {
                var transactionData = await _apiService.GetTransactionDataAsync(startDate, endDate);

                if (transactionData == null || !transactionData.Any())
                {
                    throw new ApplicationException("İlgili tarih aralığında işlem verisi bulunamadı.");
                }

                var groupedData = transactionData
                    .GroupBy(x => x.ContractName)
                    .Select(g => new TransactionResult
                    {
                        ContractName = g.Key,
                        DateTime = ContractDateParser.ParseContractDate(g.Key),
                        TotalTransactionAmount = g.Sum(x => (decimal)(x.Price * x.Quantity) / 10),
                        TotalTransactionQuantity = g.Sum(x => (decimal)x.Quantity / 10),
                        WeightedAveragePrice = g.Sum(x => (decimal)(x.Price * x.Quantity) / 10) / g.Sum(x => (decimal)x.Quantity / 10)
                    })
                    .ToList();

                return SortData(groupedData, sortColumn, sortDirection);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException("İşlem sonuçları hesaplanamadı: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("İşlem sonuçlarını hesaplama sırasında bir hata oluştu.", ex);
            }
        }

        private List<TransactionResult> SortData(List<TransactionResult> data, string sortColumn, string sortDirection)
        {
            try
            {
                return sortColumn switch
                {
                    "ContractName" => (sortDirection == "asc" ? data.OrderBy(x => x.ContractName) : data.OrderByDescending(x => x.ContractName)).ToList(),
                    "TotalTransactionAmount" => (sortDirection == "asc" ? data.OrderBy(x => x.TotalTransactionAmount) : data.OrderByDescending(x => x.TotalTransactionAmount)).ToList(),
                    "TotalTransactionQuantity" => (sortDirection == "asc" ? data.OrderBy(x => x.TotalTransactionQuantity) : data.OrderByDescending(x => x.TotalTransactionQuantity)).ToList(),
                    "WeightedAveragePrice" => (sortDirection == "asc" ? data.OrderBy(x => x.WeightedAveragePrice) : data.OrderByDescending(x => x.WeightedAveragePrice)).ToList(),
                    _ => (sortDirection == "asc" ? data.OrderBy(x => x.DateTime) : data.OrderByDescending(x => x.DateTime)).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Veri sıralaması sırasında bir hata oluştu.", ex);
            }
        }
    }
}
