using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class RoleService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();
        public static Role GetRoleById(int Id)
        {
            return db.Roles.Where(x => x.RoleId == Id).FirstOrDefault();
        }
        public static Role GetRoleByName(string role)
        {
            return db.Roles.Where(x => x.RoleName == role).FirstOrDefault();
        }
    }
}
