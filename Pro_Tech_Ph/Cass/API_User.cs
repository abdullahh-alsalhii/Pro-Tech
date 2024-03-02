using System;
using System.ComponentModel.DataAnnotations;

namespace Pro_Tech_Ph.Cass
{
    public enum User_Roles
    {
        User = 1,
        Admin = 2,
        Tec = 3,
    }
    public class API_User
	{

        public int Id { get; set; }
      
        public string Full_Name { get; set; }
        
        public string Email { get; set; }
       
        public string Password { get; set; }
      
        public string Phone { get; set; }
 
        public User_Roles U_Role { get; set; }
       
        public string A_Roles { get; set; } = "[]"; // Json Array
    }
    
}

