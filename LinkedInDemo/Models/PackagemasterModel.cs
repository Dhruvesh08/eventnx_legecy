using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedInDemo.Models
{
    public class PackagemasterModel
    {
        public int PackageId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Detail is required")]
        public string Detail { get; set; }
        [Required(ErrorMessage = "Cost is required")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Validity is required")]
        public int Validity { get; set; }
        [Required(ErrorMessage = "No Of Registration is required ")]
        public int No_of_Registration { get; set; }
        [Required (ErrorMessage ="Cost Per Registration is required ")]
        public decimal Cost_per_Registration { get; set; }
        [Required(ErrorMessage = " No Of Events is required ")]
        public int No_of_Events { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime Createddate { get; set; }
        public System.DateTime Updateddate { get; set; }
    }
}