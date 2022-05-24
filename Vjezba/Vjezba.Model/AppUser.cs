using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class AppUser : IdentityUser
    {
        [Required]
        [RegularExpression("[0-9]{13}")]
        public string JMBG { get; set; }

        [Required]
        [RegularExpression("[0-9]{11}")]
        public string OIB { get; set; }
    }
}
