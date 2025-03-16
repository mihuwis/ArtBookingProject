using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class PriceEntry
    {
        public int PriceEntryId { get; set; }
        public int PriceListId { get; set; }
        public int AreaId { get; set; }
        public decimal Price { get; set; }
        public virtual PriceList PriceList { get; set; }
        public virtual Area Area { get; set; }
    }
}
