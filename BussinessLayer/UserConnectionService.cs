using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataLayer;

namespace BussinessLayer
{
   public static class UserConnectionService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static UserConnection GetUserConnectionById(int Id)
        {
            return db.UserConnections.Where(x => x.UserId == Id).FirstOrDefault();
        }
        public static IEnumerable<GetUserConnectionDetails_Result> UserConnectionDetails( int UserId, int EventId)
        {
           
            var RtnData = (from data2 in db.GetUserConnectionDetails( UserId, EventId)
                           select data2);

            return RtnData;
        }
    }
}
