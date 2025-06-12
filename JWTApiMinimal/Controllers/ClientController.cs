using JWTApiMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWTApiMinimal.Controllers
{
    [ApiController]
    [Route("Client")]
    public class ClientController : ControllerBase
    {

        [HttpGet]
        [Route("getList")]
        public dynamic GetClientList()
        {
            return new[]
                        {
                    new
                    {
                        Id = 1,
                        Name = "Alice Smith",
                        Email = "alice@example.com"
                    },
                    new
                    {
                        Id = 2,
                        Name = "Bob Johnson",
                        Email = "bob@example.com"
                    },
                    new
                    {
                        Id = 3,
                        Name = "Charlie Brown",
                        Email = "charlie@example.com"
                    }
                };
        }


        [HttpDelete]
        [Route("DeleteClient")]
        public dynamic deleteCLient(string id) {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = Jwt.validToken(identity);

            if (!rToken.success) return rToken.result;


            Microsoft.Extensions.Primitives.StringValues token = Request.Headers.Where(x => x.Key == "Authorization").FirstOrDefault().Value;

            return new 
            {
                success = false,
                message = "Token fail",
                result = token
            };

        }

    }
}
