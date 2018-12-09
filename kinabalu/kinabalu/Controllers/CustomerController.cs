using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kinabalu.DAL;
using Kinabalu.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kinabalu.Controllers
{
    [Route("api/CustomerController")]
    public class CustomerController : Controller
    {
        private grad_dbContext _context;

        public CustomerController(grad_dbContext context)
        {
            _context = context;
        }
        
        // GET
        public IActionResult Index()
        {
            return
            View();
        }

        // GET api/async
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            return new OkObjectResult(_context.Customer.ToList());
            //string test = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //using (var db = new ApplicationDb())
            //{
            //    await db.Connection.OpenAsync();
            //    var query = new CustomerContext(db);
            //    var result = await query.LatestPostsAsync();
            //    return new OkObjectResult(result);
            //}
        }

        //// GET api/async/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetOne(int id)
        //{
        //    using (var db = new ApplicationDb())
        //    {
        //        await db.Connection.OpenAsync();
        //        var query = new CustomerContext(db);
        //        var result = await query.FindOneAsync(id);
        //        if (result == null)
        //            return new NotFoundResult();
        //        return new OkObjectResult(result);
        //    }
        //}

        //// POST api/async
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]Customer body)
        //{
        //    using (var db = new ApplicationDb())
        //    {
        //        await db.Connection.OpenAsync();
        //        var query = new CustomerContext(db);
        //        await query.InsertAsync(body);

        //        return new OkObjectResult(body);
        //    }
        //}

        //// PUT api/async/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOne(int id, [FromBody]Customer body)
        //{
        //    using (var db = new ApplicationDb())
        //    {
        //        await db.Connection.OpenAsync();
        //        var query = new CustomerContext(db);
        //        var result = await query.FindOneAsync(id);
        //        if (result == null)
        //        {
        //            return new NotFoundResult();
        //        }

        //        result.FirstName = body.FirstName;
        //        result.LastName = body.LastName;
        //        result.EmailAddress = body.EmailAddress;
        //        result.LastLogin = body.LastLogin;
        //        await query.UpdateAsync(result);
        //        return new OkObjectResult(result);
        //    }
        //}

        //// DELETE api/async/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOne(int id)
        //{
        //    using (var db = new ApplicationDb())
        //    {
        //        await db.Connection.OpenAsync();
        //        var query = new CustomerContext(db);
        //        var result = await query.FindOneAsync(id);
        //        if (result == null)
        //        {
        //            return new NotFoundResult();
        //        }

        //        await query.DeleteAsync(result);
        //        return new OkObjectResult("Record was deleted");
        //    }
        //}

        //// DELETE api/async
        //[HttpDelete]
        //public async Task<IActionResult> DeleteAll()
        //{
        //    using (var db = new ApplicationDb())
        //    {
        //        await db.Connection.OpenAsync();
        //        var query = new CustomerContext(db);
        //        await query.DeleteAllAsync();
        //        return new OkObjectResult("all customers were deleted");
        //    }
        //}
    }
}