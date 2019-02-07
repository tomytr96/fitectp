using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.ViewModels
{
    public class PersonVM
    {
        #region PersonCS
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters."), Column("FirstName"), Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstMidName;
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Required"), DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Upload)]
        public string ImagePath { get; set; } 
        #endregion

        public string Role { get; set; }
        public string ConfirmPassword { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageFile { get; set; }

    }
}