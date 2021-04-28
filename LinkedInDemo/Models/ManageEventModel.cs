using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class ManageEventModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public int RegisteredUser { get; set; }
        public int? ReferredUser { get; set; }

    }
}