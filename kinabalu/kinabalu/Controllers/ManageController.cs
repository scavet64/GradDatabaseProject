using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Kinabalu.Models;
using Kinabalu.Models.ManageViewModels;
using Kinabalu.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Kinabalu.Controllers
{
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(grad_dbContext gradDbContext, UrlEncoder urlEncoder, IAuthenticationService authenticationService)
        {
            _urlEncoder = urlEncoder;
            _context = gradDbContext;
            _authenticationService = authenticationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var temp = _authenticationService.GetCurrentlyLoggedInUser(Request);

            if (temp == null)
            {
                throw new ApplicationException($"Unable to load user with ID");
            }

            var customer = _context.Customer
                .Include(c => c.CustomerAddress)
                    .ThenInclude(ca => ca.Address)
                .Where(c => c.CustomerId == temp.Customer.CustomerId).ToList().FirstOrDefault();
            if (customer == null)
            {
                throw new ApplicationException($"Unable to load user with ID");
            }

            var model = new IndexViewModel
            {
                Email = customer.EmailAddress,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                StatusMessage = StatusMessage,
                Addresses = customer.CustomerAddress.ToList()
            };

            return View(model);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> RemoveAssociation(int? addressId)
        {
            if (addressId == null)
            {
                return NotFound();
            }

            var temp = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (temp == null)
            {
                throw new ApplicationException($"Unable to load user with ID");
            }

            _context.Database.ExecuteSqlCommand(
                $"DELETE FROM `customer_address` WHERE `customer_id` = {temp.Customer.CustomerId} AND `address_id` = {addressId}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var temp = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (temp == null)
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            var userToUpdate = _context.Customer.Where(c => c.CustomerId == temp.Customer.CustomerId).ToList().FirstOrDefault();
            if (userToUpdate == null)
            {
                return RedirectToAction(nameof(AccountController.AccessDenied), "Account");
            }

            try
            {
                if (model.Email != userToUpdate.EmailAddress)
                {
                    userToUpdate.EmailAddress = model.Email;
                }

                if (model.FirstName != userToUpdate.FirstName)
                {
                    userToUpdate.FirstName = model.FirstName;
                }

                if (model.LastName != userToUpdate.LastName)
                {
                    userToUpdate.LastName = model.LastName;
                }

                _context.SaveChanges();
            }
            catch (Exception ex) when (ex is DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Could not update your account");
            }


            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var temp = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (temp == null)
            {
                throw new ApplicationException($"Unable to load user with ID");
            }

            try
            {
                User user = _context.User.Where(u => u.UserId == temp.User.UserId).ToList().FirstOrDefault();

                if (user != null)
                {
                    user.Password = model.NewPassword;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Could not update password");
            }

            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }
    }
}
