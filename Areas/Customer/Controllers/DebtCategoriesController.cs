using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetiFi.Data;
using BudgetiFi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BudgetiFi.Areas.Customer.Controllers
{
   
    [Area("Customer")]
    [Authorize]
    public class DebtCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private string _userId;

        [TempData]
        public string StatusMessage { get; set; }

        public DebtCategoriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/DebtCategories
        public async Task<IActionResult> Index()
        {
            _userId = GetCurrentUserId();
            var applicationDbContext = _context.DebtCategories.Include(d => d.User);
            return View(await applicationDbContext.Where(c=> c.UserId == _userId).ToListAsync());
        }

        // GET: Customer/DebtCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var userid = _userManager.GetUserId(HttpContext.User);

            var debtCategory = await _context.DebtCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (debtCategory == null)
            {
                return NotFound();
            }

            return View(debtCategory);
        }

        // GET: Customer/DebtCategories/Create
        public IActionResult Create()
        {
            _userId = GetCurrentUserId();
            return View();
        }

        // POST: Customer/DebtCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId")] DebtCategory debtCategory)
        {
            _userId = GetCurrentUserId();

            debtCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(debtCategory);
            if (ModelState.IsValid)
            {

                //Checks if the debt type has already been added by user
                var checksIfCategoryExists = _context.DebtCategories.FirstOrDefault(c => c.Name == debtCategory.Name);

                var exits = _context.DebtCategories.Where(e => e.Name.ToLower().Contains(debtCategory.Name.ToLower()));


                if (checksIfCategoryExists != null || exits.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Category exists under " + checksIfCategoryExists.Name + " category. Please use another name.";
                }
                else
                {
                    _context.Add(debtCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }


              
            }

            ViewBag.StatusMessage = StatusMessage;
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", debtCategory.UserId);
            return View(debtCategory);
        }

        // GET: Customer/DebtCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _userId = GetCurrentUserId();

         

            if (id == null)
            {
                return NotFound();
            }

            var debtCategory = await _context.DebtCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (debtCategory == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", debtCategory.UserId);
            return View(debtCategory);
        }

        // POST: Customer/DebtCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] DebtCategory debtCategory)
        {
            _userId = GetCurrentUserId();
            debtCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(debtCategory);
            if (id != debtCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debtCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebtCategoryExists(debtCategory.Id))
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
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", debtCategory.UserId);
            return View(debtCategory);
        }

        // GET: Customer/DebtCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var debtCategory = await _context.DebtCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (debtCategory == null)
            {
                return NotFound();
            }

            return View(debtCategory);
        }

        // POST: Customer/DebtCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _userId = GetCurrentUserId();
            var debtCategory = await _context.DebtCategories.FirstOrDefaultAsync(x=>x.Id == id && x.UserId == _userId);
            _context.DebtCategories.Remove(debtCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DebtCategoryExists(int id)
        {
            return _context.DebtCategories.Any(e => e.Id == id && e.UserId == _userId);
        }

        protected string GetCurrentUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }

       
    }
}
