using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetiFi.Data;
using BudgetiFi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BudgetiFi.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class DebtDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string _userId;
        [TempData]
        public string StatusMessage { get; set; }

        public DebtDetailsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        // GET: Customer/DebtDetails/Profile

        public IActionResult Profile()
        {
            _userId = GetCurrentUserId();
            int numberOfCreditCard = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "Credit Card".ToLower());

            double totalCreditCard = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "Credit Card".ToLower())
                .Sum(e => e.Balance);



            int numberOfStudentLoan = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "Student Loan".ToLower());

            double totalOfStudentLoan = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "Student Loan".ToLower())
                .Sum(e => e.Balance);

            int numberOfMortgage = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "mortgage".ToLower()
                || c.DebtCategory.Name.ToLower() == "Home Loan".ToLower());

            double totalMortgage = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "mortgage".ToLower()
                || d.DebtCategory.Name.ToLower() == "Home Loan".ToLower())
                .Sum(e => e.Balance);


            int autoLoan = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "auto loan".ToLower() 
                || c.DebtCategory.Name.ToLower() == "car loan".ToLower());

            double totalAutoLoan = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "auto loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "car loan".ToLower())
                .Sum(e => e.Balance);

            int numberOfAutoLoan = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "personal loan".ToLower()
                || c.DebtCategory.Name.ToLower() == "consolidation loan".ToLower()
                || c.DebtCategory.Name.ToLower() == "other loan".ToLower());

            double totalOtherLoan = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "personal loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "consolidation loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "other loan".ToLower())
                .Sum(e => e.Balance);

            int numberOfMedicalBills = _context.DebtDetails.Include(u => u.User).Where(x => x.UserId == _userId)
                .Count(c => c.DebtCategory.Name.ToLower() == "medical loan".ToLower()
                || c.DebtCategory.Name.ToLower() == "doctor loan".ToLower()
                || c.DebtCategory.Name.ToLower() == "hospital loan".ToLower()
                || c.DebtCategory.Name.ToLower() == "medical bill".ToLower()
                || c.DebtCategory.Name.ToLower() == "medical bills".ToLower());

            double totalMedicalBills = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.DebtCategory.Name.ToLower() == "medical loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "doctor loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "hospital loan".ToLower()
                || d.DebtCategory.Name.ToLower() == "medical bill".ToLower()
                || d.DebtCategory.Name.ToLower() == "medical bills".ToLower())
                .Sum(e => e.Balance);

            double totalDebt = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId)
                .Sum(e => e.Balance);

            ViewBag.NumberOfMortgage = numberOfMortgage;
            ViewBag.NumberOfCreditCard = numberOfCreditCard;
            ViewBag.NumberOfStudentLoan = numberOfStudentLoan;
            ViewBag.NumberOfAutoLoan = autoLoan;
            ViewBag.NumberOfOtherLoan = numberOfAutoLoan;
            ViewBag.NumberOfMedicalBills = numberOfMedicalBills;
            ViewBag.TotalCreditCard = totalCreditCard;
            ViewBag.TotalOfStudentLoan = totalOfStudentLoan;
            ViewBag.TotalMortgage = totalMortgage;
            ViewBag.TotalAutoLoan = totalAutoLoan;
            ViewBag.TotalOtherLoan = totalOtherLoan;
            ViewBag.TotalMedicalBills = totalMedicalBills;
            ViewBag.TotalDebt = totalDebt;

            if(HttpContext.Session.GetString("Username") != null)
            {

            }
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        // GET: Customer/DebtDetails
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "debt_name" : "";
            ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            _userId = GetCurrentUserId();
           IQueryable<DebtDetail> applicationDbContext = _context.DebtDetails.Include(d => d.CreditorCategory).Include(d => d.DebtCategory)
                .Include(d => d.User).Where(x=>x.UserId == _userId);
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(d => d.Name.ToLower().Contains(searchString.ToLower())
                || d.DebtCategory.Name.ToLower().Contains(searchString.ToLower()) 
                || d.CreditorCategory.Name.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "debt_name":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    applicationDbContext = applicationDbContext.OrderBy(s => s.PaymentDate);
                    break;
                case "date_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.PaymentDate);
                    break;
                default:
                    applicationDbContext = applicationDbContext.OrderBy(s => s.Name);
                    break;

            }

            double creditAvailable = _context.DebtDetails.Include(d => d.User).Where(e => e.Ignore == true)
                .Where(d => d.UserId == _userId)
                .Sum(e => e.AvailableCredit);

            double monthlyPayment = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId)
                .Sum(e => e.MinimumPayment);

            double totalDebt = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId)
                .Sum(e => e.Balance);

            double totalReccurringDebt = _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.Ignore == true)
                .Sum(e => e.Balance);

            double totalCreditLimit= _context.DebtDetails.Include(d => d.User)
                .Where(d => d.UserId == _userId && d.Ignore == true)
                .Sum(e => e.CreditLimit);
            ViewBag.TotalCreditLimit = totalCreditLimit;
            ViewBag.CreditAvailable = creditAvailable > 0 ? creditAvailable : 0;
            ViewBag.MonthlyPayment = monthlyPayment > 0 ? monthlyPayment : 0;
            ViewBag.TotalDebt = totalDebt > 0 ? totalDebt : 0;
            ViewBag.PercentageAvailable = totalReccurringDebt > 0 ?((double)(creditAvailable / totalCreditLimit)): 0;
          


            int pageSize = 3;
            return View(await PaginatedList<DebtDetail>.CreateAsync(applicationDbContext.AsNoTracking().Where(d => d.UserId == _userId), pageNumber ?? 1, pageSize));
        }

        // GET: Customer/DebtDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtDetail = await _context.DebtDetails
                .Include(d => d.CreditorCategory)
                .Include(d => d.DebtCategory)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (debtDetail == null)
            {
                return NotFound();
            }

            return View(debtDetail);
        }

        // GET: Customer/DebtDetails/Create
        public IActionResult Create()
        {
            _userId = GetCurrentUserId();
            ViewData["CreditorCategoryId"] = new SelectList(_context.CreditorCategories.Where(c=>c.UserId == _userId), "Id", "Name");
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories.Where(c => c.UserId == _userId), "Id", "Name");
            return View();
        }

        // POST: Customer/DebtDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PaymentDate,CreditLimit,Balance,MinimumPayment,DebtCategoryId,CreditorCategoryId,Ignore")] DebtDetail debtDetail)
        {
            _userId = GetCurrentUserId();

            debtDetail.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(debtDetail);
            if (ModelState.IsValid)
            {
                
                //Checks if the debt type has already been added by user
                var checksIfdebtdetailExists = _context.DebtDetails.Include(c => c.DebtCategory).Include(c=>c.CreditorCategory)
                    .Where(s => s.Name == debtDetail.Name && s.DebtCategory.Id == debtDetail.DebtCategoryId
                    && s.CreditorCategory.Id == debtDetail.CreditorCategoryId
                    && s.UserId == _userId && s.Name.ToLower().Contains(debtDetail.Name.ToLower()));

                if (checksIfdebtdetailExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Creditor exists under " + checksIfdebtdetailExists.First().DebtCategory.Name
                        + " category. Please use another name.";
                }
                else
                {

                    ViewBag.Available = debtDetail.AvailableCredit;
                   
                    _context.Add(debtDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                
            }
            ViewData["CreditorCategoryId"] = new SelectList(_context.CreditorCategories.Where(c => c.UserId == _userId), "Id", "Name");
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories.Where(c => c.UserId == _userId), "Id", "Name");
            ViewBag.StatusMessage = StatusMessage;
            return View(debtDetail);
        }

        // GET: Customer/DebtDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var debtDetail = await _context.DebtDetails.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (debtDetail == null)
            {
                return NotFound();
            }
            ViewData["CreditorCategoryId"] = new SelectList(_context.CreditorCategories
                .Where(c => c.UserId == _userId), 
                "Id", "Name", debtDetail.CreditorCategoryId);
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories
                .Where(c => c.UserId == _userId), 
                "Id", "Name", debtDetail.DebtCategoryId);
           
            return View(debtDetail);
        }

        // POST: Customer/DebtDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PaymentDate,CreditLimit,Balance,MinimumPayment,DebtCategoryId,CreditorCategoryId,Ignore")] DebtDetail debtDetail)
        {
            _userId = GetCurrentUserId();
            debtDetail.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(debtDetail);
            if (id != debtDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Checks if the debt detail name has already been added by user
                var checksIfDetailExists = _context.DebtDetails.Include(c => c.DebtCategory).Include(c=>c.CreditorCategory)
                    .Where(s => s.Name == debtDetail.Name && s.DebtCategory.Id != debtDetail.DebtCategoryId 
                    || s.CreditorCategory.Id != debtDetail.CreditorCategoryId
                    && s.UserId == _userId && s.Name.ToLower().Contains(debtDetail.Name.ToLower()));

               //var x = _context.DebtDetails.Where(s => s.Id == id && s.AvailableCredit == debtDetail.AvailableCredit);
                
                if (checksIfDetailExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Account exists with " + checksIfDetailExists.First().CreditorCategory.Name + " . Please use another name.";
                }
                else
                {
                    try
                    {
                        _context.Update(debtDetail);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DebtDetailExists(debtDetail.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));

                }
                
            }
            ViewData["CreditorCategoryId"] = new SelectList(_context.CreditorCategories
                .Where(c => c.UserId == _userId), "Id", "Name", debtDetail.CreditorCategoryId);
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories
                .Where(c => c.UserId == _userId), "Id", "Name", debtDetail.DebtCategoryId);
            ViewBag.StatusMessage = StatusMessage;
            return View(debtDetail);
        }

        // GET: Customer/DebtDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var debtDetail = await _context.DebtDetails
                .Include(d => d.CreditorCategory)
                .Include(d => d.DebtCategory)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            if (debtDetail == null)
            {
                return NotFound();
            }
            ViewData["CreditorCategoryId"] = new SelectList(_context.CreditorCategories
                .Where(c => c.UserId == _userId),
                "Id", "Name", debtDetail.CreditorCategoryId);
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories
                .Where(c => c.UserId == _userId),
                "Id", "Name", debtDetail.DebtCategoryId);

            return View(debtDetail);
        }

        // POST: Customer/DebtDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _userId = GetCurrentUserId();
            var debtDetail = await _context.DebtDetails.SingleOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            _context.DebtDetails.Remove(debtDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DebtDetailExists(int id)
        {
            return _context.DebtDetails.Any(e => e.Id == id);
        }
        protected string GetCurrentUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }
    }
}
