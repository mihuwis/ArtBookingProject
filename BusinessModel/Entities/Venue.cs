using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
        public virtual ICollection<PriceList> PriceLists { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
    }

}