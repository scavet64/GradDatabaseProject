using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.Models;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Services
{
    public interface IAuthenticationService
    {
        bool isAuthenticated(HttpRequest request, HttpResponse response);
        UserCustomerViewModel GetCurrentlyLoggedInUser(HttpRequest request);
        bool isUserAdmin(HttpRequest request);
    }
}
