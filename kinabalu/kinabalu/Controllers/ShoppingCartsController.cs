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
    public class ShoppingCartsController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;


        public ShoppingCartsController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var temp = (from s in _context.ShoppingCart
                       join pv in _context.ProductsView on new {A = s.ProductId, B = s.ProductSource} equals new {A = pv.ProductId, B = pv.Source }
                       select new ShoppingCartProductViewModel{ ShoppingCart = s, ProductName = pv.Name});

            return View(temp.ToList());
        }

        public async Task<IActionResult> PlaceOrder()
        {
            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);

            var shoppingCart = (from s in _context.ShoppingCart
                where s.CustomerId == customerUser.User.CustomerId && s.CustomerSource == customerUser.User.CustomerSource
                select s).ToList();

            var order = new Order
            {
                CustomerId = customerUser.User.CustomerId,
                CustomerSource = customerUser.User.CustomerSource
            };

            _context.Order.Add(order);

            foreach (var item in shoppingCart)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.OrderId,
                    ProductSource = item.ProductSource,
                    ProductId = item.ProductId,
                    Quantity = item.ProductQuantity
                };

                _context.OrderProduct.Add(orderProduct);
            }

            _context.ShoppingCart.RemoveRange(shoppingCart.ToArray());

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerSource,ProductId,ProductSource,ProductQuantity,LastUpdate")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerSource,ProductId,ProductSource,ProductQuantity,LastUpdate")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.CustomerId))
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
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id, string source)
        {
            if (id == null || source == null)
            {
                return NotFound();
            }

            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);

            var shoppingCart = await _context.ShoppingCart
                .FirstOrDefaultAsync(s => s.ProductId == id &&
                                          s.ProductSource.Equals(source) &&
                                          s.CustomerId == customerUser.User.CustomerId &&
                                          s.CustomerSource.Equals(customerUser.User.CustomerSource));
            if (shoppingCart == null)
            {
                return NotFound();
            }

            _context.ShoppingCart.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.CustomerId == id);
        }
    }
}
