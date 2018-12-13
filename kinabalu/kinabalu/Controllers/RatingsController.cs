using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Models;
using Kinabalu.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kinabalu.Controllers
{
    public class RatingsController : Controller
    {
        private readonly grad_dbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public RatingsController(grad_dbContext context, IAuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        // POST: Ratings/Create
        [HttpGet]
        public ActionResult Rate(int prodId, string source, int rating)
        {
            var loggedInUser = _authenticationService.GetCurrentlyLoggedInUser(Request);
            if (loggedInUser == null)
            {
                return NotFound();
            }

            try
            {
                //Check to see if this product id exists in the view first
                var prod = _context.ProductsView.FirstOrDefault(p => p.ProductId == prodId && p.Source == source);
                if (prod == null)
                {
                    return NotFound();
                }

                //did this user already rate this product?
                Rating ratingObject = _context.Rating.FirstOrDefault(r =>
                    r.CustomerId == loggedInUser.Customer.CustomerId && r.ProductId == prodId && r.ProductSource == source);
                if (ratingObject == null)
                {
                    //Make a new rating then add it to the context
                    ratingObject = new Rating()
                    {
                        CustomerId = loggedInUser.Customer.CustomerId,
                        CustomerSource = loggedInUser.User.CustomerSource,
                        ProductId = prodId,
                        ProductSource = source,
                        Rating1 = rating
                    };
                    _context.Add(ratingObject);
                }
                else
                {
                    //update existing rating
                    ratingObject.Rating1 = rating;
                }

                //save our changes
                _context.SaveChanges();
            }
            catch
            {
                //throw some error??
            }
            return RedirectToAction("Index", "Products"); ;
        }
    }
}