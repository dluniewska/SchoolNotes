using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }
    }
}
