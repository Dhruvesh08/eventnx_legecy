using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class AdminService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();
        public static void UpdateAdminsetting(Adminsetting adminsetting)
        {
            db.Entry(adminsetting).State = EntityState.Modified;
            db.SaveChanges();
        }


        public static Adminsetting GetAdminSetting()
        {
            return db.Adminsettings.FirstOrDefault();
        }

        
    }
}
