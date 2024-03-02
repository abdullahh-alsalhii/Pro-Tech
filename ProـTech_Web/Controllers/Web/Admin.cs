using Microsoft.AspNetCore.Mvc;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Pasges
{
    [Check_Role_Session(User_Roles.Admin, null, false)]
    public class Admin : Controller
    {
        private IActionResult AdminView()
        {
            var sessionData = Funcs.GetSession(HttpContext)!;
            return View(sessionData);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/admin/panel");
        }
        [HttpGet]
        public IActionResult Reports()
        {
            return AdminView();
        }
        [HttpGet]
        public IActionResult Panel()
        {
            return AdminView();
        }
        [HttpGet]
        public IActionResult AddApplition()
        {
            return AdminView();
        }
        [HttpGet]
        public IActionResult Customers()
        {
            return AdminView();
        }
        [HttpGet]
        public IActionResult Employees()
        {
            return AdminView();
        }
    }
}
