using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ImprovementOpportunityApp.Models.Data;
using System.Collections.Generic;

namespace ImprovementOpportunityApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [Required]
        public int DepartmentId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastUpdated { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        public ICollection<Suggestion> Suggestions { get; set; }
        public ICollection<ForumMessage> ForumMessages { get; set; }
        public ICollection<UserVote> UserVotes { get; set; }

        public ApplicationUser()
        {
            IsActive = true;
            DateAdded = DateTime.Now;
            LastUpdated = DateTime.Now;
            Suggestions = new HashSet<Suggestion>();
            ForumMessages = new HashSet<ForumMessage>();
            UserVotes = new HashSet<UserVote>();
        }

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
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Department> Departments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumMessage> ForumMessages { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }

    }
}