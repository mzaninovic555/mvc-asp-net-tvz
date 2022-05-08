using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model
{
    public class Client
    {
        [Key]
        public int ID { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(30, ErrorMessage = "Maximum length allowed is 30 characters")]
        public string Email { get; set; }
        public char Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Range(0, 100, ErrorMessage = "Working experience has to be within 0-100")]
        public int? WorkingExperience { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityID { get; set; }
        public City? City { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<Meeting>? Meetings { get; set; }

    }
}
