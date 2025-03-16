using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class ArtEvent
    {
        public int ArtEventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ArtOrganizationId { get; set; }
        public virtual ArtOrganization ArtOrganization { get; set; }
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
