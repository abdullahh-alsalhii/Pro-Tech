using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ProـTech_Web.DataBase;
using ProـTech_Web.Global;
using ProـTech_Web.Interfaces;
using static ProـTech_Web.Models.ReportsModle_Config;
using static ProـTech_Web.Models.UsersModle_Config;

namespace ProـTech_Web.Models
{
    public class ReportsModle_Config
    {
        public enum Reports_Status
        {
            Closed_By_Customer = 1,
            Closed_By_Admin = 2,
            Open = 3,
        }
    }
    public class ReportsModle
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [AllowNull]
        public int? Tec_Id { get; set; } // forgen from (UsersModel, Id)
        [Required]
        public int Customer_Id { get; set; } // forgen from (UsersModel, Id)
        [Required]
        public Reports_Status Status { get; set; }
        [Required]
        public DateTime Opened_At { get; set; }
        [AllowNull]
        public DateTime? Closed_At { get; set; }
        [MaxLength(100)]
        [Required]
        public string Lat_Long_Location { get; set; }
        [Required]
        [MaxLength(20)]
        public string Type { get; set; } // forgen from (Report_Types, Name)
        [Required]
        public int Close_Code { get; set; }
    }

    public class ReportsModle_With_Join_Data
    {
        public ReportsModle Report { get; set; }  
        public UsersModle Customer { get; set; }
        public UsersModle? Tec { get; set; }
    }
    public class ReportsModle_DTO : ModlesFather , Dto_Void_Funcs, Dto_Value_Funcs<ReportsModle_With_Join_Data, List<ReportsModle_With_Join_Data>>
    {
        public int? Id { get; set; }
        public int? Tec_Id { get; set; } // forgen from (UsersModel, Id)
        public int? Customer_Id { get; set; } // forgen from (UsersModel, Id)
        public Reports_Status? Status { get; set; }
        public DateTime? Opened_At { get; set; }
        public DateTime? Closed_At { get; set; }
        public string? Lat_Long_Location { get; set; }
        public string? Type { get; set; } // forgen from (Report_Types, Name)
        public int? Close_Code { get; set; }
        public Code_Res_With_Data<ReportsModle> Create()
        {
            var random = new Random();
            if(Type is null || Lat_Long_Location is null)
            {
                return new()
                {
                    Code_Res = new ()
                    {
                        IsDone = false,
                        Msg = "misiing params"
                    }
                };
            }
            ReportsModle Create_Report(JWTData user_opner)
            {
                return new()
                {
                    Closed_At = null,
                    Opened_At = DateTime.Now,
                    Lat_Long_Location = Lat_Long_Location!,
                    Close_Code = random.Next(100000, 999999),
                    Customer_Id = user_opner.Id,
                    Status = Reports_Status.Open,
                    Type = Type!,
                };
            };
            var jwt_data = Auth_Funcs.Get_Valid_JWT_Data(JWT);
            if (!jwt_data.Code_Res.IsDone)
            {
                return new()
                {
                    Code_Res = jwt_data.Code_Res
                };
            }
            var is_valid_user = Auth_Funcs.Check_User_Role(User_Roles.User, jwt_data);
            if (!is_valid_user.IsDone)
            {
                return new()
                {
                    Code_Res = is_valid_user
                };
            }
            var created_report = Create_Report(jwt_data.Data);
            var data = DB.Add(created_report);
            return new()
            {
                Code_Res = new ()
                {
                    IsDone = true
                },
                Data = data.Entity
            };
        }
        public Code_Res dev_Create()
        {
            var random = new Random();
            ReportsModle created_report = new()
            {
                Closed_At = null,
                Opened_At = DateTime.Now,
                Lat_Long_Location = Lat_Long_Location!,
                Close_Code = random.Next(100000, 999999),
                Customer_Id = 101,
                Status = Reports_Status.Open,
                Type = Type!,
            };
            DB.Add(created_report);
            return new()
            {
                IsDone = true
            };
        }
        public Code_Res UpdateTic()
        {
            // the sesstion check will be from the middelwhere
            var db_report = DB.Reports.SingleOrDefault(x => x.Id == Id);
            if(db_report is null || Tec_Id is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "report not funde"
                };
            }
            db_report.Tec_Id = Tec_Id;
            return new()
            {
                IsDone = true,
                Msg = "report updated"
            };
        }
        public Code_Res UpdateStstus(bool add_close_date)
        {
            // the sesstion check will be from the middelwhere
            var db_report = DB.Reports.SingleOrDefault(x => x.Id == Id);
            if (db_report is null || Status is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "report not funde"
                };
            }
            db_report.Status = (Reports_Status)Status;
            if (add_close_date)
            {
                db_report.Closed_At = DateTime.Now;
            }
            return new()
            {
                IsDone = true,
            };
        }
        public Code_Res_With_Data<ReportsModle_With_Join_Data> GetById(int Id)
        {
            var db_report = DB.Reports.SingleOrDefault(x => x.Id == Id);
            if(db_report is null)
            {
                return new()
                {
                    Code_Res = new()
                    {
                        IsDone = false,
                        Msg = "report not found"
                    }
                };
            }
            var db_customer = DB.Users.SingleOrDefault(x => x.Id == db_report.Customer_Id);
            var db_tec = DB.Users.SingleOrDefault(x => x.Id == db_report.Tec_Id);
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                
                Data = new()
                {
                    Report = db_report,
                    Customer = db_customer!,
                    Tec = db_tec,
                }
            };
        }

        public Code_Res_With_Data<List<ReportsModle_With_Join_Data>> GetList_with_id(GetFromTo fromTo, int? tec_id, int? customer_id)
        {
            List<ReportsModle_With_Join_Data> final_data = new();

            var search_value = fromTo.Search;
            var is_time_null = fromTo.From_time is null && fromTo.To_time is null;
            var is_tec_id_null = tec_id is null;
            var is_customer_id_null = customer_id is null;

            List <ReportsModle> reports_list = new();
            List<int> customer_ids = new();
            List<int> tec_ids = new();
            void pushLists(ReportsModle data)
            {
                reports_list.Add(data);
                customer_ids.Add(data.Customer_Id);
                tec_ids.Add(data.Tec_Id is null ? 0 : (int)data.Tec_Id);
            }

            foreach (var data in FromTo(DB.Reports.Where(x =>
                (search_value.IsNullOrEmpty() && is_time_null) ? true &&
                (is_tec_id_null ? true : x.Tec_Id == tec_id) && (is_customer_id_null ? true : x.Customer_Id == customer_id) :
                    (
                        (search_value.IsNullOrEmpty() ? false :
                            (
                                EF.Functions.Like(x.Type, $"%{search_value}%")
                                ||
                                EF.Functions.Like(Convert.ToString(x.Status)!, $"%{search_value}%")
                            )
                        )
                        ||
                        (is_time_null ? false :
                            (x.Opened_At >= fromTo.From_time && x.Opened_At <= fromTo.To_time)
                        )
                    ) && (is_tec_id_null ? true : x.Tec_Id == tec_id) && (is_customer_id_null ? true : x.Customer_Id == customer_id)
            ), fromTo.From, fromTo.To))
            {
                pushLists(data);
            }
           

            List<UsersModle> customes = DB.Users.Where(x => customer_ids.Contains(x.Id)).ToList();
            List<UsersModle> tecs = DB.Users.Where(x => tec_ids.Contains(x.Id)).ToList();

            for(int i = 0; i < reports_list.Count; i++)
            {
                ReportsModle_With_Join_Data final_obj = new()
                {
                    Report = reports_list[i]
                };
                for(int j = 0;  j < customes.Count; j++)
                {
                    if (reports_list[i].Customer_Id == customes[j].Id)
                    {
                        final_obj.Customer = customes[j];
                        break;
                    }
                };
                for (int j = 0; j < tecs.Count; j++)
                {
                    if (reports_list[i].Customer_Id == tecs[j].Id)
                    {
                        final_obj.Tec = tecs[j];
                        break;
                    }
                };
                final_data.Add(final_obj);
            }

            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = final_data
            };
        }
        public Code_Res_With_Data<List<ReportsModle_With_Join_Data>> GetList(GetFromTo fromTo)
        {
            List<ReportsModle_With_Join_Data> final_data = new();

            var search_value = fromTo.Search;
            var is_time_null = fromTo.From_time is null && fromTo.To_time is null;
            List<ReportsModle> reports_list = new();
            List<int> customer_ids = new();
            List<int> tec_ids = new();
            void pushLists(ReportsModle data)
            {
                reports_list.Add(data);
                customer_ids.Add(data.Customer_Id);
                tec_ids.Add(data.Tec_Id is null ? 0 : (int)data.Tec_Id);
            }

            foreach (var data in FromTo(DB.Reports.Where(x =>
                (search_value.IsNullOrEmpty() && is_time_null) ? true :
                    (
                        (search_value.IsNullOrEmpty() ? false :
                            (
                                EF.Functions.Like(x.Type, $"%{search_value}%")
                                ||
                                EF.Functions.Like(Convert.ToString(x.Status)!, $"%{search_value}%")
                            )
                        )
                        ||
                        (is_time_null ? false :
                            (x.Opened_At >= fromTo.From_time && x.Opened_At <= fromTo.To_time)
                        )
                    )
            ), fromTo.From, fromTo.To))
            {
                pushLists(data);
            }


            List<UsersModle> customes = DB.Users.Where(x => customer_ids.Contains(x.Id)).ToList();
            List<UsersModle> tecs = DB.Users.Where(x => tec_ids.Contains(x.Id)).ToList();

            for (int i = 0; i < reports_list.Count; i++)
            {
                ReportsModle_With_Join_Data final_obj = new()
                {
                    Report = reports_list[i]
                };
                for (int j = 0; j < customes.Count; j++)
                {
                    if (reports_list[i].Customer_Id == customes[j].Id)
                    {
                        final_obj.Customer = customes[j];
                        break;
                    }
                };
                for (int j = 0; j < tecs.Count; j++)
                {
                    if (reports_list[i].Tec_Id == tecs[j].Id)
                    {
                        final_obj.Tec = tecs[j];
                        break;
                    }
                };
                final_data.Add(final_obj);
            }

            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = final_data
            };
        }

        public Code_Res Update()
        {
            throw new NotImplementedException();
        }
        public Code_Res Delete()
        {
            throw new NotImplementedException();
        }

        Code_Res Dto_Void_Funcs.Create()
        {
            throw new NotImplementedException();
        }
    }
}
