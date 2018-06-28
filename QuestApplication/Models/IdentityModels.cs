using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestApplication.Entities;

namespace QuestApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Teacher Teacher { get; set; }
        public virtual Student Student { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
               .HasOptional(s => s.Teacher)
               .WithRequired(a => a.User);               

            modelBuilder.Entity<ApplicationUser>()
               .HasOptional(s => s.Student)
               .WithRequired(ad => ad.User);

            modelBuilder.Entity<Assignment>().HasKey(t => t.Id);
            modelBuilder.Entity<Question>().HasKey(t => t.Id).Ignore(c=>c.SerialNo);           
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<QuestApplication.Entities.Assignment> Assignments { get; set; }

        public System.Data.Entity.DbSet<QuestApplication.Entities.Question> Questions { get; set; }
    }
}