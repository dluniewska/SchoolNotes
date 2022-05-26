//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace School.Models
//{
//    public class RolesSeeder
//    {
//        private readonly ApiContext _context;

//        public RolesSeeder(ApiContext context)
//        {
//            _context = context;
//        }

//        public void Seed()
//        {
//            if (_context.Database.CanConnect())
//            {
//                if (!_context.Roles.Any())
//                {
//                    var roles = GetRoles();
//                    _context.Roles.AddRange(roles);
//                    _context.SaveChanges();
//                }
//            }
//        }

//        private IEnumerable<Role> GetRoles()
//        {
//            var roles = new List<Role>()
//            {
//                new Role()
//                {
//                    Name = "User"
//                },
//                new Role()
//                {
//                    Name = "Admin"
//                },
//            };

//            return roles;
//        }
//    }
//}
