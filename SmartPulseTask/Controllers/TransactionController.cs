using Microsoft.AspNetCore.Mvc;
using SmartPulseTask.Entities;
using SmartPulseTask.Services.Interfaces;

namespace SmartPulseTask.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionCalculationService _transactionCalculationService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionCalculationService transactionCalculationService, ILogger<TransactionController> logger)
        {
            _transactionCalculationService = transactionCalculationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string sortColumn = "DateTime", string sortDirection = "asc")
        {
            try
            {
                startDate ??= DateTime.Today;
                endDate ??= DateTime.Today;

                var groupedData = await _transactionCalculationService.CalculateTransactionResultsAsync(startDate.Value, endDate.Value, sortColumn, sortDirection);

                var viewModel = new TransactionViewModel
                {
                    Transactions = groupedData,
                    StartDate = startDate.Value.ToString("yyyy-MM-dd"),
                    EndDate = endDate.Value.ToString("yyyy-MM-dd"),
                    SortColumn = sortColumn,
                    SortDirection = sortDirection
                };

                return View(viewModel);
            }
            catch (ApplicationException ex)
            {
                TempData["ErrorMessage"] = ex.Message; 
                return RedirectToAction("Error", "Home");
            }
        }

    }
}