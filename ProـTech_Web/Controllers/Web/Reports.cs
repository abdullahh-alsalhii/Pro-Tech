using Microsoft.AspNetCore.Mvc;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Web
{
    public class Reports : Controller
    {
        
        [Check_Role_Session(
            u_role: User_Roles.Admin,
            a_roles: new[] { Admin_Roles.Manage_Reports, Admin_Roles.View_Reports },
            all_a_r_req: false,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Get_One")]
        public IActionResult Get_One([FromBody] ReportsModle_DTO c_report)
        {
            int id = c_report.Id is null ? 0 :(int)c_report.Id;
            c_report.Set_Up(HttpContext);
            var report = c_report.GetById(id);
            if(!report.Code_Res.IsDone)
            {
                return BadRequest(report.Code_Res);
            }
            return Ok(report);
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin,
            a_roles: new[] { Admin_Roles.Manage_Reports, Admin_Roles.View_Reports },
            all_a_r_req: false,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Get_List")]
        public IActionResult Get_List([FromBody]  GetFromTo fromTo)
        {
            ReportsModle_DTO c_report = new();
            c_report.Set_Up(HttpContext);
            var report = c_report.GetList(fromTo);
            if(!report.Code_Res.IsDone)
            {
                return BadRequest(report.Code_Res);
            }
            return Ok(report);
        }
    }
}
