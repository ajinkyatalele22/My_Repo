using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ajinkya_Repository.Models
{
    [MetadataType(typeof(Registration_Model))]
    public partial class Registration
    {
        public string ConfirmPassword { get; set; }
    }

    public class Registration_Model
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name Is Required ...")]
        public string First_Name { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Is Required ...")]
        public string Last_Name { get; set; }
        
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Of Birth Is Required ...")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> Date_Of_Birth { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID Is Required . . . ")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email ID")]
        public string EmailID { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile Number Is Required . . . ")]
        [RegularExpression(@"^((?!(0))[0-9]{10})$", ErrorMessage = "Invalid Mobile Number. . . ")]
        public string Mobile_Number { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Is Required . . . ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmed Password Must be Same . . . ")]
        public string ConfirmPassword { get; set; }
    }
}