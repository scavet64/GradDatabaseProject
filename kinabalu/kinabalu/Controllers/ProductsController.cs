using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kinabalu.Models;
using Kinabalu.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Kinabalu.Controllers
{
    public class ProductsController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public ProductsController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> AddToShoppingCart(int? productId, string productSource)
        {
            if (productId == null || productSource == null)
            {
                return NotFound();
            }

            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);

            var shoppingCartEntry = (from sc in _context.ShoppingCart
                where (sc.CustomerId.Equals(customerUser.User.CustomerId) && sc.CustomerSource.Equals(customerUser.User.CustomerSource) && 
                       sc.ProductId.Equals(productId) && sc.ProductSource.Equals(productSource))
                select sc).ToList().FirstOrDefault();

            if (shoppingCartEntry != null)
            {
                shoppingCartEntry.ProductQuantity++;
            }
            else
            {
                var shoppingCart = new ShoppingCart
                {
                    CustomerId = customerUser.User.CustomerId,
                    CustomerSource = customerUser.User.CustomerSource,
                    ProductId = productId.Value,
                    ProductSource = productSource,
                    ProductQuantity = 1
                };

                _context.Add(shoppingCart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products
        public async Task<IActionResult> Index(string category = null)
        {
            IQueryable<ProductsView> temp;
            if (category == null)
            {
                temp = _context.ProductsView;
            }
            else
            {
                temp = from p in _context.ProductsView
                    where p.Category.Equals(category)
                    select p;
            }

            return View(temp.ToList());
        }

        public async Task<IActionResult> IndexCategory(string category)
        {
            var temp = from p in _context.ProductsView
                where p.Category.Equals(category)
                select p;

            return View(temp.ToList());
        }

        // GET: Products
        public async Task<IActionResult> Local()
        {
            var grad_dbContext = _context.Product.Include(p => p.Category).Include(p => p.Supplier);
            return View(await grad_dbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,SupplierId,CategoryId,Cost,ReorderLevel,WeightUnitOfMeasure,Weight,Quantity,LastUpdate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,SupplierId,CategoryId,Cost,ReorderLevel,WeightUnitOfMeasure,Weight,Quantity,LastUpdate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
