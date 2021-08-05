using BusinessEntity.Type;
using System;


namespace BusinessEntity.Entity
{
    public class LoginUser
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public ProfileType Type { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnabled { get; set; }
    }
}
