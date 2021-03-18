using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCalendar.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCalendar.Data
{
    public class Context: IdentityDbContext<CalendarUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Family> Families { get; set; }
        
        //setting many-to-many relationship between user (CalendarUser) and users's groups(Families)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarUser>()
                .HasMany(u => u.FamilyName)
                .WithMany(p => p.FamilyMembers)
                .UsingEntity<FamiliesCalendarUsers>(
                    j => j
                        .HasOne(pt => pt.Family)
                        .WithMany(t => t.FamiliesCalendarUsers)
                        .HasForeignKey(pt => pt.FamilyId),
                    j => j
                        .HasOne(pt => pt.CalendarUser)
                        .WithMany(p => p.FamiliesCalendarUsers)
                        .HasForeignKey(pt => pt.CalendarUserId),
                    j =>
                    {
                        j.HasKey(t => new { t.CalendarUserId, t.FamilyId });
                    });
        }

    }
}
