using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int ScheduleItemId { get; set; }
        public int AreaId { get; set; }
        public int? SeatId { get; set; }
        public decimal Price { get; set; }
        public virtual ScheduleItem ScheduleItem { get; set; }
        public virtual Area Area { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
