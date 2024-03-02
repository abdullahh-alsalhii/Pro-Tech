using Google.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Buffers;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using ProـTech_Web.Global;
using ProـTech_Web.Models;
using static Google.Rpc.Context.AttributeContext.Types;

namespace ProـTech_Web.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Check_Role_JWT : Attribute , IAsyncActionFilter
    {
        private UsersModle_Config.User_Roles U_Role { get; set; }
        public Check_Role_JWT(UsersModle_Config.User_Roles u_role)
        {
            U_Role = u_role;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var jwt = context.HttpContext.Request.Query["JWT"].ToString();
            var user_auth_data_jwt = Auth_Funcs.Get_Valid_JWT_Data(jwt);
            if (user_auth_data_jwt.Code_Res.IsDone)
            {
                var is_user_role_ok = Auth_Funcs.Check_User_Role(U_Role, user_auth_data_jwt);
                if (is_user_role_ok.IsDone)
                {
                    await next();
                } else
                {
                    context.HttpContext.Response.Redirect("/Not_Found");
                }
            } else
            {
                context.HttpContext.Response.Redirect("/Not_Found");
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Check_Role_Session : Attribute, IAsyncActionFilter
    {
        private UsersModle_Config.User_Roles? U_Role { get; set; } = null;
        private UsersModle_Config.Admin_Roles[]? A_Roles { get; set; } = null;
        private bool Is_All_Admin_Roles_Requred = false;
        private bool Is_Res_Api = false;
        public Check_Role_Session(UsersModle_Config.User_Roles u_role = 0, UsersModle_Config.Admin_Roles[]? a_roles = null, bool all_a_r_req = false, bool is_res_api = false)
        {
            U_Role = u_role == 0 ? null : u_role;
            A_Roles = a_roles;
            Is_All_Admin_Roles_Requred = all_a_r_req;
            Is_Res_Api = is_res_api;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            void bad_res()
            {
                if (Is_Res_Api)
                {
                    context.HttpContext.Response.Redirect("/Api/Not_Found");
                }
                else
                {
                    context.HttpContext.Response.Redirect("/Not_Found");
                }
            }
            var user_auth_data_Session = Auth_Funcs.Get_Valid_Session_Data(context.HttpContext);
            Code_Res Is_User_Role_Ok = new()
            {
                IsDone = U_Role is null ? true : false
            };
            Code_Res Is_Admin_Role_Ok = new()
            {
                IsDone = A_Roles is null ? true : false
            };
            if (user_auth_data_Session.Code_Res.IsDone)
            {
                if(U_Role is not null)
                {
                    Is_User_Role_Ok = Auth_Funcs.Check_User_Role((UsersModle_Config.User_Roles)U_Role, user_auth_data_Session);
                }
                if(A_Roles is not null && A_Roles.Length != 0)
                {
                    if (Is_All_Admin_Roles_Requred)
                    {
                        Is_Admin_Role_Ok = Auth_Funcs.Check_Admin_Roles(A_Roles, user_auth_data_Session);
                    } else
                    {
                        Is_Admin_Role_Ok = Auth_Funcs.Check_Admin_Role(A_Roles, user_auth_data_Session);
                    }
                }
                if(Is_User_Role_Ok.IsDone && Is_Admin_Role_Ok.IsDone)
                {
                    await next();
                } else
                {
                    bad_res();
                }
            } else
            {
                bad_res();
            }
        }
    }
}
