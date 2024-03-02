using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProـTech_Web.Global;
using ProـTech_Web.Middlewares;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Pasges
{
    public class Users : Controller
    {
        private Code_Res Before_Edit_User_Datebase(UserModle_DTO c_user)
        {
            Code_Res res = new() {
                IsDone = false
            };
            if (c_user.U_Role == User_Roles.Admin)
            {
                c_user.A_Roles = new() { Admin_Roles.Update_User,
                    Admin_Roles.Edit_User_Roles,
                    Admin_Roles.Update_User,
                    Admin_Roles.View_Reports,
                    Admin_Roles.Create_User,
                    Admin_Roles.Delete_User,
                    Admin_Roles.Manage_Reports,
                };
            } else
            {
                c_user.A_Roles = null;
            }
            res.IsDone = true;
            return res;
        }
        [HttpGet]
        public IActionResult Auth()
        {
            var session = Funcs.GetSession(HttpContext);
            if (session is not null)
            {
                return Redirect("/admin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Auth([FromForm] UserModle_DTO c_user)
        {
            c_user.Set_Up(HttpContext);
            var login_data = c_user.Login();
            if (!login_data.Code_Res.IsDone)
            {
                Response.StatusCode = 400;
                ViewData["msg"] = login_data.Code_Res.Msg;
                return View();
            }
            if(login_data.Data.U_Role != User_Roles.Admin)
            {
                Response.StatusCode = 400;
                ViewData["msg"] = "User Not Found";
                return View();
            }
            SessionData sessionData = new() { 
                A_Roles = JsonConvert.DeserializeObject<List<Admin_Roles>>(login_data.Data.A_Roles)!,
                Email = login_data.Data.Email,
                Full_Name = login_data.Data.Full_Name,
                Id = login_data.Data.Id,
                Phone = login_data.Data.Phone,
                U_Role = login_data.Data.U_Role,
            };
            HttpContext.Session.SetString(Session_Keys.UserData, Funcs.Json(sessionData));
            return Redirect("/admin");
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin, 
            a_roles: new[] {Admin_Roles.Create_User, Admin_Roles.Delete_User, Admin_Roles.Update_User },
            all_a_r_req: false,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Get_One")]
        public IActionResult Get_One([FromBody] UserModle_DTO c_user)
        {
            int id = c_user.Id is null ? 0 : (int)c_user.Id;
            c_user.Set_Up(HttpContext);
            var user = c_user.GetById(id);
            return !user.Code_Res.IsDone ? BadRequest(user.Code_Res) : Ok(user.Data);
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin, 
            a_roles: new[]  {Admin_Roles.Update_User },
            all_a_r_req: true,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Update")]
        public IActionResult Update([FromForm] UserModle_DTO c_user)
        {
            c_user.Set_Up(HttpContext);
            var is_can_update = Before_Edit_User_Datebase(c_user);
            if (!is_can_update.IsDone)
            {
                return Ok(is_can_update);
            }
            var updated_user = c_user.Update();
            var db = c_user.Get_DB();
            var is_update = Funcs.DB_Save_With_Uniqe(ref db, "email has been taken");
            if (!is_update.IsDone)
            {
                return Ok(is_update);
            }
            return !updated_user.IsDone ? Ok(updated_user) : Ok(updated_user);
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin, 
            a_roles: new[]  { Admin_Roles.Create_User },
            all_a_r_req: true,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Create")]
        public IActionResult Create([FromForm] UserModle_DTO c_user)
        {
            c_user.Set_Up(HttpContext);
            var is_can_create = Before_Edit_User_Datebase(c_user);
            if (!is_can_create.IsDone)
            {
                return BadRequest(is_can_create);
            }
            var updated_user = c_user.Create();
            var db = c_user.Get_DB();
            var is_update = Funcs.DB_Save_With_Uniqe(ref db, "email has been taken");
            if (!is_update.IsDone)
            {
                return BadRequest(is_update);
            }
            return !updated_user.IsDone ? BadRequest(updated_user) : Ok(updated_user);
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin,
            a_roles: new[] { Admin_Roles.Delete_User },
            all_a_r_req: true,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Delete")]
        public IActionResult Delete([FromForm] UserModle_DTO c_user)
        {
            c_user.Set_Up(HttpContext);
            var updated_user = c_user.Delete();
            return !updated_user.IsDone ? BadRequest(updated_user) : Ok(updated_user);
        }
        [Check_Role_Session(
            u_role: User_Roles.Admin, 
            a_roles: new[] { Admin_Roles.Create_User, Admin_Roles.Delete_User, Admin_Roles.Update_User },
            all_a_r_req: false,
            is_res_api: true
        )]
        [HttpPost("/[Controller]/Api_Get_List")]
        public IActionResult Get_List([FromBody] GetFromTo fromTo)
        {
            var c_data = new UserModle_DTO();
            c_data.Set_Up(HttpContext);
            var updated_user = c_data.GetList(fromTo);
            return !updated_user.Code_Res.IsDone ? BadRequest(updated_user.Code_Res) : Ok(updated_user);
        }
        [HttpPost("/[Controller]/Api_Get_List_Role")]
        public IActionResult Get_List_Role([FromBody] GetFromTo fromTo, [FromQuery] User_Roles role, [FromQuery] User_Roles? role2 = null)
        {
            var c_data = new UserModle_DTO();
            c_data.Set_Up(HttpContext);
            var updated_user = c_data.GetList_with_role(fromTo, role, role2);
            return !updated_user.Code_Res.IsDone ? BadRequest(updated_user.Code_Res) : Ok(updated_user);
        }
    }
}
