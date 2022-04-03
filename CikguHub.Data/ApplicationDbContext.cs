using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CikguHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Enrolment> Enrolments { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            modelBuilder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
                    {
                        new IdentityRole<int> {
                            Id = 1,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new IdentityRole<int> {
                            Id = 2,
                            Name = "Member",
                            NormalizedName = "MEMBER"
                        }
                    });

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    Name = "Admin",
                    UserName = "admin@cikguhub.my",
                    NormalizedUserName = "admin@cikguhub.my".ToUpper(),
                    Email = "admin@cikguhub.my",
                    NormalizedEmail = "admin@cikguhub.my".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin"),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser
                {
                    Id = 2,
                    Name = "Member",
                    UserName = "member@cikguhub.my",
                    NormalizedUserName = "member@cikguhub.my".ToUpper(),
                    Email = "member@cikguhub.my",
                    NormalizedEmail = "member@cikguhub.my".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "member"),
                    SecurityStamp = string.Empty
                }
            ); ; ;

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1, // for admin username
                    UserId = 1  // for admin role
                },
                new IdentityUserRole<int>
                {
                    RoleId = 2, // for member username
                    UserId = 2  // for member role
                }
            );
        }
    }
}
