using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public ICookieService _cookieService;

        public AuthenticationService(ICookieService cookieService)
        {
            _cookieService = cookieService;
        }

        public bool isAuthenticated(HttpRequest request)
        {
            string cookievalue = _cookieService.Get("loggedin", request);
            return !string.IsNullOrWhiteSpace(cookievalue);
        }

        public string GetCurrentlyLoggedInUser(HttpRequest request)
        {
            return _cookieService.Get(KinabaluConstants.cookieName, request);
        }
    }
}
