using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kinabalu.Services
{
    public class CookieService : ICookieService
    {
        public CookieService() {}

        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique identifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>
        /// <param name="response">The response to set the cookie in</param>
        public void Set(string key, string value, TimeSpan expireTime, HttpResponse response)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.Add(expireTime);

            response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key</param>
        /// <param name="response">The response to get the cookie from</param>
        /// <returns>string value</returns>  
        public string Get(string key, HttpRequest request)
        {
            return request.Cookies[key];
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>
        /// <param name="response">The response to remove the cookie from</param>
        public void Remove(string key, HttpResponse response)
        {
            response.Cookies.Delete(key);
        }
    }
}
