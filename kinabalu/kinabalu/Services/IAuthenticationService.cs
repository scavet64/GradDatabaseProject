using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Services
{
    public interface IAuthenticationService
    {
        bool isAuthenticated(HttpRequest request);
        string GetCurrentlyLoggedInUser(HttpRequest request);
    }
}
