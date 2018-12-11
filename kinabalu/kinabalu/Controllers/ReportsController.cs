using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Controllers;
using Kinabalu.Models;
using Kinabalu.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kinabalu.Controllers
{
    public class ReportsController : Controller
    {

        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public ReportsController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            this._authenticationService = authenticationService;
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

        // GET: Report
        public ActionResult Index()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View();
        }

        // GET: Report/Create
        public ActionResult BelowMinStock()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.BelowMinimumStockView.ToList());
        }

        // GET: Report/Create
        public ActionResult InactiveUser()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.InactiveUserView.ToList());
        }

        // GET: Report/Create
        public ActionResult MostWishedForByCategory()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.MostWishedForByCategory.ToList());
        }

        // GET: Report/Create
        public ActionResult ProductLowSales()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.ProductLowSalesView.ToList());
        }

        // GET: Report/Create
        public ActionResult ProductShipment()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.ProductShipmentView.ToList());
        }

        // GET: Report/Create
        public ActionResult UnpurchasedWishedForItems()
        {
            if (!CheckAuthentication(Request, Response))
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            return View(_context.UnpurchasedWishedForItemsView.ToList());
        }
    }
}