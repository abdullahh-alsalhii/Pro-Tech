using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using ProـTech_Web.DataBase;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Global
{
    public class Funcs
    {
        public static void Sleep(int Second)
        {
            Task.Delay(TimeSpan.FromSeconds(Second)).GetAwaiter().GetResult();
        }
        public static Code_Res DB_Save_With_Uniqe(ref MainContext db, string msg)
        {
            var res = new Code_Res();
            res.IsDone = false;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if(ex.ToString().Contains("Duplicate entry"))
                {
                    res.Msg = msg;
                    return res;
                }
                res.Msg = "Somthing Is Worng";
                return res;
            }
            res.IsDone = true;
            return res;
        }
        public static string Json(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        public static T Copy_Obj<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(Json(obj!))!;
        }
        public static SessionData? GetSession(HttpContext context)
        {
            var data = context.Session.GetString(Session_Keys.UserData);
            if(data is null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<SessionData>(data);
        }
    }
    public class Auth_Funcs
    {
        public static Code_Res_With_Data<SessionData> Get_Valid_Session_Data(HttpContext HTTP)
        {
            var invalidRes = new Code_Res_With_Data<SessionData>()
            {
                Code_Res = new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised,
                },
            };
            if (HTTP is null)
            {
                return invalidRes;
            }
            var user_data = HTTP.Session.GetString(Session_Keys.UserData);
            if (user_data.IsNullOrEmpty())
            {
                return invalidRes;
            }
            var data = JsonConvert.DeserializeObject<SessionData>(user_data!)!;
            return new()
            {
                Data = data,
                Code_Res = new() {
                    IsDone = true,
                }
            };
        }
        public static Code_Res_With_Data<string> Get_valid_JWT(string? JWT)
        {
            var invalidRes = new Code_Res_With_Data<string>()
            {
                Code_Res = new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised,
                },
            };
            if (JWT.IsNullOrEmpty())
            {
                return invalidRes;
            }
            var jwt_data = JwtAuthManager.Get(JWT);
            if (!jwt_data.IsValid)
            {
                return invalidRes;
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true
                },
                Data = jwt_data.Data
            };
        }
        public static Code_Res_With_Data<JWTData> Get_Valid_JWT_Data(string? JWT)
        {
            var jwt_data = Get_valid_JWT(JWT);
            if (!jwt_data.Code_Res.IsDone)
            {
                return new()
                {
                    Code_Res = jwt_data.Code_Res,
                };
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true
                },
                Data = JsonConvert.DeserializeObject<JWTData>(jwt_data.Data)!
            };
        }
        public static Code_Res Check_User_Role(UsersModle_Config.User_Roles role, Code_Res_With_Data<JWTData>? user_auth_data = null)
        {
            if(user_auth_data is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised
                };
            }
            if (!user_auth_data.Code_Res.IsDone)
            {
                return user_auth_data.Code_Res;
            }
            if (user_auth_data is not null && user_auth_data.Data.U_Role == role)
            {
                return new()
                {
                    IsDone = true,
                };
            }

            return new()
            {
                IsDone = false,
                Msg = Words.UnAuthorised
            };
        }
        public static Code_Res Check_User_Role(UsersModle_Config.User_Roles role, Code_Res_With_Data<SessionData>? user_auth_data = null)
        {
            if (user_auth_data is null)
            {
                return Check_User_Role(role, (Code_Res_With_Data<JWTData>?)null);
            }
            return Check_User_Role(role, new Code_Res_With_Data<JWTData>()
            {
                Code_Res = user_auth_data.Code_Res,
                Data = user_auth_data.Data
            });
        }
        public static Code_Res Check_Admin_Role(UsersModle_Config.Admin_Roles role, Code_Res_With_Data<SessionData>? user_auth_data = null)
        {
            if (user_auth_data is null || user_auth_data.Data is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised
                };
            }
            if (!user_auth_data.Code_Res.IsDone)
            {
                return user_auth_data.Code_Res;
            }
            foreach(var user_role in user_auth_data.Data.A_Roles)
            {
                if(user_role == role)
                {
                    return new()
                    {
                        IsDone = true
                    };
                }
            }
            return new()
            {
                IsDone = false,
                Msg = Words.UnAuthorised
            };
        }
        public static Code_Res Check_Admin_Roles(Admin_Roles[] roles, Code_Res_With_Data<SessionData>? user_auth_data = null)
        {
            if (user_auth_data is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised
                };
            }
            if (!user_auth_data.Code_Res.IsDone)
            {
                return user_auth_data.Code_Res;
            }
            foreach (var role in roles)
            {
                var is_role_found = false;
                foreach (var user_role in user_auth_data.Data.A_Roles)
                {
                    if(user_role == role)
                    {
                        is_role_found = true;
                    }
                }
                if (!is_role_found)
                {
                    return new()
                    {
                        IsDone = false,
                        Msg = Words.UnAuthorised
                    };
                }
            }
            //foreach (var user_role in user_auth_data.Data.A_Roles)
            //{
            //    var is_role_found = false;
            //    foreach (var role in roles)
            //    {
            //        if(role == user_role)
            //        {
            //            is_role_found = true;
            //        };
            //    }
            //    if (!is_role_found)
            //    {
            //        return new()
            //        {
            //            IsDone = false,
            //            Msg = Words.UnAuthorised
            //        };
            //    }
            //}
            return new()
            {
                IsDone = true
            };
        }
        public static Code_Res Check_Admin_Role(UsersModle_Config.Admin_Roles[] roles, Code_Res_With_Data<SessionData>? user_auth_data = null)
        {
            if (user_auth_data is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = Words.UnAuthorised
                };
            }
            if (!user_auth_data.Code_Res.IsDone)
            {
                return user_auth_data.Code_Res;
            }
            foreach (var user_role in user_auth_data.Data.A_Roles)
            {
                foreach (var role in roles)
                {
                    if (role == user_role)
                    {
                        return new()
                        {
                            IsDone = true,
                        };
                    };
                }
            }
            return new()
            {
                IsDone = false,
                Msg = Words.UnAuthorised
            };
        }
    }
}
