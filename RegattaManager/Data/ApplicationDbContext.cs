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

            builder.Entity<Startboat>()
                .HasOne(r => r.Regatta)
                .WithMany(rs => rs.Startboats)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaStartingFee>()
                .HasKey(t => new { t.StartingFeeId, t.RegattaId });

            builder.Entity<RegattaStartingFee>()
                .HasOne(r => r.Regattas)
                .WithMany(rs => rs.RegattaStartingFees)
                .HasForeignKey(rm => rm.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaStartingFee>()
                .HasOne(s => s.StartingFees)
                .WithMany(sr => sr.RegattaStartingFees)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCampingFee>()
                .HasKey(t => new { t.CampingFeeId, t.RegattaId });

            builder.Entity<RegattaCampingFee>()
                .HasOne(r => r.Regattas)
                .WithMany(rs => rs.RegattaCampingFees)
                .HasForeignKey(rm => rm.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCampingFee>()
                .HasOne(c => c.CampingFees)
                .WithMany(cr => cr.RegattaCampingFees)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaOldclass>()
                .HasKey(t => new { t.OldclassId, t.RegattaId });

            builder.Entity<RegattaOldclass>()
                .HasOne(r => r.Regattas)
                .WithMany(ro => ro.RegattaOldclasses)
                .HasForeignKey(rr => rr.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaOldclass>()
                .HasOne(o => o.Oldclasses)
                .WithMany(ro => ro.RegattaOldclasses)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCompetition>()
                .HasKey(t => new { t.CompetitionId, t.RegattaId });

            builder.Entity<RegattaCompetition>()
                .HasOne(r => r.Regattas)
                .WithMany(c => c.RegattaCompetitions)
                .HasForeignKey(rr => rr.RegattaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RegattaCompetition>()
                .HasOne(b => b.Competitions)
                .WithMany(c => c.RegattaCompetitions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReportedStartboatMember>()
              .HasKey(t => new { t.ReportedStartboatId, t.MemberId });

            builder.Entity<ReportedStartboatMember>()
                .HasOne(sm => sm.ReportedStartboat)
                .WithMany(s => s.ReportedStartboatMembers)
                .HasForeignKey(sm => sm.ReportedStartboatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReportedStartboatMember>()
                .HasOne(sm => sm.Member)
                .WithMany(m => m.ReportedStartboatMembers)
                .HasForeignKey(sm => sm.MemberId);

            builder.Entity<ReportedStartboatStandby>()
               .HasKey(t => new { t.ReportedStartboatId, t.MemberId });

            builder.Entity<ReportedStartboatStandby>()
                .HasOne(sm => sm.ReportedStartboat)
                .WithMany(s => s.ReportedStartboatStandbys)
                .HasForeignKey(sm => sm.ReportedStartboatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReportedStartboatStandby>()
                .HasOne(sm => sm.Member)
                .WithMany(m => m.ReportedStartboatStandbys)
                .HasForeignKey(sm => sm.MemberId);

            builder.Entity<ReportedStartboat>()
                .HasOne(r => r.Regatta)
                .WithMany(rs => rs.ReportedStartboats)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<RegattaManager.Models.Boatclass> Boatclasses { get; set; }

        public DbSet<RegattaManager.Models.Raceclass> Raceclasses { get; set; }

        public DbSet<RegattaManager.Models.Oldclass> Oldclasses { get; set; }

        public DbSet<RegattaManager.Models.Racestatus> Racestati { get; set; }

        public DbSet<RegattaManager.Models.Club> Clubs { get; set; }

        public DbSet<RegattaManager.Models.Regatta> Regattas { get; set; }

        public DbSet<RegattaManager.Models.Race> Races { get; set; }

        public DbSet<RegattaManager.Models.Member> Members { get; set; }

        public DbSet<RegattaManager.Models.Startboat> Startboats { get; set; }

        public DbSet<RegattaManager.Models.Startboatstatus> Startboatstati { get; set; }

        public DbSet<RegattaManager.Models.StartboatMember> StartboatMembers { get; set; }

        public DbSet<RegattaManager.Models.Water> Waters { get; set; }

        public DbSet<RegattaManager.Models.CampingFee> CampingFees { get; set; }

        public DbSet<RegattaManager.Models.RegattaCampingFee> RegattaCampingFees { get; set; }

        public DbSet<RegattaManager.Models.StartingFee> StartingFees { get; set; }

        public DbSet<RegattaManager.Models.RegattaOldclass> RegattaOldclasses { get; set; }

        public DbSet<RegattaManager.Models.Competition> Competitions { get; set; }

        public DbSet<RegattaManager.Models.RegattaStartingFee> RegattaStartingFees { get; set; }

        public DbSet<RegattaManager.Models.RegattaCompetition> RegattaCompetitions { get; set; }
    }
}
