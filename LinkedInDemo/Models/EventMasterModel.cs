using System;
using System.ComponentModel.DataAnnotations;

namespace LinkedInDemo.Models
{
    public class EventMasterModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        [Required(ErrorMessage = "Event Url is required")]
        [DataType(DataType.Url,ErrorMessage = "Enter valid Url")]
        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessage = "Enter valid Url")]
        public string EventURL { get; set; }
        [Required(ErrorMessage = "Event Name  is required")]
        public string EventName { get; set; }
        
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Comment is required")]
        [StringLength(1000, ErrorMessage = " Comment must not be more than 1000 char")]
        
        public string Commentary { get; set; }
        public string ArticalUrl { get; set; }
        //[Required(ErrorMessage = "Article Title is required")]
        public string ArticalTitle { get; set; }
        
        [Required(ErrorMessage = " Page Url is required")]
        public string ResponseURL { get; set; }
        public int RegisteredUsers { get; set; }
        public int CustomerId { get; set; }
        public decimal AvailableCredits { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
        public string ImageName { get; set; }
        [Required(ErrorMessage = "Event start date is required")]
        public string EventStartDate { get; set; }
        [Required(ErrorMessage = "Event End date is required")]
        public string EventEndDate { get; set; }
        public string EventStartDateString { get; set; }
        public string EventEndDateString { get; set; }
        [Required(ErrorMessage = "Contact Person Name is required")]
        //[RegularExpression(@"[~`!@#$%^&*()+=|\\{}':;.,<>/?[\]""_-]", ErrorMessage = "Invalid Character in Contact Person Name")]
        public string ContactPersonName { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "Please enter valid Contact number")]
        [MaxLength(15,ErrorMessage = "Phone should be of less then equal to 14 digits")]
        [MinLength(10, ErrorMessage = "Phone should be of less then equal to 14 digits")]
        public string ContactPersonPhone { get; set; }
        [Required(ErrorMessage = "Email  is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ContactPersonEmail { get; set; }
        public Guid? EventKey { get; set; }
        public string ButtonURL { get; set; }
        public string FbButtonURL { get; set; }
        public string GoogleButtonURL { get; set; }
        public string ProfileImage { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Referralusers { get; set; }
        public int Linkedinusers { get; set; }
        public int Facebookusers { get; set; }
        public string ZoomToken { get; set; }
        public string WebinarId { get; set; }

        public string BigMarkerWebinairId { get; set; }
        public string BigMarkerToken { get; set; }
        public string ZoomUUID { get; set; }
        public string Channel { get; set; }
        public string HeySummitToken { get; set; }
        public string HeySummitWebinarId { get; set; }
    }

   

}