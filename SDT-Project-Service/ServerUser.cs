﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SDT_Project_Service
{
    enum UserTypes { User = 0, Accountant = 1, Admin = 2, SysAdmin = 3 }
    class ServerUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserTypes UserType { get; set; }
        public OperationContext operationContext { get; set; }
    }
}
