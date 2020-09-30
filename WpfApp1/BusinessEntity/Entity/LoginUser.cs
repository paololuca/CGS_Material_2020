using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BusinessEntity.Entity
{
    public class LoginUser
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public LoginProfile Type { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnabled { get; set; }
    }
}
