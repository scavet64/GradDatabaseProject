using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Models;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private ICookieService _cookieService;
        private grad_dbContext _context;

        public AuthenticationService(ICookieService cookieService, grad_dbContext context)
        {
            _context = context;
            _cookieService = cookieService;
        }

        public bool isAuthenticated(HttpRequest request, HttpResponse response)
        {
            if (int.TryParse(getCookieValue(request), out int result))
            {
                var user = _context.User.Where(u => u.UserId == result).ToList().FirstOrDefault();
                if (user != null)
                {
                    //update cookie time
                    _cookieService.Set(KinabaluConstants.cookieName, result.ToString(), new TimeSpan(0,30,0), response);
                    return true;
                }
            }

            return false;
        }

        public string GetCurrentlyLoggedInUser(HttpRequest request)
        {
            if (int.TryParse(getCookieValue(request), out int result))
            {
                var user = (from u in _context.User
                    join r in _context.Customer
                        on u.CustomerId equals r.CustomerId
                    where (u.UserId == result)
                    select new
                    {
                        Email = r.EmailAddress,
                    }).ToList().FirstOrDefault();

                if (user != null)
                {
                    return user.Email;
                }
            }

            return null;
        }

        public bool isUserAdmin(HttpRequest request)
        {
            if (int.TryParse(getCookieValue(request), out int result))
            {
                var entryPoint = (from u in _context.User
                    join r in _context.Role
                        on u.RoleId equals r.RoleId
                    where (u.CustomerId == result && r.RoleId == KinabaluConstants.AdminRole)
                    select new
                    {
                        u.UserId
                    }).ToList().FirstOrDefault();

                return entryPoint != null;
            }

            return false;
        }

        private string getCookieValue(HttpRequest request)
        {
            return _cookieService.Get(KinabaluConstants.cookieName, request);
        }

    }
}
