using System.ComponentModel.DataAnnotations;

namespace Vjezba.Model
{
    public class City
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
