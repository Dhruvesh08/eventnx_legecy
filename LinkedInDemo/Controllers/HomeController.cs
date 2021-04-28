using BussinessLayer;
using DataLayer;
using LinkedInDemo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using Customer = DataLayer.Customer;

namespace LinkedInDemo.Controllers
{
    public class HomeController : Controller
    {
        MongodbHelper _mongodbHelper;
        public HomeController()
        {
            _mongodbHelper = new MongodbHelper();
        }

        private EventRegistrationEntities db = new EventRegistrationEntities();
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }
        public ActionResult Index()
        {
            var eid = (Request.QueryString["eid"] == null) ? "" : Request.QueryString["eid"].ToString();
            var r = (Request.QueryString["r"] == null) ? "" : Request.QueryString["r"].ToString();
            var userid = (Request.QueryString["userid"] == null) ? "" : Request.QueryString["userid"].ToString();
            var code = (Request.QueryString["code"] == null) ? "" : Request.QueryString["code"].ToString();
            var state = (Request.QueryString["state"] == null) ? "" : Request.QueryString["state"].ToString();
            if (eid == "" && r == "" && userid == "" && code == "" && state == "")
            {
                return View();
            }
            else
            {
                return View("Redirect");
            }
            
        }
        public ActionResult zoomdemo()
        {
            return View();
        }
        public ActionResult zoomregister()
        {
            return View();
        }

        public ActionResult bigmarkerdemo()
        {
            return View();
        }
        public ActionResult bigmarkerregister()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult GenerateToken(string code, string eventkey, int referralid = 0)
        {
            var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type=authorization_code&code=" + code + "&redirect_uri=https%3A%2F%2Fwww.eventnx.com%2F%3Feid%3D" + eventkey + "%26r%3D" + referralid + "&client_id=81wyrvvax51otg&client_secret=AJITvo8AxL4Kf4a8", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FBGenerateToken(string code, int eventid, int referralid = 0)
        {
            var eventdetail = EventService.GetEventById(eventid);
            var client = new RestClient("https://graph.facebook.com/v5.0/oauth/access_token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "client_id=482419392462772&redirect_uri=" + code + "&redirect_uri=https://www.eventnx.com/&eid=" + eventdetail.EventKey + "&r=" + referralid + "&client_secret=cb937ae9975ffb63b122157a1b121008&code="+ code, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return Json(response.Content, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Authenticate(string accesstoken, string eventkey, int referralid = 0)
        {
            var eventid = EventService.GetEventByKey(eventkey).EventId;
            var client = new RestClient("https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~digitalmediaAsset:playableStreams))");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.linkedin.com");
            request.AddHeader("Postman-Token", "0b77a8dc-8ae0-4dc4-97f2-48d965518224,d4177a7d-8d60-4145-bee8-b63f4987d2d9");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
            request.AddHeader("Authorization", "Bearer " + accesstoken);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);


            var client1 = new RestClient("https://api.linkedin.com/v2/emailAddress?q=members&projection=%28elements%2A%28handle~%29%29");
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            request1.AddHeader("Connection", "keep-alive");
            request1.AddHeader("Accept-Encoding", "gzip, deflate");
            request1.AddHeader("Host", "api.linkedin.com");
            request1.AddHeader("Postman-Token", "ee43f094-9f10-419f-b99b-fd77245a0702,b68b518c-60fd-4ebf-8b61-f8f2b97ea23f");
            request1.AddHeader("Cache-Control", "no-cache");
            request1.AddHeader("Accept", "*/*");
            request1.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
            request1.AddHeader("Authorization", "Bearer " + accesstoken);
            request1.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            IRestResponse response1 = client1.Execute(request1);

            var exists = false;
            //IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var profile = JsonConvert.DeserializeObject<ProfileNew.RootObject>(response.Content);
                var emailresponse = JsonConvert.DeserializeObject<EmailResponse.RootObject>(response1.Content);
                //ShareOnWall(profile.Id, accesstoken);
                int userid = 0;
                string email = "";
                RegisteredUser registeredUser = new RegisteredUser();
                try
                {
                    email = emailresponse.elements[0].handle.emailAddress;
                }
                catch
                {

                }
                if (email != "")
                {
                    var matchuser = db.RegisteredUsers.Where(x => x.Email == email && x.EventId == eventid).ToList();
                    if (matchuser.Count > 0)
                    {
                        exists = true;
                        userid = matchuser.FirstOrDefault().UserId;
                    }
                    else
                    {
                        registeredUser.EventId = eventid;
                        registeredUser.FirstName = profile.firstName.localized.en_US;
                        registeredUser.LastName = profile.lastName.localized.en_US;
                        registeredUser.Email = email;
                        registeredUser.Country = "";
                        registeredUser.DateOfRegistration = DateTime.Now;
                        registeredUser.IsDeleted = false;
                        registeredUser.IsRegistered = false;
                        registeredUser.ProfileImage = profile.profilePicture.displayImage.elements.FirstOrDefault().identifiers.FirstOrDefault().identifier;
                        registeredUser.ProfileLink = profile.id;
                        registeredUser.ProfileId = profile.id;
                        registeredUser.AuthToken = accesstoken;
                        registeredUser.ReferalId = referralid;
                        registeredUser.Source = "linkedin";
                        var allowuser = db.Packages.Select(x => x.No_of_Registration).FirstOrDefault();
                        var reguser = db.RegisteredUsers.Where(x => x.EventId == eventid && x.Ispaid == true).Count();

                        //if (allowuser > reguser)
                        //{
                        //    registeredUser.Ispaid = true;
                        //}
                        //else
                        //{
                            registeredUser.Ispaid = false;
                        //}
                        db.RegisteredUsers.Add(registeredUser);
                        db.SaveChanges();

                        userid = registeredUser.UserId;
                    }
                    var eventurl = db.EventMasters.Find(eventid);
                    var adminsetting = AdminService.GetAdminSetting();
                    var utmsource = adminsetting.Utm_Source;
                    var utmmedium = adminsetting.Utm_Medium;
                    var utmcampaign = eventid;
                    var utmcontent = referralid;
                    var utmterm = adminsetting.Utm_Term;
                    return Json(new { eventurl = eventurl.ResponseURL + "?&eid=" + eventkey + "&r=" + referralid + "&userid=" + userid + "&exists=" + exists + "&accesstoken=" + accesstoken + "&profileid=" + profile.id + "&utm_source=" + utmsource + "&utm_medium=" + utmmedium + "&utm_campaign=" + utmcampaign + "&utm_content=" + utmcontent + "&utm_term=" + utmterm }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { eventurl = "", data = response.Content, error = response.ErrorMessage, ex = response.ErrorException }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { eventurl = "", data = response.Content, error = response.ErrorMessage, ex = response.ErrorException }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFBProfile(string accesstoken, int eventid, int referralid = 0)
        {
            var eventdetail = EventService.GetEventById(eventid);
            var client = new RestClient("https://graph.facebook.com/me");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "fields=id,name,email,picture&access_token=" + accesstoken, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            int userid = 0;
            var exists = false;
            RegisteredUser registeredUser = new RegisteredUser();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var profile = JsonConvert.DeserializeObject<FacebookModel>(response.Content);
                if (profile != null)
                {
                    var matchuser = db.RegisteredUsers.Where(x => x.Email == profile.email && x.EventId == eventid).ToList();
                    if (matchuser.Count > 0)
                    {
                        exists = true;
                        userid = matchuser.FirstOrDefault().UserId;
                    }
                    else
                    {
                        registeredUser.EventId = eventid;
                        if(profile.name != null)
                        {
                            if(profile.name.Split(' ').Length > 0)
                            {
                                registeredUser.FirstName = profile.name.Split(' ')[0];
                                registeredUser.LastName = profile.name.Split(' ')[1];
                            }
                        }
                        registeredUser.Email = profile.email;
                        registeredUser.Country = "";
                        registeredUser.DateOfRegistration = DateTime.Now;
                        registeredUser.IsDeleted = false;
                        registeredUser.IsRegistered = false;
                        registeredUser.ProfileImage = profile.picture.data.url;
                        registeredUser.ProfileLink = profile.id;
                        registeredUser.ProfileId = profile.id;
                        registeredUser.AuthToken = accesstoken;
                        registeredUser.ReferalId = referralid;
                        registeredUser.Source = "facebook";
                        var allowuser = db.Packages.Select(x => x.No_of_Registration).FirstOrDefault();
                        var reguser = db.RegisteredUsers.Where(x => x.EventId == eventid && x.Ispaid == true).Count();

                        //if (allowuser > reguser)
                        //{
                        //    registeredUser.Ispaid = true;
                        //}
                        //else
                        //{
                        registeredUser.Ispaid = false;
                        //}
                        db.RegisteredUsers.Add(registeredUser);
                        db.SaveChanges();

                        userid = registeredUser.UserId;
                    }
                    var eventurl = db.EventMasters.Find(eventid);
                    var adminsetting = AdminService.GetAdminSetting();
                    var utmsource = adminsetting.Utm_Source;
                    var utmmedium = adminsetting.Utm_Medium;
                    var utmcampaign = eventid;
                    var utmcontent = referralid;
                    var utmterm = adminsetting.Utm_Term;
                    return Json(new { eventurl = eventurl.ResponseURL + "?&eid=" + eventdetail.EventKey + "&r=" + referralid + "&userid=" + userid + "&exists=" + exists + "&accesstoken=" + accesstoken + "&profileid=" + profile.id + "&utm_source=" + utmsource + "&utm_medium=" + utmmedium + "&utm_campaign=" + utmcampaign + "&utm_content=" + utmcontent + "&utm_term=" + utmterm }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { eventurl = "", data = response.Content, error = response.ErrorMessage, ex = response.ErrorException }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { eventurl = "", data = response.Content, error = response.ErrorMessage, ex = response.ErrorException }, JsonRequestBehavior.AllowGet);
        }

        public void ShareOnWall(int eventid, string profileid, string accesstoken, int userid)
        {
            var eventtopost = db.EventMasters.Find(eventid);
            if (eventtopost.Description == null || eventtopost.Description == "")
                return;

            var adminsetting = AdminService.GetAdminSetting();
            var utmsource = adminsetting.Utm_Source;
            var utmmedium = adminsetting.Utm_Medium;
            var utmcampaign = adminsetting.Utm_Campaign;
            var utmcontent = userid;
            var utmterm = eventid;
            eventtopost.Commentary = eventtopost.Commentary.Replace("\r\n", " ");
            string referralLink = "";
            //var UtmArray = eventtopost.DomainName.Split('&');
            if (eventtopost.DomainName.Contains("?"))
            {
                referralLink = eventtopost.DomainName + "&r=" + userid;
                if (!eventtopost.DomainName.Contains("utm_source"))
                {
                    referralLink = referralLink + "&utm_source=" + utmsource;
                }
                if (!eventtopost.DomainName.Contains("utm_medium"))
                {
                    referralLink = referralLink + "&utm_medium=" + utmmedium;
                }
                if (!eventtopost.DomainName.Contains("utm_campaign"))
                {
                    referralLink = referralLink + "&utm_campaign=" + utmcampaign;
                }
                if (!eventtopost.DomainName.Contains("utm_content"))
                {
                    referralLink = referralLink + "&utm_content=" + utmcontent;
                }
                if (!eventtopost.DomainName.Contains("utm_term"))
                {
                    referralLink = referralLink + "&utm_term=" + utmterm;
                }
            }
            else
            {
                referralLink = eventtopost.DomainName + "?r=" + userid;
                if (!eventtopost.DomainName.Contains("utm_source"))
                {
                    referralLink = referralLink + "&utm_source=" + utmsource;
                }
                if (!eventtopost.DomainName.Contains("utm_medium"))
                {
                    referralLink = referralLink + "&utm_medium=" + utmmedium;
                }
                if (!eventtopost.DomainName.Contains("utm_campaign"))
                {
                    referralLink = referralLink + "&utm_campaign=" + utmcampaign;
                }
                if (!eventtopost.DomainName.Contains("utm_content"))
                {
                    referralLink = referralLink + "&utm_content=" + utmcontent;
                }
                if (!eventtopost.DomainName.Contains("utm_term"))
                {
                    referralLink = referralLink + "&utm_term=" + utmterm;
                }
            }
            //referralLink = GoogleUrlShortener(referralLink);
            eventtopost.Commentary = eventtopost.Commentary;
            eventtopost.Description = eventtopost.Description.Replace("\r\n", " ");
            //eventtopost.ArticalTitle = eventtopost.ArticalTitle.Replace("\r\n", " ");

            //eventtopost.Description = "Did you know we have over 425,000 attendees across our portfolio of 80 events per year? Our goal is to nurture professional communities through conferences, workshops and face-to-face meetings. Check out our 2019 calendar www.dmgevents.com/events hashtag#dmgevents hashtag#eventprofs";
            var client = new RestClient("https://api.linkedin.com/v2/ugcPosts");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "29d7e874-187b-4b65-a360-be9c8879f6d6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + accesstoken);
            request.AddHeader("Content-Type", "application/json");
            string RequestString = "";
            RequestString = RequestString + "{";
            RequestString = RequestString + "\"author\": \"urn:li:person:" + profileid + "\",";
            RequestString = RequestString + "\"lifecycleState\": \"PUBLISHED\",";
            RequestString = RequestString + "\"specificContent\": {";
            RequestString = RequestString + "\"com.linkedin.ugc.ShareContent\": {";
            RequestString = RequestString + "\"shareCommentary\": {";
            RequestString = RequestString + "\"text\": \"" + eventtopost.Commentary + "\"";
            RequestString = RequestString + "},";
            RequestString = RequestString + "\"shareMediaCategory\": \"ARTICLE\",";
            RequestString = RequestString + "\"media\": [";
            RequestString = RequestString + "{";
            RequestString = RequestString + "\"status\": \"READY\",";
            RequestString = RequestString + "\"originalUrl\": \"" + referralLink + "\"";
            RequestString = RequestString + "}";
            RequestString = RequestString + "]";
            RequestString = RequestString + "}";
            RequestString = RequestString + "},";
            RequestString = RequestString + "\"visibility\": {";
            RequestString = RequestString + "\"com.linkedin.ugc.MemberNetworkVisibility\": \"CONNECTIONS\"";
            RequestString = RequestString + "}";
            RequestString = RequestString + "}";

            //request.AddParameter("undefined", "{\r\n    \"author\": \"urn:li:person:" + profileid + "\",\r\n    \"lifecycleState\": \"PUBLISHED\",\r\n    \"specificContent\": {\r\n        \"com.linkedin.ugc.ShareContent\": {\r\n            \"shareCommentary\": {\r\n                \"text\": \"" + eventtopost.Description + "\"\r\n            },\r\n            \"shareMediaCategory\": \"NONE\"\r\n        }\r\n    },\r\n    \"visibility\": {\r\n        \"com.linkedin.ugc.MemberNetworkVisibility\": \"PUBLIC\"\r\n    }\r\n}", ParameterType.RequestBody);
            request.AddParameter("undefined", RequestString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var test = response.Content;
        }

        [HttpPost]
        public ActionResult GetRegisteredUser(int UserId)
        {
            var registereduser = RegisteredUserService.GetRegisteredUserById(UserId);
            if (registereduser == null)
                return Json(new { success = false });

            RegisteredUserModel model = new RegisteredUserModel();
            model.UserId = registereduser.UserId;
            model.EventId = (int)registereduser.EventId;
            model.FirstName = registereduser.FirstName;
            model.LastName = registereduser.LastName;
            model.Email = registereduser.Email;
            model.Country = registereduser.Country;
            model.ProfileLink = registereduser.ProfileLink;
            model.ProfileImage = registereduser.ProfileImage;
            return Json(new { success = true, user = model });
        }
        [HttpPost]
        public ActionResult UpdateUserFromCRM(int UserId, string firstname, string lastname, string emailid, string companyname, string jobtitle, string phone, string crmregid, string allowpost,
            string senioritylevel, string primaryjob, string natureofbusiness, string country, string topicofinterest, string registeredforglobal,
            bool checkbox1, bool checkbox2, bool checkbox3, bool checkbox4, bool iszoomevent,bool isbigmarkerevent)
        {
            try
            {
                var registeredUser = db.RegisteredUsers.Find(UserId);
                registeredUser.CRM_FirstName = firstname;
                registeredUser.CRM_LastName = lastname;
                registeredUser.CRM_EmaiId = emailid;
                registeredUser.CRM_CompanyName = companyname;
                registeredUser.CRM_JobTitle = jobtitle;
                registeredUser.CRM_RegistrationId = crmregid;
                registeredUser.IsDeleted = false;
                registeredUser.IsRegistered = true;
                db.Entry(registeredUser).State = EntityState.Modified;
                var allowuser = db.Packages.Select(x => x.No_of_Registration).FirstOrDefault();
                var reguser = db.RegisteredUsers.Where(x => x.EventId == registeredUser.EventId && x.Ispaid == true).Count();
                var events = EventService.GetEventById(Convert.ToInt32(registeredUser.EventId));
                var availablecredits = CustomerService.GetCustomerAvailableCredit(events.CustomerId);
                if (availablecredits > 0)
                {
                    registeredUser.Ispaid = true;
                }
                else
                {
                    registeredUser.Ispaid = false;
                }
                db.SaveChanges();


                if (allowpost == "true" || allowpost == null || allowpost == "undefined")
                {
                    ShareOnWall(Convert.ToInt32(registeredUser.EventId), registeredUser.ProfileId, registeredUser.AuthToken, registeredUser.UserId);
                }

                if (iszoomevent)
                {
                    db.AddZoomUserDetails(0, registeredUser.EventId, registeredUser.UserId, senioritylevel, primaryjob, natureofbusiness, country,
                        topicofinterest, registeredforglobal, checkbox1, checkbox2, checkbox3, checkbox4, null, null);

                    var zoomtoken = db.SpGetEventToken(events.EventId);
                    try
                    {
                        if(zoomtoken != null)
                        {
                            ZoomModel objZoomModel = new ZoomModel();
                            objZoomModel.id = events.WebinarId;
                            objZoomModel.token = zoomtoken.FirstOrDefault();
                            objZoomModel.payload.first_name = registeredUser.FirstName;
                            objZoomModel.payload.last_name = registeredUser.LastName;
                            objZoomModel.payload.email = registeredUser.Email;
                            objZoomModel.payload.org = registeredUser.Company;
                            objZoomModel.payload.phone = phone;
                            objZoomModel.payload.country = country;
                            var jsonbody = JsonConvert.SerializeObject(objZoomModel);
                            var client = new RestClient("https://connect-zoom.eventnx.com/api/webinar/register");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.PUT);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", jsonbody, ParameterType.RequestBody);
                            IRestResponse<ZoomResponse> response = client.Execute<ZoomResponse>(request);
                            var zoomresponse = response.Data;
                            if (zoomresponse.token != null)
                            {
                                EventService.UpdateZoomTokenEvent(zoomresponse.token,events.CustomerId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (isbigmarkerevent)
                {

                    db.AddBigMarkerUserDetails(0, registeredUser.EventId, registeredUser.UserId, senioritylevel, primaryjob, natureofbusiness, country,
                        topicofinterest, registeredforglobal, checkbox1, checkbox2, checkbox3, checkbox4, null, null);

                    var zoomtoken = db.SpGetBigMarkerEventToken(events.EventId);
                    try
                    {
                        if (zoomtoken != null)
                        {
                            BigMarkerRequest bigMarkerModel = new BigMarkerRequest();
                            bigMarkerModel.Id = events.BigMarkerWebinairId;
                            bigMarkerModel.Token = zoomtoken.FirstOrDefault();
                            bigMarkerModel.Payload.FirstName = registeredUser.FirstName;
                            bigMarkerModel.Payload.LastName = registeredUser.LastName;
                            bigMarkerModel.Payload.Email = registeredUser.Email;
                            bigMarkerModel.Payload.CustomFields = "";
                            bigMarkerModel.Payload.UtmBmcrSource = registeredUser.Source;
                            bigMarkerModel.Payload.CustomUserId = registeredUser.UserId.ToString();
                            var jsonbody = JsonConvert.SerializeObject(bigMarkerModel);
                            var client = new RestClient("https://connect-zoom.eventnx.com/api/bigmarker/register");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.PUT);
                            request.AddHeader("Content-Type", "application/json"); 
                            request.AddParameter("application/json", jsonbody, ParameterType.RequestBody);
                            IRestResponse<BigMarkerResponse> response = client.Execute<BigMarkerResponse>(request);
                            var bigmarkeresponse = response.Data;
                            if (bigmarkeresponse.Token != null)
                            {
                                EventService.UpdateBigMarkerTokenEvent(bigmarkeresponse.Token);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return Json(new { success = true, message = "saved Data successfully" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Could not saved Data"+ ex.ToString()
                });
            }
        }



        //[HttpPost]
        public ActionResult GetRegisteredUserByUserId(int UserId)
        {
            try
            {
                var registeredUser = db.RegisteredUsers.Find(UserId);
                var eventid = registeredUser.EventId;

                var eventtopost = db.EventMasters.Find(eventid);
                var adminsetting = AdminService.GetAdminSetting();
                var utmsource = adminsetting.Utm_Source;
                var utmmedium = adminsetting.Utm_Medium;
                var utmcampaign = adminsetting.Utm_Campaign;
                var utmcontent = UserId;
                var utmterm = eventid;
                eventtopost.Commentary = eventtopost.Commentary.Replace("\r\n", " ");
                var referralLink = "";
                if (eventtopost.DomainName.Contains("?"))
                {
                    referralLink = eventtopost.DomainName;// + "&r=" + UserId;
                    if (!eventtopost.DomainName.Contains("utm_source"))
                    {
                        referralLink = referralLink + "&utm_source=" + utmsource;
                    }
                    if (!eventtopost.DomainName.Contains("utm_medium"))
                    {
                        referralLink = referralLink + "&utm_medium=" + utmmedium;
                    }
                    if (!eventtopost.DomainName.Contains("utm_campaign"))
                    {
                        referralLink = referralLink + "&utm_campaign=" + utmcampaign;
                    }
                    if (!eventtopost.DomainName.Contains("utm_content"))
                    {
                        referralLink = referralLink + "&utm_content=" + utmcontent;
                    }
                    if (!eventtopost.DomainName.Contains("utm_term"))
                    {
                        referralLink = referralLink + "&utm_term=" + utmterm;
                    }
                    referralLink = referralLink + "&r=" + UserId;
                }
                else
                {
                    referralLink = eventtopost.DomainName + "?test=" + UserId;
                    if (!eventtopost.DomainName.Contains("utm_source"))
                    {
                        referralLink = referralLink + "&utm_source=" + utmsource;
                    }
                    if (!eventtopost.DomainName.Contains("utm_medium"))
                    {
                        referralLink = referralLink + "&utm_medium=" + utmmedium;
                    }
                    if (!eventtopost.DomainName.Contains("utm_campaign"))
                    {
                        referralLink = referralLink + "&utm_campaign=" + utmcampaign;
                    }
                    if (!eventtopost.DomainName.Contains("utm_content"))
                    {
                        referralLink = referralLink + "&utm_content=" + utmcontent;
                    }
                    if (!eventtopost.DomainName.Contains("utm_term"))
                    {
                        referralLink = referralLink + "&utm_term=" + utmterm;
                    }
                    referralLink = referralLink + "&r=" + UserId;
                }

                referralLink = System.Web.HttpUtility.UrlEncode(referralLink);
                var register_userlist = new RegisteredUserModel();

                var client = new RestClient("https://api-ssl.bitly.com/v3/shorten?access_token=96cc216b40bdbce6e032fcaeb2505ff997986706&longUrl=" + referralLink);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    dynamic tmp = JsonConvert.DeserializeObject(response.Content);
                    referralLink = (string)tmp.data.url;
                }

                List<UserMasterModel> userlist = new List<UserMasterModel>();
                var regusers = RegisteredUserService.Eventwiseuser("", "", "", (int)eventid, eventtopost.CustomerId, 0).ToList().Where(x=>x.UserId != UserId);
                foreach (var user in regusers)
                {
                    UserMasterModel model = new UserMasterModel();
                    model.UserId = user.UserId;
                    model.FullName = user.FirstName + " " + user.LastName;
                    model.ReferredBy = user.ReferredBy;
                    model.Email = user.Email;
                    model.Country = user.Country;
                    model.DomainName = user.DomainName;
                    model.EventId = user.EventId;
                    model.EventName = user.EventName;
                    model.EventStartDate = user.EventStartDate;
                    model.EventEndDate = user.EventEndDate;
                    model.DateOfRegistration = user.DateOfRegistration;
                    model.ReferralCount = Convert.ToInt32(user.ReferralCount);
                    model.ConnectionCount = Convert.ToInt32(user.ConnectionCount);
                    model.ProfileImage = user.ProfileImage;
                    model.visitorcount = user.VisitorCount;
                    model.CRM_CompanyName = user.CRM_CompanyName != null ? user.CRM_CompanyName : "";
                    model.CRM_JobTitle = user.CRM_JobTitle != null ? user.CRM_JobTitle : "";
                    model.CRM_RegistrationId = user.CRM_RegistrationId;
                    model.Source = user.Source;
                    userlist.Add(model);
                }
                return Json(new { RegisteredUser = userlist, success = true, ReferralLink = referralLink }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Could not saved Data"
                });
            }
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisteredUserModel model)
        {
            var registereduser = db.RegisteredUsers.Find(model.UserId);
            var eventdata = EventService.GetEventByKey(model.EventKey);
            registereduser.EventId = eventdata.EventId;
            registereduser.FirstName = model.FirstName;
            registereduser.LastName = model.LastName;
            registereduser.Email = model.Email;
            registereduser.DateOfRegistration = DateTime.Now;
            registereduser.IsDeleted = false;
            registereduser.IsRegistered = true;
            var allowuser = db.Packages.Select(x => x.No_of_Registration).FirstOrDefault();
            var reguser = db.RegisteredUsers.Where(x => x.EventId == eventdata.EventId && x.Ispaid == true).Count();
            if (allowuser >= reguser)
            {
                registereduser.Ispaid = true;
            }
            else
            {
                registereduser.Ispaid = true;
            }

            db.Entry(registereduser).State = EntityState.Modified;
            db.SaveChanges();

            ShareOnWall(eventdata.EventId, model.profileid, model.accesstoken, registereduser.UserId);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult GetEventDetails(string EventKey,string referralid = "")
        {
            var eventdata = EventService.GetEventByKey(EventKey);

            if(referralid !="")
            {
                var visitorstate = new VisitorState();
                visitorstate.IPaddress = "";
                visitorstate.CreatedDate = DateTime.Now;
                visitorstate.ReferralId = Convert.ToInt32(referralid);
                RegisteredUserService.InsertVisitorState(visitorstate);

            }
            return Json(new { success = true, form = eventdata.FormBuilderCode,eventid=eventdata.EventId, buttonUrl = eventdata.ButtonURL, fbButtonUrl = eventdata.FbButtonURL,domainname= eventdata.DomainName });
        }

        [HttpPost]
        public ActionResult SubmitData(string collection, string data)
        {
            try
            {
                _mongodbHelper.SaveData(collection, data);
                return Json(new { success = true, message = "Data saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Could not saved Data"
                });
            }
        }

       
        public ActionResult DemoLogin()
        {
            return View();
        }
        
        
        public ActionResult DemoRegister()
        {
            return View();
        }

    

        //public static string GoogleUrlShortener(string longUrl)
        //{
        //    using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rebrandly.com") })
        //    {
        //        httpClient.DefaultRequestHeaders.Add("apikey", "YOUR_API_KEY");
        //        httpClient.DefaultRequestHeaders.Add("workspace", "YOUR_WORKSPACE_ID");

        //        var body = new StringContent(
        //            JsonConvert.SerializeObject(payload), UnicodeEncoding.UTF8, "application/json");

        //        using (var response = await httpClient.PostAsync("/v1/links", body))
        //        {
        //            response.EnsureSuccessStatusCode();

        //            var link = JsonConvert.DeserializeObject<dynamic>(
        //                await response.Content.ReadAsStringAsync());

        //            Console.WriteLine($"Long URL was {payload.destination}, short URL is {link.shortUrl}");
        //        }
        //    }
        //}
    }

    public class XMLSerialize
    {
        public static string Serialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }

    public class XMLDeserialize
    {
        public static T Deserialize<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}