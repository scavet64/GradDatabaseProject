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
using MySql.Data.MySqlClient;
using X.PagedList;

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

        public IActionResult OutlierRatings(int id, string source)
        {
            var result = _context.OutliersRatingsProcedure.FromSql(
                new RawSqlString("call detect_outliers(@id, @source)"),
                new MySqlParameter("@id", id),
                new MySqlParameter("@source", source));

            return View(result.ToList());
        }

        public IActionResult SuggestedProduct()
        {
            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (customerUser == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            var result = _context.SuggestedProductsProcedure.FromSql(
                new RawSqlString("call recommended_items(@id, @source)"),
                new MySqlParameter("@id", customerUser.User.CustomerId),
                new MySqlParameter("@source", customerUser.User.CustomerSource));

            return View(result.ToList());
        }

        public async Task<IActionResult> AddToWishlist(int? productId, string productSource)
        {
            if (productId == null || productSource == null)
            {
                return NotFound();
            }

            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);

            var wishlist = new Wishlist
            {
                CustomerId = customerUser.User.CustomerId,
                CustomerSource = customerUser.User.CustomerSource,
                ProductId = productId.Value,
                ProductSource = productSource,
            };

            _context.Add(wishlist);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Wishlists");
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
            return RedirectToAction("Index", "ShoppingCarts"); ;
        }

        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, int? page = 1, string category = null)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentFilter = currentFilter;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.CostSortParm = sortOrder == "Cost" ? "cost_desc" : "Cost";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "Quantity_desc" : "Quantity";
            ViewBag.SourceSortParm = sortOrder == "Source" ? "Source_desc" : "Source";
            ViewBag.AverageRatingSortParm = sortOrder == "Average Rating" ? "Average_Rating_desc" : "Average Rating";
            ViewBag.AverageRecievedRatingSortParm = sortOrder == "Average Received Rating" ? "Average_Received_Rating_desc" : "Average Received Rating";

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

            if (!String.IsNullOrEmpty(currentFilter))
            {
                temp = temp.Where(s => s.Name.Contains(currentFilter) ||
                                       s.Description.Contains(currentFilter) ||
                                       s.Source.Contains(currentFilter));
            }

            temp = ProcessSorting(sortOrder, temp);

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(temp.ToPagedList(pageNumber, pageSize));
        }

        private static IQueryable<ProductsView> ProcessSorting(string sortOrder, IQueryable<ProductsView> temp)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    temp = temp.OrderByDescending(s => s.Name);
                    break;
                case "name":
                    temp = temp.OrderBy(s => s.Name);
                    break;
                case "Category_desc":
                    temp = temp.OrderByDescending(s => s.Category);
                    break;
                case "Category":
                    temp = temp.OrderBy(s => s.Category);
                    break;
                case "cost_desc":
                    temp = temp.OrderByDescending(s => s.Cost);
                    break;
                case "Cost":
                    temp = temp.OrderBy(s => s.Cost);
                    break;
                case "Quantity_desc":
                    temp = temp.OrderByDescending(s => s.Quantity);
                    break;
                case "Quantity":
                    temp = temp.OrderBy(s => s.Quantity);
                    break;
                case "Source_desc":
                    temp = temp.OrderByDescending(s => s.Source);
                    break;
                case "Source":
                    temp = temp.OrderBy(s => s.Source);
                    break;
                case "Average_Received_Rating_desc":
                    temp = temp.OrderByDescending(s => s.AverageReceivedRating);
                    break;
                case "Average Rating":
                    temp = temp.OrderBy(s => s.AverageReceivedRating);
                    break;
                case "Average_Rating_desc":
                    temp = temp.OrderByDescending(s => s.AverageRating);
                    break;
                case "Average Received Rating":
                    temp = temp.OrderBy(s => s.AverageRating);
                    break;
                default:
                    temp = temp.OrderBy(s => s.Name);
                    break;
            }

            return temp;
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
