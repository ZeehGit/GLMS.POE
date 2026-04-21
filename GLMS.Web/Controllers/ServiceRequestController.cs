using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICurrencyService _currencyService;

        public ServiceRequestController(AppDbContext context,
            ICurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        // GET: ServiceRequest
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c!.Client)
                .ToListAsync();
            return View(requests);
        }

        // GET: ServiceRequest/Create
        public async Task<IActionResult> Create()
        {
            // Only show Active contracts in dropdown
            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status == ContractStatus.Active)
                .ToListAsync();

            ViewBag.Contracts = new SelectList(
                activeContracts, "ContractId", "ServiceLevel");

            // Get current exchange rate to display
            try
            {
                var rate = await _currencyService.GetUSDToZARRateAsync();
                ViewBag.CurrentRate = rate;
            }
            catch
            {
                ViewBag.CurrentRate = 0;
                ViewBag.RateError = "Could not fetch exchange rate. Please try again.";
            }

            return View();
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            // Workflow validation — block Expired or OnHold contracts
            var contract = await _context.Contracts
                .FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Contract not found.");
            }
            else if (contract.Status == ContractStatus.Expired ||
                     contract.Status == ContractStatus.OnHold)
            {
                ModelState.AddModelError("",
                    $"Cannot create a Service Request for a contract " +
                    $"with status '{contract.Status}'. " +
                    $"Only Active contracts are allowed.");
            }

            if (ModelState.IsValid)
            {
                // Fetch live exchange rate and calculate ZAR
                var rate = await _currencyService.GetUSDToZARRateAsync();
                serviceRequest.ExchangeRateUsed = rate;
                serviceRequest.CostZAR = _currencyService
                    .ConvertUSDToZAR(serviceRequest.CostUSD, rate);

                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var activeContracts = await _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status == ContractStatus.Active)
                .ToListAsync();

            ViewBag.Contracts = new SelectList(
                activeContracts, "ContractId", "ServiceLevel",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // GET: ServiceRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);
            if (request == null) return NotFound();
            return View(request);
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request != null) _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}