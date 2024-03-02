using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProـTech_Web.Controllers.Pasges;
using ProـTech_Web.DataBase;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.API
{
    [ApiController]
    [Route("/Api/[Controller]")]
    public class Test : Controller
    {
        private readonly ILogger<Home> _logger;
        [HttpGet]
        public IActionResult Index()
        {

            return Ok(new
            {
                res = Response.StatusCode,
                data = "this is a data"
            });
        }
        [HttpGet("Not_Found")]
        public IActionResult Not_Found()
        {
            Response.StatusCode = 404;
            return Ok(new
            {
                res = Response.StatusCode,
                Msg = "Not Found"
            });
        }
    }
}
