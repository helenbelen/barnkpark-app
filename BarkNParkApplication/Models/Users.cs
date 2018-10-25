using System;
using System.Collections.Generic;

namespace BarkNParkApplication.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserFirstname { get; set; }
        public string UserLastname { get; set; }
        public string UserPaypal { get; set; }
    }
}
