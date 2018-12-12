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
    public class RestocksController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public RestocksController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // GET: Restocks
        public async Task<IActionResult> Index()
        {
            if (!_authenticationService.isUserAdmin(Request))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            var grad_dbContext = _context.Restock.Include(r => r.Product);
            return View(await grad_dbContext.ToListAsync());
        }

        // GET: Restocks/Details/5
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

            var restock = await _context.Restock
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.RestockId == id);
            if (restock == null)
            {
                return NotFound();
            }

            return View(restock);
        }

        private bool RestockExists(int id)
        {
            return _context.Restock.Any(e => e.RestockId == id);
        }
    }
}
