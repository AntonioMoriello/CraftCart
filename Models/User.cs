using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Models
{
    public class User
    {
        public string Id { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
