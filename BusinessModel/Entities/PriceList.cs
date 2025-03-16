using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class PriceList
    {
        public int PriceListId { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual ICollection<PriceEntry> PriceEntries { get; set; }
    }

}
