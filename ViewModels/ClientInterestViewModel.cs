using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.ViewModels
{
    public class ClientInterestViewModel
    {
    SSDDatingEntities21 db = new SSDDatingEntities21();
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "User Name")]
        public string userName { get; set; }
        [Display(Name = "Birthdate")]
        public DateTime birthdate { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "city")]
        public string city { get; set; }
        [Display(Name = "latitude")]
        public string latitude { get; set; }
        [Display(Name = "longitude")]
        public string longitude { get; set; }
        [Display(Name = "Province")]
        public string province { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Display(Name = "User ID")]
        public string userId { get; set; }
        [Display(Name = "Interests")]
        public List<string> interests { get; set; }
    }
}