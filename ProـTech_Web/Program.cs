using MySqlConnector;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using ProـTech_Web.DataBase;
using ProـTech_Web.Global;
using ProـTech_Web.Models;
using ProـTech_Web.SignalR;
using static ProـTech_Web.Models.UsersModle_Config;

namespace Yofi_ASP_Net
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //----- create 100 user
            //MainContext db = new();
            //List<UsersModle> users_list = new();
            //Random rnd = new Random();
            //Console.WriteLine("Creating...");
            //for (int i = 1; i <= 100; i++)
            //{
            //    users_list.Add(new()
            //    {
            //        A_Roles = JsonConvert.SerializeObject(new List<UsersModle_Config.Admin_Roles> { (UsersModle_Config.Admin_Roles)rnd.Next(1, 6 + 1) }),
            //        U_Role = (UsersModle_Config.User_Roles)rnd.Next(1, 3 + 1),
            //        Email = "lala@lolo.com" + i,
            //        Password = "12345",
            //        Full_Name = "sad",
            //        Phone = "12345685"
            //    });
            //    Console.WriteLine($"User {i} Created");
            //}
            //db.Users.AddRange(users_list);
            //Console.WriteLine("Saving Data");
            //db.SaveChanges();
            //Console.WriteLine("Data Saved");
            //----- create 100 user
            //----- create 100 report
            //var random = new Random();
            //var types = new List<string>() { 

            //};
            //var all_types = JsonConvert.DeserializeObject<List<string>>(
            //    "[\"ahamd\",\"samer\",\"yofi\",\"ali zidan\",\"sham\",\"4_55\",\"wsqed\",\"qdqw\",\"gogo\",\"lama\"]"
            //)!;
            //for(int i = 1; i <= 100; i++)
            //{
            //    ReportsModle_DTO report = new()
            //    {
            //        Lat_Long_Location = $"[{random.Next(1, 30)}.{random.Next(100000, 999999)}, {random.Next(1, 30)}.{random.Next(100000, 999999)}]",
            //        Type = all_types[random.Next(all_types.Count)]
            //    };
            //    report.dev_Create();
            //    report.Get_DB().SaveChanges();
            //    Console.WriteLine($"report {i} created");
            //}
            //----- create 100 report
            //----- GetListTest
            //MainContext db = new();
            //var data = db.Users.Where(x => true).Skip(-1).Take(1).ToList();
            //File.WriteAllBytes("C:\\Users\\som\\Desktop\\mas\\test.json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            //Console.WriteLine("json_data_file_saved");
            //----- GetListTest
            //----- create user in database
            //var c_user = new UserModle_DTO()
            //{
            //    Email = "lala@lala.com",
            //    Full_Name = "lala lolo lele",
            //    Password = "12345",
            //    Phone = "123",
            //    U_Role = UsersModle_Config.User_Roles.User,
            //};
            //var is_created = c_user.Create();

            //try
            //{
            //    c_user.DB.SaveChanges();
            //    Console.WriteLine(is_created.Msg);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString().Contains("Duplicate entry"));
            //}
            //-----create user in database
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();
            //use Api
            builder.Services.AddEndpointsApiExplorer();
            // use Swagger
            builder.Services.AddSwaggerGen();
            //use SignalR
            builder.Services.AddSignalR();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "YoFi_Sesstion";
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });
            var app = builder.Build();
            if (app!.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // use sesstion
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            Console.WriteLine(Hasher.Hash("12341234"));
            // signalR Clasess
            app.MapHub<MainSocket>("/MineSocket");
            // signalR Clasess

            app.Run();
           


        }
    }
}


/*
 
user role token

NkYtODEtQTEtRTgtQUUtQzUtQTUtQUQtMTQtMUQtNTUtOUQtNEMtNkItMTUtMDEtMkItMUYtQ0YtNEEtOTktRTAtODItNTUtNjAtRDEtNTYtOTgtRkUtMzgtMzctQ0ItNzctNTktQkMtNDQtQ0YtMTItMzEtQ0MtOEMtQUItODYtNjQtMTMtM0EtOTItMzktNUUtOTYtQTgtNzktQTktQ0YtMDAtNjUtM0QtNkItRjktMTktOUUtQjItN0ItNUQtMTEtMUEtQzYtMzctNzMtRjQtMkUtNjEtODItMTQtRjktNTAtMDQtQjQtODUtOTktQTgtODgtRDItMEUtOTAtODQtMEQtM0EtRjYtM0EtODgtMDgtNzktNzQtQjctOEYtN0UtNzMtMTQtOTEtMjktNEEtNzktMEMtNDQtRDctODUtOTctOEQtMUUtNTYtOEItNzgtRDMtNDktOEItQ0MtOTYtOUItMDMtMUYtNTUtNUEtREEtOUItRTktOUItMDAtQkMtRTctODQtMTgtQ0UtNjMtRDItQjItMDMtOUEtNjQtNUYtOTMtOUUtQUItNEEtRTctMzYtMEMtMTktODItRkMtRDMtRjMtMTEtRDgtRTAtNjAtMjAtOEUtMDQtRTUtOTItQjItQTMtRUMtNEQtMkEtRDctRUYtNzMtOTgtQkMtNTctOEMtRTQtRDAtOTYtQUYtOUItODItQTktQzUtQkQtNzktNUMtNEMtQUItN0EtODEtRUMtRTQtQTEtMzItOTQtMTktRTMtNjYtN0EtMUYtNEMtNkQtQzctNUYtMTUtMkYtNUEtNzUtQzctRTEtN0YtM0UtQ0MtRkMtRkMtNUYtNDYtQjQtN0EtQ0YtN0ItMjEtRUMtQkMtQzUtMDctQ0YtRDQtRDAtQTAtQ0YtMTktQUUtNzYtNDQtMjAtM0QtQjQtRDktNEYtRDItODEtNjktNEYtOEEtMjAtNzgtMzEtMTctNkYtNEMtNzMtMkQtNzAtQkMtMkItRkMtRDMtMTAtNjItMjItQTgtQTYtMjUtRTctNDEtNzItRDktMUEtNEItNDAtMTUtNUYtMzYtNDQtNTYtOUQtRDEtNzctMUYtNzItRUEtRjgtNjYtQTMtQTUtMjAtMEUtOUEtNTItRUMtQjgtQjctRjItREQtMUItQzUtMkUtODUtRkMtMDUtNTctMjAtMTEtNjItQTctMTUtOEMtRUUtQjctMzAtODMtOTQtNDItQ0UtMzgtRjAtQTItNEQtQTUtOEYtMEMtMDAtMzUtNkUtNUEtQzAtNTMtQTEtMzUtMkYtRkItMEItQTYtNUItRjItOTUtN0UtOUUtQzEtQ0YtODgtNTktQUItOTctRkUtNkMtQzYtOEYtN0ItNDItMEMtRjYtMDMtRTUtNkYtQTUtRDgtMDUtQkYtMEQtMjAtQzUtRkMtRjEtQzEtMDctOEQtNjQtQ0EtNjUtMDYtQ0MtQ0ItQzYtQkQtREMtMjUtQzgtQTQtMkMtQ0UtNEUtNjQtRDAtM0ItNTYtNEUtRDUtRUQtMzUtQzctOEYtQkMtNkYtOEQtM0QtN0EtQTAtQzMtNjMtQ0EtNUQtMzQtMjEtREYtODQtM0QtNUQtQjktMjQtRjItMTMtOTMtOEEtMUQtNTctRkMtODUtREMtMjYtQjQtRTYtMUItMDktQzEtQzYtQTgtMjctRTMtRTMtNTQtMEYtRDQtMDctNTQtMzctQTMtNTQtRDMtQTgtQjItODMtOUEtRTYtNzgtMEYtQkMtNzctQjQtNzgtMTktMEQtQkEtNkItNTMtMTItNzctMEMtRDUtQTgtOTctMjItOEYtNTctRTktNTUtQUMtNDUtMkYtOUQtNDctQ0QtRUQtMTUtOTAtRjAtNUItMjgtMDAtNTEtMUEtMkEtRTctM0ItRjUtQTAtOEEtODgtNjItRDMtQkQtMUMtM0QtMUYtQkItQUMtRTUtRjItQzMtMEQtNjAtNTgtQzgtMkEtNUUtRDUtQjEtQTgtQjgtMDQtOEYtMUUtMUQtODMtRkItMEItRTMtREEtQUItQ0MtNDItNTQtQkYtOUMtNjAtRkEtNzMtQTYtQzYtNTktRkMtNkMtMzItMzItNUMtNDYtNDktNEItNzQtQzQtNTMtQkYtRTktNTYtM0YtNzAtOTEtNDAtOTUtNkMtMEItRkUtRkMtOEItNjItNTQtMDItRDUtM0MtQTYtMDMtMjItMDUtOTYtNDEtNkItMkItNEMtMjktOUItMTEtNEQtNkEtQUEtRTYtMjctMzQtM0UtRjAtQjEtNkMtRjMtREUtQUEtMTEtOTgtODYtMzctMjItRTYtNjUtOUEtRjItOTctRDktOTAtN0ItNTMtNUItNkItOTktQ0EtNkItNDItOTEtNEMtRkUtQTUtMjAtMUYtNEYtOTktRTAtNzctM0UtNDMtNUItRDUtRTctRUQtNEMtNzAtQzQtQTItNjItMTgtREQtNjItNDQtN0QtRTEtQzMtNzMtRDUtNkYtQkYtRUUtNTItNTItMDUtNEItNEMtN0ItOUItMjgtODMtQUMtQjgtNDItQzQtQ0QtNUEtMjItREMtMDMtRjAtRUUtMzgtRTUtNTctMDctMDMtMDctMUEtNkYtNkQtNUEtQTktNzktMTMtRjYtRTUtNzgtM0YtRDktNDgtMUMtMEMtQkItNzQtOTMtNUYtQTYtNTEtMUYtNDQtQzktRkMtOEM=
tec role token
NkYtODEtQTEtRTgtQUUtQzUtQTUtQUQtMTQtMUQtNTUtOUQtNEMtNkItMTUtMDEtMkItMUYtQ0YtNEEtOTktRTAtODItNTUtNjAtRDEtNTYtOTgtRkUtMzgtMzctQ0ItNzctNTktQkMtNDQtQ0YtMTItMzEtQ0MtOEMtQUItODYtNjQtMTMtM0EtOTItMzktNUUtOTYtQTgtNzktQTktQ0YtMDAtNjUtM0QtNkItRjktMTktOUUtQjItN0ItNUQtMTEtMUEtQzYtMzctNzMtRjQtMkUtNjEtODItMTQtRjktNTAtMDQtQjQtODUtOTktQTgtODgtRDItMEUtOTAtODQtMEQtM0EtRjYtM0EtODgtMDgtNzktNzQtQjctOEYtN0UtNzMtMTQtOTEtMjktNEEtNzktMEMtNDQtRDctODUtOTctOEQtMUUtNTYtOEItNzgtRDMtNDktOEItQ0MtOTYtOUItMDMtMUYtNTUtNUEtREEtOUItRTktOUItMDAtQkMtRTctODQtMTgtQ0UtNjMtRDItQjItMDMtOUEtNjQtNUYtOTMtOUUtQUItNEEtRTctMzYtMEMtMTktODItRkMtRDMtRjMtMTEtRDgtRTAtNjAtMjAtOEUtMDQtRTUtOTItQjItQTMtRUMtNEQtMkEtRDctRUYtNzMtOTgtQkMtNTctOEMtRTQtRDAtOTYtQUYtOUItODItQTktQzUtQkQtNzktNUMtNEMtQUItN0EtODEtRUMtRTQtQTEtMzItOTQtMTktRTMtNjYtN0EtMUYtNEMtNkQtQzctNUYtMTUtMkYtNUEtNzUtQzctRTEtN0YtM0UtQ0MtRkMtRkMtNUYtNDYtQjQtN0EtQ0YtN0ItMjEtRUMtQkMtQzUtMDctQ0YtRDQtRDAtQTAtQ0YtMTktQUUtNzYtNDQtMjAtM0QtQjQtRDktNEYtRDItODEtNjktNEYtOEEtMjAtNzgtMzEtMTctNkYtNEMtNzMtMkQtNzAtQkMtMkItRkMtRDMtMTktNjAtM0QtQTAtRjMtRkYtMUYtRUItNDgtNkYtM0EtRjEtREEtQzMtQTctQkMtMEItRUEtODQtMjUtMjAtMzMtQkQtNzUtQTUtRjctNEMtN0ItMTItRTgtNDUtRkItQTktMjQtQzQtRjctRkYtMzUtMEYtMzQtMjQtMUEtNzgtMjAtM0MtMTctNTMtRkItRTQtNTYtRTEtNkMtRjMtRkEtMDctRUYtMTYtMkUtQTAtRDEtM0QtMUQtNjYtMjgtNzctOUMtMEMtMTUtNjQtNDEtQ0MtQTgtNDQtM0ItQTQtNjYtNTItRDAtNTAtQzktODAtRDgtQkItMDAtODYtM0EtNDEtRkEtREYtMjUtN0QtNTYtMjAtQzEtMjYtMDctQzYtMDQtQjItMDItMUQtMjQtMTMtM0ItRUQtMjgtMzYtQ0EtN0UtNTItQTgtREItNEUtMkQtREMtOTEtRUItNDUtNzEtM0QtMEUtNEYtNUMtQUEtMkEtMTItQ0MtNUItODMtMDQtQ0YtNkItRkEtNUMtQTYtOTgtM0UtMTQtODgtRTYtNkItNkItN0QtNjEtMjItNDMtNDQtQTAtQTktNEQtMjAtOEMtNUMtRUUtQzMtNjYtOTAtNDAtQTEtNTYtRDAtQTctMDUtRDQtRTEtMzMtRUItRUMtODQtQkItQzQtNjMtMDctQjAtRjItQzAtNzUtRTctRDItQjktRjctMDktQjgtMzYtNjUtQTQtNEMtODktNTktQTctMUItNEEtNDEtMzEtMEQtMzctMkYtMTYtRjctOUYtRDItQzItNDAtMTQtMzUtRDMtNTgtQjgtRTQtOEEtQjQtRDgtQjItOTQtMUQtNjAtMEYtNzctOEYtMkQtNzYtREYtQzYtQTYtNEMtN0UtNzktOTMtMUYtQjItOTYtODYtQzItQzYtMEQtMTMtODItQjEtODEtNzctNkYtMzMtQTgtQjUtMUEtN0EtREQtOUQtNzUtMzItODYtNUEtMjUtMjktODYtRUUtODctOEYtMTYtQjMtNzQtOUMtOEQtODMtQjItNTQtQjItRjQtMDktMjAtRTMtRDYtNjgtNDAtQjctODgtMjktMzctRTctNTktQTAtMjItN0EtRTItQUMtODAtM0YtMDgtMTctODctMzUtMTAtNjItOTAtNUYtRjMtQzEtODUtQjMtRTgtNDctRjgtQkMtMkUtMUEtNTAtNTMtNjktMEItMjItRkUtREItNkYtNDItRjktOUItNTctNjEtNUItRDgtRTItNzItNUQtMkQtQ0UtRjgtMDktOTQtMTYtNjEtQ0ItMkQtMzktMUQtRUUtODAtNzMtQjItNDUtQUQtQTMtREMtMDQtRUYtMjUtOUYtMEUtODMtMzQtREQtMDQtNzItQzctREItOUItQUQtOUEtMkUtRDgtQUEtMjctQTItNjAtNjYtRUItN0QtODctNTctNEYtMEItQUQtOEYtMzQtQkQtMDktNEYtODQtQTEtQkMtNUUtRUUtNTgtMzgtNjUtMUEtNzItQTItMjMtMTQtOEQtRTUtMTUtQzUtNUUtRDQtNDMtNTEtOEQtMDEtMzk=
*/