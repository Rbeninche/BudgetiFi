﻿using System;
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
    public class CreditorCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string _userId;
        [TempData]
        public string StatusMessage { get; set; }

        public CreditorCategoriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer/CreditorCategories
        public async Task<IActionResult> Index()
        {
            _userId = GetCurrentUserId();
            var applicationDbContext = _context.CreditorCategories.Include(c => c.DebtCategory).Include(c => c.User);
            return View(await applicationDbContext.Where(c=>c.UserId == _userId).ToListAsync());
        }

        // GET: Customer/CreditorCategories/Create
        public IActionResult Create()
        {
            _userId = GetCurrentUserId();
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories.Where(c => c.UserId == _userId), "Id", "Name");

            return View();
        }

        // POST: Customer/CreditorCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DebtCategoryId,UserId")] CreditorCategory creditorCategory)
        {
            _userId = GetCurrentUserId();

            creditorCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(creditorCategory);
            if (ModelState.IsValid)
            {
                //Checks if the debt type has already been added by user
                var checksIfCreditorExists = _context.CreditorCategories.Include(c => c.DebtCategory)
                    .FirstOrDefault(c => c.Name == creditorCategory.Name
                && c.DebtCategory.Id == creditorCategory.DebtCategoryId
                && c.UserId == _userId);

                var checkIfNameExits = _context.CreditorCategories.Where(c => c.Name.ToLower()
                .Contains(creditorCategory.Name.ToLower()) && c.UserId == _userId);


                if (checksIfCreditorExists != null || checkIfNameExits.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error : Creditor exists under " + checksIfCreditorExists.Name + " category. Please use another name.";
                }
                else
                {
                    _context.Add(creditorCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }


            }
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories, "Id", "Name", creditorCategory.DebtCategoryId);
            ViewBag.StatusMessage = StatusMessage;
            return View(creditorCategory);
        }

        // GET: Customer/CreditorCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var creditorCategory = await _context.CreditorCategories
                .Include(c => c.DebtCategory)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            if (creditorCategory == null)
            {
                return NotFound();
            }

            return View(creditorCategory);
        }

        

        // GET: Customer/CreditorCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var creditorCategory = await _context.CreditorCategories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == _userId);
            if (creditorCategory == null)
            {
                return NotFound();
            }
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories
                .Where(c => c.UserId == _userId), 
                "Id", "Name", creditorCategory.DebtCategoryId);
          
            return View(creditorCategory);
        }

        // POST: Customer/CreditorCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DebtCategoryId,UserId")] CreditorCategory creditorCategory)
        {
            _userId = GetCurrentUserId();
            creditorCategory.UserId = _userId;
            ModelState.Clear();
            TryValidateModel(creditorCategory);
            if (id != creditorCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditorCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditorCategoryExists(creditorCategory.Id))
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
            ViewData["DebtCategoryId"] = new SelectList(_context.DebtCategories
              .Where(c => c.UserId == _userId),
              "Id", "Name", creditorCategory.DebtCategoryId);

            return View(creditorCategory);
        }

        // GET: Customer/CreditorCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _userId = GetCurrentUserId();
            if (id == null)
            {
                return NotFound();
            }

            var creditorCategory = await _context.CreditorCategories
                .Include(c => c.DebtCategory)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            if (creditorCategory == null)
            {
                return NotFound();
            }

            return View(creditorCategory);
        }

        // POST: Customer/CreditorCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _userId = GetCurrentUserId();
            var creditorCategory = await _context.CreditorCategories.SingleOrDefaultAsync(m => m.Id == id && m.UserId == _userId);
            _context.CreditorCategories.Remove(creditorCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditorCategoryExists(int id)
        {
            return _context.CreditorCategories.Any(e => e.Id == id && e.UserId == _userId);
        }

        protected string GetCurrentUserId()
        {
            return _userManager.GetUserId(HttpContext.User);
        }
    }


}