using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDT_Project.AdditionalStructures
{
    public enum UserTypes { User = 0, Accountant = 1, Admin = 2, SysAdmin = 3 }
    public class ServerUser
    {
        public uint Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserTypes UserType { get; set; }
    }
}
