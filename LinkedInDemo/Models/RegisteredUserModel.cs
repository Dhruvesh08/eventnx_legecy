using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LinkedInDemo.Models
{
    public class RegisteredUserModel
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        public string Source { get; set; }
        public string EventName { get; set; }
        public System.DateTime EventStartDate { get; set; }
        public System.DateTime EventEndDate { get; set; }
        public string DomainName { get; set; }
        public string accesstoken { get; set; }
        public string profileid { get; set; }
        public string Company { get; set; }
        public string JobDesignation { get; set; }
        public string ButtonURL { get; set; }
        public string Image { get; set; }
        public string ImageName { get; set; }
        public string MobileNumber { get; set; }
        //[DataType(DataType.Date)]
        public System.DateTime DateOfRegistration { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRegistered { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileLink { get; set; }
        public SelectList domainlist { get; set; }
        public SelectList eventlist { get; set; }
        public int RegisteredUser { get; set; }
        public int Linkedinusers { get; set; }
        public int Facebookusers { get; set; }
        public decimal Creditrequired { get; set; }
        public int TotalReferences { get; set; }
        public int CustomerId { get; set; }
        public int ReferralId { get; set; }
        public Nullable<int> ReferralCount { get; set; }
        public int ConnectionCount { get; set; }
        public string EventKey { get; set; }
        public int usercount { get; set; }
        public string CRM_CompanyName { get; set; }
        public string CRM_JobTitle { get; set; }
        public List<Event_RegisterUserList> Event_RegisterUserList { get; set; }
    }
    public class Event_RegisterUserList
    {
        public string ProfileImage { get; set; }
        public string FullName { get; set; }
        public int id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public bool IsRegistered { get; set; }
    }

    public class Payload
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string industry { get; set; }
        public string org { get; set; }
        public string job_title { get; set; }
        public string purchasing_time_frame { get; set; }
        public string role_in_purchase_process { get; set; }
        public string no_of_employees { get; set; }
        public string comments { get; set; }
        public string registrant_id { get; set; }
        public long id { get; set; }
        public string topic { get; set; }
        public DateTime start_time { get; set; }
        public string join_url { get; set; }
    }

    public class ZoomModel
    {
        public ZoomModel()
        {
            payload = new Payload();
        }
        public string id { get; set; }
        public string token { get; set; }
        public Payload payload { get; set; }
    }


    public class ZoomResponse
    {
        public ZoomResponse()
        {
            payload = new Payload();
        }
        public string token { get; set; }
        public Payload payload { get; set; }
    }

    public partial class BigMarkerRequest
    {
        public BigMarkerRequest()
        {
            Payload = new BigmarkerPayload();
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("payload")]
        public BigmarkerPayload Payload { get; set; }
    }

    public partial class BigmarkerPayload
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("custom_fields")]
        public string CustomFields { get; set; }

        [JsonProperty("utm_bmcr_source")]
        public string UtmBmcrSource { get; set; }

        [JsonProperty("custom_user_id")]
        public string CustomUserId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public partial class BigMarkerResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("payload")]
        public BigmarkerPayload Payload { get; set; }
    }

}