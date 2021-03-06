﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kinabalu.Models;
using Kinabalu.Services;

namespace Kinabalu.Controllers
{
    public class CustomersController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public CustomersController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/All/
        public IActionResult All()
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.CustomerView.ToHashSet().Take(200));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,LastUpdate")] Customer customer)
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
