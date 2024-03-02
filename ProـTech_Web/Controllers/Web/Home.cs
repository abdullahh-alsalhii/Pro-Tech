using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Pasges
{
    public class Home : Controller
    {
        private readonly ILogger<Home> _logger;
        public Home(ILogger<Home> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Not_Found()
        {
            return NotFound();
        }
        [Check_Role_Session(User_Roles.Admin, null, false)]
        [HttpGet]
        public IActionResult Test_Single_R_Admin()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Test_Single_R_Customer()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Test_Single_R_Tec()
        {
            return View();
        }
        
     public IActionResult Log_Out()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}