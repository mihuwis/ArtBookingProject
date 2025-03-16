using BusinessModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Entities
{
    public class User
    {

        public int UserId { get; set; }

        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public USerRole? UserRole { get; set; }

        public int? ArtOrganizationId { get; set; }

        public virtual ArtOrganization? ArtOrganization { get; set; }

    }
}
