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

namespace BudgetiFi.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class BudgetCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string _userId;
        [TempData]
        public string StatusMessage { get; set; }

        public BudgetCategoriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/BudgetCategories
        public async Task<IActionResult> Index()
        {
            _userId = GetCurrentUserId();
            var applicationDbContext = _context.BudgetCategories.Include(b => b.User);
            return View(await applicationDbContext.Where(c=> c.UserId == _userId)
                .ToListAsync());
        }

        // GET: Customer/BudgetCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var budgetCategory = await _context.BudgetCategories
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            if (budgetCategory == null)
            {
                return NotFound();
            }

            return View(budgetCategory);
        }

        // GET: Customer/BudgetCategories/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Customer/BudgetCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId")] BudgetCategory budgetCategory)
        {
            _userId = GetCurrentUserId();

            budgetCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(budgetCategory);
            if (ModelState.IsValid)
            {

                //Checks if the budget type has already been added by user
                var checksIfCategoryExists = _context.BudgetCategories.FirstOrDefault(c => c.Name == budgetCategory.Name
                && c.UserId == _userId && c.Name.ToLower().Contains(budgetCategory.Name.ToLower()));




                if (checksIfCategoryExists != null)
                {
                    //Error
                    StatusMessage = "Error : Category exists under " + checksIfCategoryExists.Name + " category. Please use another name.";
                }
                else
                {
                    _context.Add(budgetCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }



            }
            ViewBag.StatusMessage = StatusMessage;
           
            return View(budgetCategory);
        }

        // GET: Customer/BudgetCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _userId = GetCurrentUserId();



            if (id == null)
            {
                return NotFound();
            }

            var budgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (budgetCategory == null)
            {
                return NotFound();
            }
            
            return View(budgetCategory);
        }

        // POST: Customer/BudgetCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] BudgetCategory budgetCategory)
        {
            _userId = GetCurrentUserId();
            budgetCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(budgetCategory);
            if (id != budgetCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Checks if the budget type has already been added by user
                var checksIfCategoryExists = _context.BudgetCategories.FirstOrDefault(c => c.Name == budgetCategory.Name
                && c.UserId == _userId && c.Name.ToLower().Contains(budgetCategory.Name.ToLower()));

                if (checksIfCategoryExists != null)
                {
                    //Error
                    StatusMessage = "Error : Category exists under " + checksIfCategoryExists.Name + " category. Please use another name.";
                }
                else
                {
                    try
                    {
                        _context.Update(budgetCategory);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BudgetCategoryExists(budgetCategory.Id))
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
            ViewBag.StatusMessage = StatusMessage;
            return View(budgetCategory);
        }

        // GET: Customer/BudgetCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var budgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (budgetCategory == null)
            {
                return NotFound();
            }

            return View(budgetCategory);
        }

        // POST: Customer/BudgetCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _userId = GetCurrentUserId();
            var budgetCategory = await _context.BudgetCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            _context.BudgetCategories.Remove(budgetCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetCategoryExists(int id)
        {
            return _context.BudgetCategories.Any(e => e.Id == id);
        }

        protected string GetCurrentUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }

    }
}
