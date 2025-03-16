using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class ArtOrganization
    {

        public int ArtOrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
