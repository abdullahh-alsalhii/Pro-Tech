using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text.Json;
using ProـTech_Web.Controllers.Phone;
using ProـTech_Web.Global;
using ProـTech_Web.Models;
using static ProـTech_Web.Models.UsersModle_Config;
using static ProـTech_Web.SignalR.MainSocket;

namespace ProـTech_Web.SignalR
{
    public class MainSocket : Hub
    {
        // modles
        class Groups_Keys
        {
            public static readonly string Admin = "Admin_Group";
            public static readonly string Customer = "Customer_Group";
            public static readonly string Tec = "Tec_Group";
        }
        public class Req<T>
        {
            public string? Jwt { get; set; } = null;
            public T Data { get; set; }
        }
        public class Req
        {
            public string? Jwt { get; set; } = null;
        }
        public class User_Data : JWTData
        {
            public string Jwt { get; set; }
            public string Connection_Id { get; set; }
        }
        // modles


        // my_methods
        private async Task Respons<T>(string event_name, Res<T> data)
        {
            await Clients.Clients(Context.ConnectionId).SendAsync(event_name, data);
        }
        private async Task Bad_Respons(string mag)
        {
            await Clients.Clients(Context.ConnectionId).SendAsync(Words.Socket_Error_Event, new Res
            {
                IsDone = false,
                Msg = mag
            });
        }
        private async Task Send_By_Id<T>(string id, string event_name, Res<T> data)
        {
            await Clients.Clients(id).SendAsync(event_name, data);
        }
        private User_Data? GetUserByConnectionId(string id)
        {
            for (int i = 0; i < Curent_Users.Count; i++)
            {
                if (Curent_Users[i].Connection_Id == Context.ConnectionId)
                {
                    return Curent_Users[i];
                }
            }
            return null;
        }
        private User_Data? GetUserById(int id)
        {
            for (int i = 0; i < Curent_Users.Count; i++)
            {
                if (Curent_Users[i].Id == id)
                {
                    return Curent_Users[i];
                }
            }
            return null;
        }
        // my_methods



        // dev_socket_events
        public async Task Dev_Print_Users(Req req)
        {
            Console.WriteLine("development: Dev_Print_Users Invoced");
            var final = new List<User_Data>();
            foreach (var user in Curent_Users)
            {
                var fake_user = Funcs.Copy_Obj(user);
                fake_user.Jwt = null;
                final.Add(fake_user);
            }
            await Clients.Clients(Context.ConnectionId).SendAsync("Dev_Print_Users", new Res<List<User_Data>>()
            {
                IsDone = true,
                Data = final,
            });
        }
        // dev_socket_events



        // connect and desconnect
        // signalR connection defenition
        // https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&tabs=visual-studio
        private static List<User_Data> Curent_Users = new ();
        public override Task OnConnectedAsync()
        {
            //Console.WriteLine($"new user conncted to signalR {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //Console.WriteLine($"old user disconnect to signalR {Context.ConnectionId}");
            for(int i = 0; i < Curent_Users.Count; i++)
            {
                if (Curent_Users[i].Connection_Id == Context.ConnectionId)
                {
                    Curent_Users.RemoveAt(i);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }
        // connect and desconnect

        // Main Events
        public async Task Erorr(Code_Res req)
        {
            await Bad_Respons(req.Msg);
        }
        public async Task SaveUser(Req req)
        {
            HttpContext httpContext = Context.GetHttpContext()!;
            JWTData? user_data;
            user_data = Funcs.GetSession(httpContext);
            if (user_data == null)
            {
                if(req.Jwt is null)
                {
                    await Bad_Respons(Words.Socket_No_Data_Msg);
                    return;
                }
                var jwt_data = Auth_Funcs.Get_Valid_JWT_Data(req.Jwt);
                if (!jwt_data.Code_Res.IsDone)
                {
                    await Bad_Respons(Words.Socket_No_Data_Msg);
                    return;
                }
                user_data = jwt_data.Data;
                if(user_data is null)
                {
                    await Bad_Respons(Words.Socket_No_Data_Msg);
                    return;
                }
            } else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, Groups_Keys.Admin);
            }
            if(user_data.U_Role == User_Roles.Tec)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, Groups_Keys.Tec);
            } else if (user_data.U_Role == User_Roles.User)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, Groups_Keys.Customer);
            }
            User_Data final_data = new() {
                Email = user_data.Email,
                Full_Name = user_data.Full_Name,
                Id = user_data.Id,
                Jwt = req.Jwt is null ? Words.No_JWT_Sesstion_Using : req.Jwt,
                Phone = user_data.Phone,
                U_Role = user_data.U_Role,
                Connection_Id = Context.ConnectionId
            };
            Curent_Users.Add(final_data);
        }
        // Main Events


        // Customer Events
        /*
         type Body {
            jwt:string
            data:{
                lat_Long_Location:"[lat, long]", => req
                type:string => req
            }
        }
         */
        public async Task Open_Report(Req<ReportsModle_DTO> c_req)
        {
            var user = GetUserByConnectionId(Context.ConnectionId);
            if(user is null || c_req.Jwt is null)
            {
                await Bad_Respons(Words.Socket_No_Data_Msg);
                return;
            }
            if(user.U_Role != User_Roles.User)
            {
                await Bad_Respons(Words.Socket_Not_Allowed_Evebt);
                return;
            }
            HttpContext httpContext = Context.GetHttpContext()!;
            c_req.Data.Set_Up(httpContext, c_req.Jwt);
            var created_data = c_req.Data.Create();
            if (!created_data.Code_Res.IsDone)
            {
                await Bad_Respons(created_data.Code_Res.Msg);
                return;
            }
            var db = c_req.Data.Get_DB();
            var is_save_ok = Funcs.DB_Save_With_Uniqe(ref db, "Somthing is Worng");
            if (!is_save_ok.IsDone)
            {
                await Bad_Respons(created_data.Code_Res.Msg);
                return;
            }
            var fake_data = Funcs.Copy_Obj(created_data);
            fake_data.Data.Close_Code = 0;
            await Clients.Group(Groups_Keys.Admin).SendAsync("Open_Report", fake_data);
        }
        // Customer Events

        // Tec Events
        /*
         type Body {
            Jwt:string
            Data:{
                Id:number,
                Close_Code:number
            }
        }
        */
        public async Task Tec_Close_Report(Req<ReportsModle_DTO> c_req)
        {
            var user = GetUserByConnectionId(Context.ConnectionId);
            if (user is null || c_req.Jwt is null)
            {
                await Bad_Respons(Words.Socket_No_Data_Msg);
                return;
            }
            if (user.U_Role != User_Roles.Tec)
            {
                await Bad_Respons(Words.Socket_Not_Allowed_Evebt);
                return;
            }
            HttpContext httpContext = Context.GetHttpContext()!;
            c_req.Data.Set_Up(httpContext, c_req.Jwt);
            int id = c_req.Data.Id is null ? 0 : (int)c_req.Data.Id;
            var report = c_req.Data.GetById(id);
            if (!report.Code_Res.IsDone)
            {
                await Bad_Respons(report.Code_Res.Msg);
                return;
            }
            if(report.Data.Report.Tec_Id != user.Id)
            {
                await Bad_Respons("report not found ");
                return;
            }
            if(report.Data.Report.Close_Code != c_req.Data.Close_Code)
            {
                await Bad_Respons("Invalid Close Code");
                return;
            }
            if(report.Data.Report.Status != ReportsModle_Config.Reports_Status.Open)
            {
                await Bad_Respons("This Report Has Closed !");
                return;
            }
            c_req.Data.Status = ReportsModle_Config.Reports_Status.Closed_By_Customer;
            c_req.Data.UpdateStstus(add_close_date: true);
            var db = c_req.Data.Get_DB();
            var is_it_saved = Funcs.DB_Save_With_Uniqe(ref db, "Somthing Is Worng");
            if (!is_it_saved.IsDone)
            {
                await Bad_Respons(is_it_saved.Msg);
                return;
            }
            var fakeData = Funcs.Copy_Obj(report);
            fakeData.Data.Report.Close_Code = 0;
            await Clients.Group(Groups_Keys.Admin).SendAsync("Tec_Close_Report", fakeData);
            var customer = GetUserById(report.Data.Report.Customer_Id);
            if(customer is not null)
            {
                await Clients.Clients(customer.Connection_Id).SendAsync("Tec_Close_Report", fakeData);
            }
        }
        // Tec Events



        // Admin Events
        /*
         type Body {
            Id:number
         }
         */
        public async Task Admin_Close_Report(ReportsModle_DTO c_report)
        {
            var user = GetUserByConnectionId(Context.ConnectionId);
            if (user is null)
            {
                await Bad_Respons(Words.Socket_No_Data_Msg);
                return;
            }
            HttpContext httpContext = Context.GetHttpContext()!;
            var sesstiobData = Funcs.GetSession(httpContext);
            var is_admin_can_change = Auth_Funcs.Check_Admin_Role(Admin_Roles.Manage_Reports, new () { 
                Code_Res = new ()
                {
                    IsDone = true
                },
                Data = sesstiobData,
            });
            if (user.U_Role != User_Roles.Admin || !is_admin_can_change.IsDone)
            {
                await Bad_Respons(Words.Socket_Not_Allowed_Evebt);
                return;
            }
            
            c_report.Set_Up(httpContext);
            int id = c_report.Id is null ? 0 : (int)c_report.Id;
            var report = c_report.GetById(id);
            if (!report.Code_Res.IsDone)
            {
                await Bad_Respons(report.Code_Res.Msg);
                return;
            }
            if (report.Data.Report.Status != ReportsModle_Config.Reports_Status.Open)
            {
                await Bad_Respons("This Report Has Closed !");
                return;
            }
            c_report.Status = ReportsModle_Config.Reports_Status.Closed_By_Admin;
            var is_updated = c_report.UpdateStstus(add_close_date: true);
            var db = c_report.Get_DB();
            var is_it_saved = Funcs.DB_Save_With_Uniqe(ref db, "Somthing Is Worng");
            if (!is_it_saved.IsDone)
            {
                await Bad_Respons(is_it_saved.Msg);
                return;
            }
            if (!is_updated.IsDone)
            {
                await Bad_Respons(is_updated.Msg);
                return;
            }
            is_updated.Msg = "report closed";
            var fakeData = Funcs.Copy_Obj(is_updated);
            await Clients.Group(Groups_Keys.Admin).SendAsync("Admin_Close_Report", fakeData);
            var customer = GetUserById(report.Data.Report.Customer_Id);
            int tec_id = report.Data.Report.Tec_Id is null ? 0 : (int)report.Data.Report.Tec_Id;
            var tec = GetUserById(tec_id);
            if(tec is not null)
            {
                await Clients.Clients(tec.Connection_Id).SendAsync("Admin_Close_Report", fakeData);
            }
            if (customer is not null)
            {
                await Clients.Clients(customer.Connection_Id).SendAsync("Admin_Close_Report", fakeData);
            }
        }
        /*
        type Body {
           Id:number,
           Tec_Id:number
        }
        */
        public async Task Give_To_Tec(ReportsModle_DTO c_report)
        {
            var user = GetUserByConnectionId(Context.ConnectionId);
            if (user is null)
            {
                await Bad_Respons(Words.Socket_No_Data_Msg);
                return;
            }
            HttpContext httpContext = Context.GetHttpContext()!;
            var sesstiobData = Funcs.GetSession(httpContext);
            var is_admin_can_change = Auth_Funcs.Check_Admin_Role(Admin_Roles.Manage_Reports, new()
            {
                Code_Res = new()
                {
                    IsDone = true
                },
                Data = sesstiobData,
            });
            if (user.U_Role != User_Roles.Admin || !is_admin_can_change.IsDone)
            {
                await Bad_Respons(Words.Socket_Not_Allowed_Evebt);
                return;
            }

            c_report.Set_Up(httpContext);
            int id = c_report.Id is null ? 0 : (int)c_report.Id;
            var report = c_report.GetById(id);
            if (!report.Code_Res.IsDone)
            {
                await Bad_Respons(report.Code_Res.Msg);
                return;
            }
            if (report.Data.Report.Tec_Id == c_report.Tec_Id)
            {
                await Bad_Respons("This Report Have Thhe Same Tecnical");
                return;
            }
            var is_updated = c_report.UpdateTic();
            var db = c_report.Get_DB();
            var is_it_saved = Funcs.DB_Save_With_Uniqe(ref db, "Somthing Is Worng");
            if (!is_it_saved.IsDone)
            {
                await Bad_Respons(is_it_saved.Msg);
                return;
            }
            var fakeData = Funcs.Copy_Obj(is_updated);
            await Clients.Group(Groups_Keys.Admin).SendAsync("Give_To_Tec", fakeData);
            var customer = GetUserById(report.Data.Report.Customer_Id);
            int tec_id = report.Data.Report.Tec_Id is null ? 0 : (int)report.Data.Report.Tec_Id;
            var tec = GetUserById(tec_id);
            if (tec is not null)
            {
                await Clients.Clients(tec.Connection_Id).SendAsync("Give_To_Tec", fakeData);
            }
            if (customer is not null)
            {
                await Clients.Clients(customer.Connection_Id).SendAsync("Give_To_Tec", fakeData);
            }
        }
        // Admin Events
    }


}

/*

public class Message_Res {
    public string msg { get; set; }
}
public class Message_Req
{
    public string msg { get; set; }
} 

 public async Task Sleep(Message_Req req)
        {
            Console.WriteLine("sleep activated");
            Funcs.Sleep(10);
            Message_Res res = new Message_Res()
            {
                msg = $"zopy"
            };
            await Clients.All.SendAsync("Sleep", res);
            return;
        }
        public async Task Number_All(Message_Req req)
        {
            
            // تجهز فنكشن فلتر
            if (req == null)
            {
                Console.WriteLine("SingleR: invalid json data");
                return;
            }
            if (req.msg == null)
            {
                Console.WriteLine("SingleR: invalid json data");
                return;
            }

            if(req.msg == "plus")
            {
                Variables.Single_R_Number_All++;
            } else if (req.msg == "min")
            {
                Variables.Single_R_Number_All--;
            }
            Message_Res res = new Message_Res()
            {
                msg = $"{Variables.Single_R_Number_All}"
            };
            await Clients.All.SendAsync("Number_All", res);
        }

        public async Task Number_Caller(Message_Req req)
        {
            // تجهز فنكشن فلتر
            // نحتاج نربط السيشن عشان تشتغل هذي الفنكشن
            HttpContext? httpContext = Context.GetHttpContext()!;
            int Number_Caller_value = int.Parse(httpContext.Session.GetString("plusser")!);
            Console.WriteLine($"SingleR: {Number_Caller_value}");
            if (req == null)
            {
                Console.WriteLine("SingleR: invalid json data");
                return;
            }
            if (req.msg == null)
            {
                Console.WriteLine("SingleR: invalid json data");
                return;
            }

            if (req.msg == "plus")
            {
                Number_Caller_value++;
                httpContext.Session.SetString("plusser", $"{Number_Caller_value}");
            }
            else if (req.msg == "min")
            {
                Number_Caller_value--;
                httpContext.Session.SetString("plusser", $"{Number_Caller_value}");
            }
            Message_Res res = new Message_Res()
            {
                msg = $"{Number_Caller_value}"
            };
            await Clients.Caller.SendAsync("Number_Caller", res);
        }
        public async Task News(Message_Req req)
        {
            Console.WriteLine(req);
            await Clients.Caller.SendAsync("News", new Message_Res() { 
                msg = "this is a msg from the back end"
            });
        }
 
 */
