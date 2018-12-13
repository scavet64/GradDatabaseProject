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
    public class OrdersController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;
        public OrdersController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // GET: Orders	
        public async Task<IActionResult> Index()
        {
            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (customerUser == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            var temp = from o in _context.Order
                       join op in _context.OrderProduct on o.OrderId equals op.OrderId
                       join p in _context.ProductsView on new { A = op.ProductId, B = op.ProductSource } equals new { A = p.ProductId, B = p.Source }
                       where o.CustomerId.Equals(customerUser.User.CustomerId) && o.CustomerSource.Equals(customerUser.User.CustomerSource)
                       select new OrderProductViewModel { Order = o, ProductSource = op.ProductSource, ProductId = op.ProductId, ProductQuantity = op.Quantity, ProductName = p.Name };
            return View(await temp.ToListAsync());
        }

        // GET: Orders/Details/5	
        public async Task<IActionResult> Details(int? id)
        {
            var customerUser = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (customerUser == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Order
               .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
