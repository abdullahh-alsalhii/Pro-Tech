using Google.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ProـTech_Web.Controllers.API;
using ProـTech_Web.DataBase;
using ProـTech_Web.Global;
using ProـTech_Web.Interfaces;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Models
{
    public class UsersModle_Config
    {

        public enum User_Roles
        {
            User = 1,
            Admin = 2,
            Tec = 3,
        }
        public enum Admin_Roles
        {
            Create_User = 1,
            Delete_User = 2,
            Update_User = 3,
            Edit_User_Roles = 4,
            Manage_Reports = 5,
            View_Reports = 6,
        }
    }
    [Index(nameof(Email), IsUnique = true)]
    public class UsersModle
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(150)]
        [Required]
        public string Full_Name { get; set; }
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [MaxLength(50)]
        [Required]
        public string Password { get; set; }
        [MaxLength(20)]
        [Required]
        public string Phone { get; set; }
        [Required]
        public User_Roles U_Role { get; set; }
        [Required]
        [MaxLength(20)]
        public string A_Roles { get; set; } = "[]"; // Json Array
    }

    public class UserModle_DTO : ModlesFather, Dto_Void_Funcs , Dto_Value_Funcs<UsersModle, List<UsersModle>>
    {
        public int? Id { get; set; } = null;
        public string? Full_Name { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? New_Password { get; set; } = null;
        public string? Phone { get; set; } = null;
        public User_Roles? U_Role { get; set; } = null;
        public List<Admin_Roles>? A_Roles { get; set; } = null;
        public Code_Res Create()
        { 
            if( Full_Name is null || Email is null || Password is null || Phone is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "missing fields"
                };
            }

            var created_user = new UsersModle()
            {
                U_Role = U_Role is null ? User_Roles.User : (User_Roles)U_Role,
                Email = Email,
                Full_Name = Full_Name,
                Password = Hasher.Hash(Password),
                Phone = Phone,
                A_Roles = "[]",
            };

            if (U_Role == User_Roles.Admin || U_Role == User_Roles.Tec)
            {
                if((A_Roles is null || A_Roles.Count == 0) && U_Role == User_Roles.Admin && U_Role is null)
                {
                    return new()
                    {
                        IsDone = false,
                        Msg = "missing fields"
                    };
                }
                if (A_Roles is not null  && U_Role == User_Roles.Tec)
                {
                    return new()
                    {
                        IsDone = false,
                        Msg = "Bad Admin Roles"
                    };
                }
                created_user.A_Roles = U_Role == User_Roles.Admin ? JsonConvert.SerializeObject(A_Roles) : "[]";
                var admin_data = Auth_Funcs.Get_Valid_Session_Data(HTTP);
                if (admin_data.Code_Res.IsDone)
                {
                    var is_can_create = Auth_Funcs.Check_Admin_Role(Admin_Roles.Create_User, admin_data);
                    if (!is_can_create.IsDone)
                    {
                        return is_can_create;
                    }
                } else
                {
                    return admin_data.Code_Res;
                }
            }

            DB.Users.Add(created_user);

            return new()
            {
                IsDone = true,
                Msg = "User Created"
            };
        }
        public Code_Res_With_Data<UsersModle> Login()
        {
            var user = DB.Users.SingleOrDefault(x => x.Email == Email);
            if(user is null)
            {

                return new()
                {
                    Code_Res = new()
                    {
                        IsDone = false,
                        Msg = "User Not Found"
                    },
                };
            }
            var is_password_ok = Hasher.Compare_Hash(Password, user.Password);
            if (!is_password_ok)
            {
                return new()
                {
                    Code_Res = new()
                    {
                        IsDone = false,
                        Msg = "User Not Found"
                    },
                };
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = user
            };
        }
        public Code_Res Update()
        {
            if ((A_Roles is null || A_Roles.Count == 0) && U_Role == User_Roles.Admin)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "missing fields"
                };
            }
            if (A_Roles is not null && U_Role == User_Roles.Tec)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "Bad Admin Roles"
                };
            }
            Code_Res UpdateUser(JWTData user_auth_data, Code_Res_With_Data<SessionData>? sessionData = null)
            {
                Code_Res res = new()
                {
                    IsDone = false,
                };
                var is_admin = Auth_Funcs.Check_Admin_Role(Admin_Roles.Update_User, sessionData);
                var db_user = is_admin.IsDone  ? DB.Users.SingleOrDefault(x => x.Id == Id) : DB.Users.SingleOrDefault(x => x.Id == user_auth_data.Id);
                if(db_user is null)
                {
                    res.Msg = "User Not Found";
                    return res;
                }
                if (is_admin.IsDone)
                {
                    // this will make you update the user with out a password
                } else if (Password is null || !Hasher.Compare_Hash(Password, db_user.Password))
                {
                    res.Msg = "Invalid Password";
                    return res;
                }
                db_user.Password = New_Password is null|| New_Password == "" ? db_user.Password : Hasher.Hash(New_Password);
                db_user.Email = Email is null || Email == "" ? db_user.Email : Email;
                db_user.Phone = Phone is null || Phone  == ""? db_user.Phone : Phone;
                db_user.Full_Name = Full_Name is null || Full_Name == "" ? db_user.Full_Name : Full_Name;
                db_user.U_Role = U_Role is not null && is_admin.IsDone || U_Role == 0 ? (User_Roles)U_Role : db_user.U_Role;
                db_user.A_Roles = A_Roles is not null && is_admin.IsDone ? JsonConvert.SerializeObject(A_Roles) : db_user.A_Roles;
                res.Msg = "User Updated";
                res.IsDone = true;
                return res;
            }
            var user_auth_data_jwt = Auth_Funcs.Get_Valid_JWT_Data(JWT);
            if (user_auth_data_jwt.Code_Res.IsDone)
            {
                return UpdateUser(user_auth_data_jwt.Data);
            }
            var user_auth_data_session = Auth_Funcs.Get_Valid_Session_Data(HTTP);
            if (user_auth_data_session.Code_Res.IsDone)
            {
                return UpdateUser(user_auth_data_session.Data, user_auth_data_session);
            }
            return new()
            {
                IsDone = false,
                Msg = "User Not Found"
            };
        }

        public Code_Res Delete()
        {

            Code_Res DeleteByUser(JWTData user_auth_data, Code_Res_With_Data<SessionData>? sessionData = null)
            {
                var is_can_Delete = Auth_Funcs.Check_Admin_Role(Admin_Roles.Delete_User, sessionData);
                if (is_can_Delete.IsDone)
                {
                    DB.Users.Where(x => x.Id == Id).ExecuteDelete();
                    return new()
                    {
                        IsDone = true,
                        Msg = "User Deleted"
                    };
                }
                if (Password is null)
                {
                    return new()
                    {
                        IsDone = true,
                        Msg = "Invalid password"
                    };
                }
                var password = Hasher.Hash(Password);
                DB.Users.Where(
                    x => x.Id == Id &&
                    Id == user_auth_data.Id &&
                    password == x.Password
                ).ExecuteDelete();
                return new()
                {
                    IsDone = true,
                    Msg = "User Deleted"
                };
            }
            var user_auth_data_jwt = Auth_Funcs.Get_Valid_JWT_Data(JWT);
            if (user_auth_data_jwt.Code_Res.IsDone)
            {
                return DeleteByUser(user_auth_data_jwt.Data);
            }
            var user_auth_data_session = Auth_Funcs.Get_Valid_Session_Data(HTTP);
            if (user_auth_data_session.Code_Res.IsDone)
            {
                return DeleteByUser(user_auth_data_session.Data, user_auth_data_session);
            }
            return new()
            {
                IsDone = false,
                Msg = "User Not Found"
            };
        }

        public Code_Res_With_Data<UsersModle> GetById(int Id)
        {
            Code_Res_With_Data<UsersModle> Get_User(JWTData user_auth_data, Code_Res_With_Data<SessionData>? sesstionData = null)
            {
                Code_Res_With_Data<UsersModle> res = new()
                {
                    Code_Res = new()
                    {
                        IsDone = false,
                    }
                };
                var is_can_get_user = Auth_Funcs.Check_Admin_Role(
                    new Admin_Roles[] {
                        Admin_Roles.Create_User,
                        Admin_Roles.Update_User,
                        Admin_Roles.Delete_User
                    },
                    sesstionData
                );
                var db_user = is_can_get_user.IsDone ? DB.Users.SingleOrDefault(x => x.Id == Id) : DB.Users.SingleOrDefault(x => x.Id == user_auth_data.Id);
                if(db_user is null)
                {
                    res.Code_Res.Msg = "User Not Found";
                    return res;
                }
                res.Data = db_user;
                res.Code_Res.IsDone = true;
                return res;
            }
            var user_auth_data_jwt = Auth_Funcs.Get_Valid_JWT_Data(JWT);
            if (user_auth_data_jwt.Code_Res.IsDone)
            {
                return Get_User(user_auth_data_jwt.Data);
            }
            var user_auth_data_session = Auth_Funcs.Get_Valid_Session_Data(HTTP);
            if (user_auth_data_session.Code_Res.IsDone)
            {
                return Get_User(user_auth_data_session.Data, user_auth_data_session);
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = false,
                    Msg = "User Not Found"
                }
            };
        }

        public Code_Res_With_Data<List<UsersModle>> GetList(GetFromTo fromTo)
        {
            // the sesstion check will be from the middelwhere
            List<UsersModle> list_data;
            var search_value = fromTo.Search;
            if (!search_value.IsNullOrEmpty())
            {
                var like_data = $"%{search_value}%";
                list_data = FromTo(
                    DB.Users.Where(x =>
                        EF.Functions.Like(x.Email, like_data) ||
                        EF.Functions.Like(x.Phone, like_data) ||
                        EF.Functions.Like(x.Full_Name, like_data)
                    ),
                    fromTo.From, fromTo.To
                );
            }
            else
            {
                list_data = FromTo(DB.Users.Where(x => true), fromTo.From, fromTo.To);
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = list_data
            };
        }
        public Code_Res_With_Data<List<UsersModle>> GetList_with_role(GetFromTo fromTo, User_Roles role, User_Roles? role2 = null)
        {
            // the sesstion check will be from the middelwhere
            List<UsersModle> list_data;
            var search_value = fromTo.Search;
            if (!search_value.IsNullOrEmpty())
            {
                var like_data = $"%{search_value}%";
                list_data = FromTo(
                    DB.Users.Where(x =>
                        (EF.Functions.Like(x.Email, like_data) ||
                        EF.Functions.Like(x.Phone, like_data) ||
                        EF.Functions.Like(x.Full_Name, like_data))
                        && (x.U_Role == role || x.U_Role == role2)
                    ),
                    fromTo.From, fromTo.To
                );
            }
            else
            {
                list_data = FromTo(DB.Users.Where(x => x.U_Role == role || x.U_Role == role2), fromTo.From, fromTo.To);
            }
            foreach(var user in list_data)
            {
                user.Password = null;
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = list_data
            };
        }
    }
}
