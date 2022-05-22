using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model
{
    public class Attachement
    {
        [Key]
        public int ID { get; set; }
        public string FilePath { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientID { get; set; }
        public Client Client { get; set; }
    }
}
