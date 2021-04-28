using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class RegisteredUserService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static void InsertRegisteredUser(RegisteredUser registeredUser)
        {
            db.RegisteredUsers.Add(registeredUser);
            db.SaveChanges();
        }

        public static void UpdateRegisteredUser(RegisteredUser registeredUser)
        {
            db.Entry(registeredUser).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void DeleteRegisteredUser(int Id)
        {
            RegisteredUser registeredUser = db.RegisteredUsers.Find(Id);
            db.RegisteredUsers.Remove(registeredUser);
            db.SaveChanges();
        }
        public static void DeleteEventRegisterdUser(int Id)
        {
            var registeredusers = GetEventRegisteredUserByEventId(Id);
            foreach (var item in registeredusers)
            {
                item.IsDeleted = true;
            }
            //db.Entry(registeredusers).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static void InsertVisitorState(VisitorState visitorstate)
        {
            db.VisitorStates.Add(visitorstate);
            db.SaveChanges();
        }
        public static RegisteredUser GetRegisteredUserById(int Id)
        {
            return db.RegisteredUsers.Where(x => x.UserId == Id).FirstOrDefault();
        }

        public static IEnumerable<RegisteredUser> GetAllRegisteredUser()
        {
            return db.RegisteredUsers.AsEnumerable();
        }


        public static IEnumerable<RegisteredUser> GetUnPaidUserForEvent(int eventid)
        {
            return db.RegisteredUsers.Where(x => x.Ispaid == false && x.EventId == eventid).ToList();
        }
        public static IEnumerable<RegisteredUser> GetPaidUserForEvent(int eventid)
        {
            return db.RegisteredUsers.Where(x => x.Ispaid == true && x.EventId == eventid).AsEnumerable();
        }

        public static int GetRegisteredUserByCustomerId(int CustomerId)
        {
            var eventids = db.EventMasters.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var eventuserids = db.EventUsers.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var alleventid = eventids.Concat(eventuserids);
            var usercount = db.RegisteredUsers.Where(x => alleventid.Contains(x.EventId.Value) && x.IsRegistered == true && x.IsDeleted==false).ToList();
            int totalcount = usercount.Count;
            return totalcount;
            //var eventids = db.EventMasters.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            //var usercount = db.RegisteredUsers.Where(x => eventids.Contains(x.EventId.Value) && x.IsRegistered == true).ToList();
            //return usercount.Count();
        }
        public static int GetLinkedinUserByCustomerId(int CustomerId)
        {
            var eventids = db.EventMasters.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var eventuserids = db.EventUsers.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var alleventid = eventids.Concat(eventuserids);
            var usercount = db.RegisteredUsers.Where(x => alleventid.Contains(x.EventId.Value) && x.IsRegistered == true && x.IsDeleted == false &&x.Source.Equals("linkedin")).ToList();
            int totalcount = usercount.Count;
            return totalcount;
        }
        public static int GetFacebookUserByCustomerId(int CustomerId)
        {
            var eventids = db.EventMasters.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var eventuserids = db.EventUsers.Where(x => x.CustomerId == CustomerId).Select(x => x.EventId).ToList();
            var alleventid = eventids.Concat(eventuserids);
            var usercount = db.RegisteredUsers.Where(x => alleventid.Contains(x.EventId.Value) && x.IsRegistered == true && x.IsDeleted == false && x.Source.Equals("facebook")).ToList();
            int totalcount = usercount.Count;
            return totalcount;
        }
        public static int GetEventIdByCustomerId(int CustomerId)
        {
            var eventid = db.EventUsers.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).Select(a=>a.EventId).FirstOrDefault();
            return eventid;
        }
        
        public static int GetReferralId()
        {
            var chk = db.RegisteredUsers.Select(x => x.UserId).ToList();
            var Totalreferal = db.RegisteredUsers.Where(x => chk.Contains(x.ReferalId.Value)).ToList();

            return Totalreferal.Count();
        }
        public static int GetReferralIByEventId(int EventId)
        {
            var chk = db.RegisteredUsers.Select(x => x.UserId).ToList();
            var Totalreferal = db.RegisteredUsers.Where(x => chk.Contains(x.ReferalId.Value) && x.EventId == EventId && x.IsRegistered==true).ToList();

            return Totalreferal.Count();
        }
        public static int GetLinkedinUsersByEventId(int EventId)
        {
            var chk = db.RegisteredUsers.Select(x => x.UserId).ToList();
            var LinkedinUsers = db.RegisteredUsers.Where(x => chk.Contains(x.ReferalId.Value) && x.EventId == EventId && x.IsRegistered == true && x.Source.Equals("linkedin")).ToList();

            return LinkedinUsers.Count();
        }
        public static int GetFacebookUsersByEventId(int EventId)
        {
            var chk = db.RegisteredUsers.Select(x => x.UserId).ToList();
            var FacebookUsers = db.RegisteredUsers.Where(x => chk.Contains(x.ReferalId.Value) && x.EventId == EventId && x.IsRegistered == true &&x.Source.Equals("facebook")).ToList();

            return FacebookUsers.Count();
        }
        public static IEnumerable<RegisteredUser> GetRegisteredUserByEventId(int EventId)
        {
            return db.RegisteredUsers.Where(x => x.EventId == EventId && x.IsRegistered == true && x.Ispaid == true && x.IsDeleted == false);
        }
        public static IEnumerable<RegisteredUser> GetLinkedinUserByEventId(int EventId)
        {
            return db.RegisteredUsers.Where(x => x.EventId == EventId && x.IsRegistered == true && x.Ispaid == true && x.IsDeleted == false && x.Source.Equals("linkedin"));
        }
        public static IEnumerable<RegisteredUser> GetFacebookUserByEventId(int EventId)
        {
            return db.RegisteredUsers.Where(x => x.EventId == EventId && x.IsRegistered == true && x.Ispaid == true && x.IsDeleted == false && x.Source.Equals("facebook"));
        }
        public static List<RegisteredUser> GetEventRegisteredUserByEventId(int EventId)
        {
            return db.RegisteredUsers.Where(x => x.EventId == EventId && x.IsRegistered == true && x.Ispaid == true && x.IsDeleted == false).ToList();
        }
        public static string GetEventNamebyEventId(int EventId)
        {
            string eventname = db.EventMasters.Where(x => x.EventId == EventId).Select(x => x.EventName).FirstOrDefault();
            return eventname;
        }
        public static DateTime GetStartdatebyEventId(int EventId)
        {

            DateTime eventstartdate = db.EventMasters.Where(x => x.EventId == EventId).Select(x => x.EventStartDate).FirstOrDefault();
            return eventstartdate;

        }
        public static DateTime GetEnddatebyEventId(int EventId)
        {
            DateTime eventenddate = db.EventMasters.Where(x => x.EventId == EventId).Select(x => x.EventEndDate).FirstOrDefault();
            return eventenddate;

        }
        public static string GetEventlogobyEventId(int EventId)
        {
            string image = db.EventMasters.Where(x => x.EventId == EventId).Select(x => x.LogoUrl).FirstOrDefault();
            if (image == null)
            {
                return image;
            }
            else
            {
                string imagename = "../Images/EventLogo/" + image;
                return imagename;
            }
        }
        public static void MarkRegisteredUserAsPaidForEvent(bool IsApproved,int eventid,int amount)
        {
            var unpaiduser = GetUnPaidUserForEvent(eventid);
            foreach (var item in unpaiduser)
            {
                if (amount > 0)
                {
                    item.Ispaid = IsApproved;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    amount = amount - 1;
                }
                else
                {
                    break;
                }
            }
        }
        public static int Creditrequired(int EventId)
        {
            int unpaidmembers = db.RegisteredUsers.Where(x => x.EventId == EventId && x.Ispaid == false).Count();
            decimal cost = db.Packages.Select(x => x.Cost_per_Registration.Value).FirstOrDefault();
            decimal extracredit = ((unpaidmembers) * (cost));
            return Convert.ToInt32(extracredit);
        }
        public static IEnumerable<GetUserDetails_Result> SearchRegisteredUser(string SearchTitle, string Alpha, string Domain, int Event, int ReferralId)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int id = 0;
            var RtnData = (from data2 in db.GetUserDetails(id, Alpha, SearchTitle, Domain, Event, ReferralId)
                           select data2);

            return RtnData;
        }
        public static IEnumerable<GetRegisteredUserDetails_Result> SearchRegisteredUserByCustomer(string SearchTitle, string Alpha, string Domain, int Event, int ReferralId, int CustomerId)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int id = 0;
            var RtnData = (from data2 in db.GetRegisteredUserDetails(id, Alpha, SearchTitle, Domain, Event, CustomerId, ReferralId)
                           select data2);

            return RtnData;
        }
        public static IEnumerable<eventwiseuser_Result> Eventwiseuser(string SearchTitle, string Alpha, string Domain, int Event, int CustomerId, int ReferralId)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int id = 0;
            var RtnData = (from data2 in db.eventwiseuser(id, Alpha, SearchTitle, Domain, Event, CustomerId, ReferralId)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<ReferralUserReport_Result> ReferralUsers(int Event, int CustomerId , string SearchTitle)
        {
            int id = 0;
            var RtnData = (from data2 in db.ReferralUserReport(id, Event, CustomerId, SearchTitle)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<ZoomUserReport_Result> ZoomUserReport(int Event, int CustomerId, string SearchTitle)
        {
            int id = 0;
            var RtnData = (from data2 in db.ZoomUserReport(id, Event, CustomerId, SearchTitle)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<BigMarkerUserReport_Result> BigMarkerUserReport(int Event, int CustomerId, string SearchTitle)
        {
            int id = 0;
            var RtnData = (from data2 in db.BigMarkerUserReport(id, Event, CustomerId, SearchTitle)
                           select data2);

            return RtnData;
        }

        public static IEnumerable<BounceUserReport_Result> BounceUsers(int Event, int CustomerId, string SearchTitle)
        {
            int id = 0;
            var RtnData = (from data2 in db.BounceUserReport(id, Event, CustomerId ,SearchTitle)
                           select data2);
            return RtnData;
        }
    }

}
