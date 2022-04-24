using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vjezba.Model
{
    public enum MeetingType
    {
        InPerson,
        VideoCall
    }
    public enum MeetingStatus
    {
        Scheduled,
        Cancelled
    }
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public MeetingType MeetingType { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public MeetingStatus MeetingStatus { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
