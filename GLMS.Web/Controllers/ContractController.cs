using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ContractController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Contract — with Search/Filter
        public async Task<IActionResult> Index(DateTime? startDate,
            DateTime? endDate, ContractStatus? status)
        {
            // Store filter values for the view
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;
            ViewBag.Statuses = Enum.GetValues(typeof(ContractStatus));

            // LINQ query with optional filters
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (startDate.HasValue)
                contracts = contracts.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                contracts = contracts.Where(c => c.EndDate <= endDate.Value);

            if (status.HasValue)
                contracts = contracts.Where(c => c.Status == status.Value);

            return View(await contracts.ToListAsync());
        }

        // GET: Contract/Create
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "ClientId", "Name");
            return View();
        }

        // POST: Contract/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? pdfFile)
        {
            // Handle PDF upload
            if (pdfFile != null)
            {
                // Validate file is PDF
                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("SignedAgreementPath",
                        "Only PDF files are allowed.");
                }
                else
                {
                    // Save to wwwroot/uploads/contracts/
                    var uploadsFolder = Path.Combine(
                        _env.WebRootPath, "uploads", "contracts");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(stream);
                    }

                    contract.SignedAgreementPath = $"/uploads/contracts/{uniqueFileName}";
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(
                _context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();
            ViewBag.Clients = new SelectList(
                _context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // POST: Contract/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract,
            IFormFile? pdfFile)
        {
            if (id != contract.ContractId) return NotFound();

            // Handle new PDF upload on edit
            if (pdfFile != null)
            {
                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("SignedAgreementPath",
                        "Only PDF files are allowed.");
                }
                else
                {
                    var uploadsFolder = Path.Combine(
                        _env.WebRootPath, "uploads", "contracts");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(stream);
                    }

                    contract.SignedAgreementPath = $"/uploads/contracts/{uniqueFileName}";
                }
            }

            if (ModelState.IsValid)
            {
                _context.Update(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(
                _context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        // GET: Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null) _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}