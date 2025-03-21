using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessModel.Entities;

namespace BusinessModel.Data
{
    public class ArtBookingDBContext : DbContext
    {
        public ArtBookingDBContext(DbContextOptions<ArtBookingDBContext> options) : base(options) { }


        public DbSet<Area> Areas { get; set; }
        public DbSet<ArtEvent> ArtEvents { get; set; }
        public DbSet<ArtOrganization> ArtOrganizations { get; set; }
        public DbSet<PriceEntry> PriceEntries { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Venue> Venues { get; set; }



    }
}
