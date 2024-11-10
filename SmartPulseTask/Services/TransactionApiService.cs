using Newtonsoft.Json;
using SmartPulseTask.Entities;
using SmartPulseTask.Services.Interfaces;
using System.Text;

namespace SmartPulseTask.Services
{
    public class TransactionApiService : ITransactionApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TransactionApiService> _logger;
        private readonly string _username;
        private readonly string _password;
        private readonly string _baseUrl;
        private readonly string _transactionHistoryUrl;

        public TransactionApiService(HttpClient httpClient, IConfiguration configuration, ILogger<TransactionApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _username = configuration["ApiSettings:Username"];
            _password = configuration["ApiSettings:Password"];
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _transactionHistoryUrl = configuration["ApiSettings:TransactionHistoryUrl"];
        }

        public async Task<List<TransactionHistoryGipDataDto>> GetTransactionDataAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var startDateFormatted = startDate.ToString("yyyy-MM-ddTHH:mm:ss+03:00");
                var endDateFormatted = endDate.ToString("yyyy-MM-ddTHH:mm:ss+03:00");

                var tgt = await GetTgtAsync(_username, _password);
                if (string.IsNullOrEmpty(tgt))
                {
                    throw new Exception("Kimlik doğrulanamadı.");
                }

                return await FetchTransactionHistory(tgt, startDateFormatted, endDateFormatted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem verileri alınırken bir hata oluştu.");
                throw new ApplicationException("İşlem verileri alınamadı.");
            }
        }

        private async Task<string> GetTgtAsync(string username, string password)
        {
            try
            {
                var requestContent = new StringContent($"username={username}&password={password}", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await _httpClient.PostAsync(_baseUrl, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var tgtUrl = response.Headers.Location?.ToString();
                    if (!string.IsNullOrEmpty(tgtUrl))
                    {
                        return tgtUrl.Split('/').Last();
                    }
                    throw new Exception("Kimlik doğrulanırken bir hata oluştu.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Kimlik doğrulama hatası. Durum Kodu: {response.StatusCode}, İçerik: {errorContent}");
                    throw new Exception("Kimlik doğrulanamadı. Lütfen bağlantı ayarlarını kontrol edin.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kimlik doğrulanırken bir hata oluştu.");
                throw new ApplicationException("Kimlik doğrulama hatası. Lütfen bağlantınızı kontrol edin.");
            }
        }

        private async Task<List<TransactionHistoryGipDataDto>> FetchTransactionHistory(string tgt, string startDate, string endDate)
        {
            try
            {
                var jsonContent = new
                {
                    startDate,
                    endDate
                };
                var content = new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("TGT", tgt);
                var response = await _httpClient.PostAsync(_transactionHistoryUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"API isteği başarısız oldu. Durum Kodu: {response.StatusCode}: {errorContent}");
                    throw new Exception("İşlem geçmişi alınamadı.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var transactionHistoryResponse = JsonConvert.DeserializeObject<TransactionHistoryResponse>(responseContent);

                if (transactionHistoryResponse?.Items == null || !transactionHistoryResponse.Items.Any())
                {
                    throw new Exception("İşlem geçmişi verisi bulunamadı.");
                }

                return transactionHistoryResponse.Items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem geçmişi verisi alınırken bir hata oluştu.");
                throw new ApplicationException("İşlem geçmişi verisi alınamadı. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}