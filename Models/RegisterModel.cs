using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ElectrosLtdApplication.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            Roles = new List<Role>();
           
        }
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name can not be blank")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname can not be blank")]
        public string Surname { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public int Mobile { get; set; }

        [Required(ErrorMessage = "Email can not be blank")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not Valid")]
        [Display(Name = "User name")]
        [Remote("doesEmailExist", "Register", HttpMethod = "POST", ErrorMessage = "email already exists. Please enter a different email.")]
        public string Email { get; set; }

        public DateTime DOB { get; set; }
        public int TownId { get; set; }


        [Required]
        [Display(Name = "User name")]
        [Remote("doesUserNameExist", "Register", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string Username { get; set; }

        [StringLength(20, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }
        public int Pin { get; set; }

        public List<Role> Roles { get; set; }

      
        

        public virtual Order Order { get; set; }
        public virtual Town Town { get; set; }
        [ForeignKey("RoleId")]
        public virtual ICollection<Role> Role { get; set; }

    }
}