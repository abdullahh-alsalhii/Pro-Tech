using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProـTech_Web.Global;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Controllers.Phone
{
    [Route("Phone/Api/[controller]")]
    public class Auth : Controller
    {
        /*
        type Body = {
            full_name:strint
            email:string
            password:string => req
            phone:string
        }
         */
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserModle_DTO c_user)
        {
            if(c_user.U_Role is not null)
            {
                return BadRequest(new Res
                {
                    IsDone = false,
                    Msg = "you can register with user only"
                } );
            }
            c_user.Set_Up(HttpContext);
            var is_user_created = c_user.Create();
            if (!is_user_created.IsDone)
            {
                return BadRequest(is_user_created);
            }
            var db = c_user.Get_DB();
            var is_it_saved = Funcs.DB_Save_With_Uniqe(ref db, "email has been taken");
            if(!is_it_saved.IsDone)
            {
                return BadRequest(is_it_saved);
            }
            return Ok(is_user_created);
        }

        /*
        https://boody.com/login
        mathod:post

        type Body = {
            email:string => req
            password:string => req
        }
         */
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserModle_DTO c_user)
        {
            c_user.Set_Up(HttpContext);
            var login_data = c_user.Login();
            if (!login_data.Code_Res.IsDone)
            {
                Response.StatusCode = 400;
                return BadRequest(login_data.Code_Res);
            }
            if (login_data.Data.U_Role != User_Roles.User && login_data.Data.U_Role != User_Roles.Tec)
            {
                Response.StatusCode = 400;
                return BadRequest(new Res
                {
                    IsDone = false,
                    Msg = "User Not Found"
                });
            }
            JWTData JWT_data = new()
            {
                Email = login_data.Data.Email,
                Full_Name = login_data.Data.Full_Name,
                Id = login_data.Data.Id,
                Phone = login_data.Data.Phone,
                U_Role = login_data.Data.U_Role,
            };
            var jwt_token = JwtAuthManager.CreateData(JWT_data);
            return Ok(new
            {
                Code_Res = new Res()
                {
                    IsDone = true,
                    Msg = "User Found"
                },
                Jwt_Token = jwt_token,
                Data = JWT_data
            });
        }
    }
}
