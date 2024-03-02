using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Phone
{
    [Route("Phone/Api/[controller]")]
    [Check_Role_JWT(User_Roles.Tec)]
    public class Tec : Controller
    {
        /*
        type Body = {
            
        }
         */
        [HttpPost("My_Info")]
        public IActionResult My_Info([FromQuery] string jwt)
        {
            var c_user = new UserModle_DTO();
            c_user.Set_Up(HttpContext, jwt);
            var user_data = c_user.GetById(0);
            if (!user_data.Code_Res.IsDone)
            {
                return BadRequest(user_data.Code_Res);
            }
            return Ok(user_data);
        }
        /*
        type Body = {
            id:number => req
        }
         */
        [HttpPost("Get_My_Report")]
        public IActionResult Get_My_Report([FromBody] ReportsModle_DTO c_report, [FromQuery] string jwt)
        {
            c_report.Set_Up(HttpContext, jwt);
            int id = c_report.Id is null ? 0 : (int)c_report.Id;
            var report_data = c_report.GetById(id);
            var user_jwtData = Auth_Funcs.Get_Valid_JWT_Data(c_report.Get_JWT()!);
            if (!report_data.Code_Res.IsDone)
            {
                return BadRequest(report_data.Code_Res);
            }
            if (user_jwtData.Data.Id != report_data.Data.Report.Tec_Id)
            {
                return BadRequest(new Res
                {
                    IsDone = false,
                    Msg = "report not found"
                });
            }
            return Ok(report_data);
        }
        /*
        type Body = {
            from:number => req
            to:number => req
            search:string
            from_time:Date
            to_time:Date
        }
        */
        [HttpPost("Get_My_Reports_List")]
        public IActionResult Get_My_Reports_List([FromBody] GetFromTo from_To, [FromQuery] string jwt)
        {
            ReportsModle_DTO c_report = new();
            c_report.Set_Up(HttpContext, jwt);
            var user_jwtData = Auth_Funcs.Get_Valid_JWT_Data(c_report.Get_JWT()!);
            var report_data = c_report.GetList_with_id(from_To, user_jwtData.Data.Id, null);
            if (!report_data.Code_Res.IsDone)
            {
                return BadRequest(report_data.Code_Res);
            }
            return Ok(report_data);
        }
    }
}
