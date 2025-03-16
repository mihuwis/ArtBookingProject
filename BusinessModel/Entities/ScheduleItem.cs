using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class ScheduleItem
    {
        public int ScheduleItemId { get; set; }
        public int ArtEventId { get; set; }
        public int VenueId { get; set; }
        public DateTime EventDate { get; set; }
        public virtual ArtEvent ArtEvent { get; set; }
        public virtual Venue Venue { get; set; }
    }

}
