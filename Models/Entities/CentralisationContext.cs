using CentralisationdeDonnee.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CentralisationV0.Models.Entities
{
    public class CentralisationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Data> Datas { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<CoordinateSystem> CoordinateSystems { get; set; }
        public DbSet<UseConstraint> UseConstraints { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Collaboration> Collaborations{ get; set; }

        
        public DbSet<Client> Clients { get; set; }

        public DbSet<ContactClient> ContactClients { get; set; }

        public CentralisationContext() : base(nameOrConnectionString: "PostgresConnection")
        {
            this.Database.CommandTimeout = 120;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            // Configure the primary key for the Theme entity
            modelBuilder.Entity<Theme>()
                .HasKey(t => t.IdTheme);

            // Configure the one-to-many relationship between Theme and Data
            modelBuilder.Entity<Data>()
                .HasRequired(d => d.Theme)
                .WithMany(t => t.Datas)
                .HasForeignKey(d => d.ThemeId);

            // Configure the one-to-many relationship between Theme and Industry
            modelBuilder.Entity<Industry>()
                .HasRequired(i => i.Theme)
                .WithMany(t => t.Industries)
                .HasForeignKey(i => i.ThemeId);

            base.OnModelCreating(modelBuilder);
        }

        public static CentralisationContext Create()
        {
            return new CentralisationContext();
        }
    }
}
