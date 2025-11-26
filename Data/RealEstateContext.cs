using Microsoft.EntityFrameworkCore;
using RealEstateWebApi.Models;
using Microsoft.Data.Sqlite;

namespace RealEstateWebApi.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Agent> agents { get; set; }
        public DbSet<Accountant> accountants { get; set; }
        public DbSet<Landlord> landlords { get; set; }
        public DbSet<Tenant> tenants { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MaintenanceRequests> MaintenanceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Agent>().ToTable("agents");
            modelBuilder.Entity<Accountant>().ToTable("accountants");
            modelBuilder.Entity<Landlord>().ToTable("landlords");
            modelBuilder.Entity<Tenant>().ToTable("tenants");
            modelBuilder.Entity<Property>().ToTable("properties");
            modelBuilder.Entity<Lease>().ToTable("Leases");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<MaintenanceRequests>().ToTable("MaintenanceRequests");


            modelBuilder.Entity<Agent>()
                .HasOne(a => a.User)
                .WithMany(u => u.Agents)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tenants)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);

 
            modelBuilder.Entity<Landlord>()
                .HasOne(l => l.User)
                .WithMany(u => u.Landlords)
                .HasForeignKey(l => l.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Accountant>()
                .HasOne(ac => ac.User)
                .WithMany(u => u.Accountants)
                .HasForeignKey(ac => ac.UserID)
                .OnDelete(DeleteBehavior.Cascade);

 
            modelBuilder.Entity<Lease>()
                .HasOne(l => l.Tenant)
                .WithMany(t => t.Leases)
                .HasForeignKey(l => l.TenantID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lease>()
                .HasOne(l => l.Agent)
                .WithMany(a => a.Leases)
                .HasForeignKey(l => l.AgentID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Property>()
                .HasOne(p => p.Landlord)
                .WithMany(l => l.Properties)
                .HasForeignKey(p => p.LandlordID)
                .OnDelete(DeleteBehavior.Cascade);

 
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Agent)
                .WithMany(a => a.Properties)
                .HasForeignKey(p => p.AgentID)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Lease)
                .WithMany(l => l.Payments)
                .HasForeignKey(p => p.LeaseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Accountant)
                .WithMany(ac => ac.Payments)
                .HasForeignKey(p => p.AccountantID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MaintenanceRequests>()
                .HasOne(m => m.Property)
                .WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(m => m.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaintenanceRequests>()
                .HasOne(m => m.Tenant)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(m => m.TenantID)
                .OnDelete(DeleteBehavior.Cascade);
         


        }
    }
}
