using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class Seat
    {
        public int SeatId { get; set; }
        public int AreaId { get; set; }
        public int SeatNumber { get; set; }
        public virtual Area Area { get; set; }
    }

}
