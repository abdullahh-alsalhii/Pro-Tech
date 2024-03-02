using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProـTech_Web.DataBase;
using ProـTech_Web.Global;
using ProـTech_Web.Interfaces;

namespace ProـTech_Web.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Report_TypesModle
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
    }
    public class Report_TypesModle_DTO : ModlesFather, Dto_Void_Funcs, Dto_Value_Funcs<Report_TypesModle, List<Report_TypesModle>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Code_Res Create()
        {
            // the sesstion check will be from the middelwhere
            if (Name is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "missing params"
                };
            }
            Report_TypesModle report_type = new()
            {
                Name = Name
            };
            DB.Report_Types.Add(report_type);
            return new()
            {
                IsDone = true,
                Msg = "Report Type Created"
            };
        }

        public Code_Res Delete()
        {
            // the sesstion check will be from the middelwhere
            var is_deleted = DB.Report_Types.Where(x => x.Id == Id).ExecuteDelete();
            if (is_deleted == 0)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "type not found"
                };
            }
            return new()
            {
                IsDone = true,
                Msg = "type deletd"
            };
        }

        public Code_Res_With_Data<Report_TypesModle> GetById(int Id)
        {
            // the sesstion check will be from the middelwhere
            var type = DB.Report_Types.SingleOrDefault(x => x.Id == Id);
            if (type is null)
            {
                return new()
                {
                    Code_Res = new()
                    {
                        IsDone = false,
                        Msg = "type not found"
                    }
                };
            }
            return new()
            {
                Code_Res = new()
                {
                    IsDone = true,
                },
                Data = type
            };
        }
        public Code_Res Update()
        {
            // the sesstion check will be from the middelwhere
            var type = DB.Report_Types.SingleOrDefault(x => x.Id == Id);
            if (Name is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "Name Is Required"
                };
            }
            if (type is null)
            {
                return new()
                {
                    IsDone = false,
                    Msg = "type not found"
                };
            }
            type.Name = Name;
            return new()
            {
                IsDone = true,
                Msg = "type updated"
            };
        }

        public Code_Res_With_Data<List<Report_TypesModle>> GetList(GetFromTo fromTo)
        {
            // the sesstion check will be from the middelwhere
            List<Report_TypesModle> list_data;
            if (!fromTo.Search.IsNullOrEmpty())
            {
                list_data = FromTo(DB.Report_Types.Where(x => EF.Functions.Like(x.Name , $"%{fromTo.Search}%")), fromTo.From, fromTo.To);
            } else
            {
                list_data = FromTo(DB.Report_Types.Where(x => true), fromTo.From, fromTo.To);
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
