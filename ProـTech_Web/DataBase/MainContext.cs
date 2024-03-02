using Microsoft.EntityFrameworkCore;
using ProـTech_Web.Models;
using ProـTech_Web.Global;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ProـTech_Web.DataBase
{
    public class MainContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(connectionString: $@"datasource={Config.DB_Host};username={Config.DB_Username};password={Config.DB_Password};database={Config.DB_Name};SslMode=none", new MySqlServerVersion(Config.DB_Version));
        }

        public DbSet<UsersModle> Users { get; set; }
        public DbSet<ReportsModle> Reports { get; set; }
        public DbSet<Report_TypesModle> Report_Types { get; set; }


    }
    public abstract class ModlesFather
    {
        public void Set_Up(HttpContext http, string? jwt = null)
        {
            HTTP = http;
            JWT = jwt;
        }
        protected HttpContext HTTP { get; set; }
        protected MainContext DB { get; } = new();
        protected string? JWT { get; set; } = null;
        public MainContext Get_DB()
        {
            return DB;
        }
        public string? Get_JWT()
        {
            return JWT;
        }
        protected List<T> FromTo<T>(IQueryable<T> data, int from, int to)
        {
            from = from < 0 ? 0 : from;
            to = to < 1 ? 1 : to;
            return data.Skip(from).Take(to).ToList();
        }
        protected bool Like<T>(T propty, string? keyword)
        {
            if (keyword.IsNullOrEmpty())
            {
                return false;
            }
            return EF.Functions.Like(propty, $"%{(string)keyword!}%");
        }
    }
}
