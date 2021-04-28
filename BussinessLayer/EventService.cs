using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace BussinessLayer
{
    public static class EventService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static object EventMasterModel { get; private set; }

        public static EventMaster GetEventById(int Id)
        {
            return db.EventMasters.Where(x=>x.EventId == Id).FirstOrDefault();
        }

        
        public static EventMaster GetEventByKey(string EventKey)
        {
            var EventKeyGuid = Guid.Parse(EventKey);
            return db.EventMasters.Where(x => x.EventKey == EventKeyGuid).AsNoTracking().FirstOrDefault();
        }

        public static IEnumerable<EventMaster> GetAllEvents()
        {
            return db.EventMasters.AsEnumerable();
        }

        public static void InsertEvent(EventMaster eventMaster)
        {
            if(eventMaster.ZoomToken != null)
            {
                UpdateZoomTokenEvent(eventMaster.ZoomToken,eventMaster.CustomerId);
            }
            db.EventMasters.Add(eventMaster);
            db.SaveChanges();
        }

        public static void UpdateEvent(EventMaster eventMaster)
        {
            db.Entry(eventMaster).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void UpdateZoomTokenEvent(string zoomToken,int customerid)
        {
            db.UpdateZoomToken(zoomToken,customerid);
        }

        public static void UpdateBigMarkerTokenEvent(string bigmarkerToken)
        {
            db.UpdateBigMarkerToken(bigmarkerToken);
        }
        public static void UpdateUser(EventUser eventuser)
        {
            db.Entry(eventuser).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeleteEvent(int Id)
        {
            EventMaster eventmaster = db.EventMasters.Find(Id);
            eventmaster.IsDeleted = true;
            UpdateEvent(eventmaster);
        }
        public static void DeleteUser(int Id)
        {
            EventUser eventuser = db.EventUsers.Find(Id);
            db.EventUsers.Remove(eventuser);
            db.SaveChanges();
            //eventuser.IsDeleted = true;
            //UpdateUser(eventuser);
        }
        public static void EventWiseEmail( int CustomerId)
        {
            var eventemail = db.Customers.Where(x => x.CustomerId.Equals(CustomerId)).Select(x => x.Email).FirstOrDefault();
            
        }
        public static int  geteventbycustomerId(int CustomerId)
        {
            var eventemail = db.EventMasters.Where(x => x.CustomerId.Equals(CustomerId)).Select(x => x.EventId).FirstOrDefault();
            return eventemail;
        }

        public static int geteventdetailbyeventId(int eventId)
        {
            var cId = db.EventMasters.Where(x => x.EventId.Equals(eventId)).Select(x => x.CustomerId).FirstOrDefault();
            return cId;
        }
        public static EventUser GetEventusersByCustomerId(int CustomerId)
        {
            var eventid = db.EventUsers.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).FirstOrDefault();
            return eventid;
        }
    }
}
