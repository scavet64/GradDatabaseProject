using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kinabalu.Models;
using Kinabalu.Services;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public SuppliersController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Checks the authentication for the logged in user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private bool CheckAuthentication(HttpRequest request, HttpResponse response)
        {
            return (_authenticationService.isAuthenticated(request, response) &&
                    _authenticationService.isUserAdmin(request));
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            var grad_dbContext = _context.Supplier.Include(s => s.Address);
            return View(await grad_dbContext.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .Include(s => s.Address)
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,Name,AddressId,LastUpdate")] Supplier supplier)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City", supplier.AddressId);
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City", supplier.AddressId);
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierId,Name,AddressId,LastUpdate")] Supplier supplier)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id != supplier.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierId))
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
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City", supplier.AddressId);
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .Include(s => s.Address)
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            var supplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.SupplierId == id);
        }
    }
}
