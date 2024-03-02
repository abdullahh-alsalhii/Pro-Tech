using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using ProـTech_Web.Models;

namespace ProـTech_Web.Global
{
    public class JWTData
    {
        public int Id { get; set; }
        public UsersModle_Config.User_Roles U_Role { get; set; }
        public string Full_Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
    public class SessionData : JWTData
    {
        public List<UsersModle_Config.Admin_Roles> A_Roles { get; set; }
    }
    public class ResModel<T>
    {
        public DateTime? expireDate { get; set; }
        public T Data { get; set; }
        public bool IsValid { get; set; }
    }
    public class JwtAuthManager
    {
        public static string Create(string data)
        {
            List<Claim> clams = new List<Claim>() {
            new Claim(ClaimTypes.Name,data)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.JWT_SECURITY_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: clams, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var EncodedJWT = Encreption.En_Code(jwt);
            return EncodedJWT.Value;
        }
        public static ResModel<string> Get(string? EncodedToken)
        {
            if (EncodedToken is null)
            {
                return new() { IsValid = false };
            }
            var token = Encreption.De_Code(EncodedToken);
            if (!token.IsOk)
            {
                return new() { IsValid = false };
            }
            JwtSecurityTokenHandler handler = new();
            var isItToken = handler.CanReadToken(token.Value);
            if (!isItToken)
            {
                return new() { IsValid = false };
            }
            var decodedValue = handler.ReadJwtToken(token.Value);

            var time = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.Integer).Value;
            var text = decodedValue.Claims.SingleOrDefault(x => x.ValueType == ClaimValueTypes.String).Value;
            DateTimeOffset expireDate = DateTimeOffset.FromUnixTimeSeconds(int.Parse(time));

            if (expireDate.LocalDateTime < DateTime.Now)
            {
                return new() { IsValid = false };
            }
            if(text.IsNullOrEmpty())
            {
                return new() { IsValid = false };
            }
            return new() { expireDate = expireDate.Date, Data = text, IsValid = true };
        }
        public static string CreateData(JWTData data)
        {
            return Create(
                data: JsonConvert.SerializeObject(data)
            );
        }
        public static ResModel<JWTData> GetData(string? EncodedToken)
        {
            var jwt_res_data = Get(
                EncodedToken: EncodedToken
            );
            if (!jwt_res_data.IsValid || jwt_res_data.Data is null)
            {
                return new() { IsValid = false };
            }
            JWTData value = JsonConvert.DeserializeObject<JWTData>(jwt_res_data.Data)!;
            return new() { expireDate = jwt_res_data.expireDate, Data = value, IsValid = true };
        }

    }
}
