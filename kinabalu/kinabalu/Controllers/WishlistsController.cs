using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kinabalu.Models;
using Kinabalu.Services;

namespace Kinabalu.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public WishlistsController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (customerUser == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            var temp = (from w in _context.Wishlist
                join pv in _context.ProductsView on new { A = w.ProductId, B = w.ProductSource } equals new { A = pv.ProductId, B = pv.Source }
                where w.CustomerId == customerUser.User.CustomerId && w.CustomerSource == customerUser.User.CustomerSource
                select new WishlistProductViewModel { Wishlist = w, ProductName = pv.Name });

            return View(await temp.ToListAsync());
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // GET: Wishlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerSource,ProductId,ProductSource,LastUpdate")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerSource,ProductId,ProductSource,LastUpdate")] Wishlist wishlist)
        {
            if (id != wishlist.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.CustomerId))
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
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
        {
            if (id == null || source == null)
            {
                return NotFound();
            }

            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);

            var wishlist = await _context.Wishlist
                .FirstOrDefaultAsync(m => m.ProductId == id && 
                                          m.ProductSource.Equals(source) && 
                                          m.CustomerId == customerUser.User.CustomerId &&
                                          m.CustomerSource.Equals(customerUser.User.CustomerSource));
            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.CustomerId == id);
        }
    }
}
