using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kinabalu.Services
{
    public interface ICookieService
    {
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique identifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>
        /// <param name="response">The response to set the cookie in</param>
        void Set(string key, string value, TimeSpan expireTime, HttpResponse response);

        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>
        /// <param name="response">The response to get the cookie from</param>
        /// <returns>string value</returns>  
        string Get(string key, HttpRequest request);

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>
        /// <param name="response">The response to remove the cookie from</param>
        void Remove(string key, HttpResponse response);
    }
}
