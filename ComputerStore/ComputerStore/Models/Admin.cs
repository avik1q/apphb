using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models
{
    public class Admin
    {
        [Key]
        [Required(ErrorMessage = "נא למלא שדה השם משתמש")]
        public string Name { get; set; }
        [Required(ErrorMessage = "נא למלא שדה הסיסמה")]
        public string Password { get; set; }
    }
}