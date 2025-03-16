using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class Area
    {
        public int AreaId { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
