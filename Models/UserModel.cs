using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Common;
using System.ComponentModel.DataAnnotations;

namespace ElectrosLtdApplication.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Roles = new List<Role>();
           
        }
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public int Mobile { get; set; }
        [Required(ErrorMessage = "Email can not be blank")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not Valid")]
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public int TownId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Pin { get; set; }

        public List<Role> Roles { get; set; }

      
        

        public virtual Order Order { get; set; }
        public virtual Town Town { get; set; }
        [ForeignKey("RoleId")]
        public virtual ICollection<Role> Role { get; set; }



    }
}