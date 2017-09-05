using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegattaManager.Models;

namespace RegattaManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<StartboatMember>()
                .HasKey(t => new { t.StartboatId, t.MemberId });

            builder.Entity<StartboatMember>()
                .HasOne(sm => sm.Startboat)
                .WithMany(s => s.StartboatMembers)
                .HasForeignKey(sm => sm.StartboatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StartboatMember>()
                .HasOne(sm => sm.Member)
                .WithMany(m => m.StartboatMembers)
                .HasForeignKey(sm => sm.MemberId);

            builder.Entity<Startboat>()
                .HasOne(r => r.Race)
                .WithMany(rr => rr.Startboats)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<RegattaManager.Models.Boatclass> Boatclass { get; set; }

        public DbSet<RegattaManager.Models.Raceclass> Raceclass { get; set; }

        public DbSet<RegattaManager.Models.Oldclass> Oldclass { get; set; }

        public DbSet<RegattaManager.Models.Racestatus> Racestatus { get; set; }

        public DbSet<RegattaManager.Models.Club> Club { get; set; }

        public DbSet<RegattaManager.Models.Regatta> Regatta { get; set; }
    }
}
