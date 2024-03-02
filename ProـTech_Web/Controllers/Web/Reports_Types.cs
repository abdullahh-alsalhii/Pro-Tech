using Microsoft.AspNetCore.Mvc;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Pasges
{
    [Check_Role_Session(
        u_role:User_Roles.Admin,
        a_roles: new[] { Admin_Roles.Manage_Reports, Admin_Roles.View_Reports },
        all_a_r_req:false,
        is_res_api:true
    )]
    public class Reports_Types : Controller
    {
        [HttpPost("[Controller]/Api_Create")]
        public IActionResult Create([FromForm] Report_TypesModle_DTO c_report_type)
        {
            c_report_type.Set_Up(HttpContext);
            var created_report_type = c_report_type.Create();
            if (!created_report_type.IsDone)
            {
                return BadRequest(created_report_type);
            }
            var db = c_report_type.Get_DB();
            var is_db_saved = Funcs.DB_Save_With_Uniqe(ref db, "Thie Type Name Already Exists");
            if (!is_db_saved.IsDone)
            {
                return BadRequest(is_db_saved);
            }
            return Ok(created_report_type);
        }
        [HttpPost("[Controller]/Api_Update")]
        public IActionResult Update([FromForm] Report_TypesModle_DTO c_report_type)
        {
            c_report_type.Set_Up(HttpContext);
            var updated_report_type = c_report_type.Update();
            if (!updated_report_type.IsDone)
            {
                return BadRequest(updated_report_type);
            }
            var db = c_report_type.Get_DB();
            var is_db_saved = Funcs.DB_Save_With_Uniqe(ref db, "Thie Type Name Already Exists");
            if (!is_db_saved.IsDone)
            {
                return BadRequest(is_db_saved);
            }
            return Ok(updated_report_type);
        }
        [HttpPost("[Controller]/Api_Delete")]
        public IActionResult Delete([FromForm] Report_TypesModle_DTO c_report_type)
        {
            c_report_type.Set_Up(HttpContext);
            var deleted_report_type = c_report_type.Delete();
            if (!deleted_report_type.IsDone)
            {
                return BadRequest(deleted_report_type);
            }
            return Ok(deleted_report_type);
        }
        [HttpPost("[Controller]/Api_Get_List")]
        public IActionResult Get_List([FromBody] GetFromTo fromto)
        {
            Report_TypesModle_DTO c_report_type = new();
            c_report_type.Set_Up(HttpContext);
            var list_report_type = c_report_type.GetList(fromto);
            if (!list_report_type.Code_Res.IsDone)
            {
                return BadRequest(list_report_type.Code_Res);
            }
            return Ok(list_report_type);
        }
        [HttpPost("[Controller]/Api_Get_One")]
        public IActionResult Get_One([FromBody] Report_TypesModle_DTO c_report_type)
        {
            int id = c_report_type.Id is null ? 0 : (int)c_report_type.Id;
            c_report_type.Set_Up(HttpContext);
            var report_type = c_report_type.GetById(id);
            if (!report_type.Code_Res.IsDone)
            {
                return BadRequest(report_type.Code_Res);
            }
            return Ok(report_type);
        }
    }
}
