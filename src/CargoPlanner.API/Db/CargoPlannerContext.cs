using System.Threading.Tasks;
using CargoPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CargoPlanner.API.Db
{
    public class CargoPlannerContext : DbContext
    {
        public CargoPlannerContext(DbContextOptions<CargoPlannerContext> options) : base(options)
        {
        }

        public DbSet<Instance> Instances { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var guidToStringConverter = new GuidToStringConverter();

            // Instances
            modelBuilder.Entity<Instance>()
                        .ToContainer("Instances")
                        .HasPartitionKey(e => e.UserId);
            modelBuilder.Entity<Instance>(instance =>
            {
                instance.Property(e => e.Id).HasValueGenerator<GuidValueGenerator>();
                instance.Property(e => e.UserId).HasConversion(guidToStringConverter);
            });
            modelBuilder.Entity<Instance>().OwnsOne(e => e.Truck, truck =>
            {
                truck.OwnsOne(e => e.FrontAxle);
                truck.OwnsOne(e => e.RearAxle);
                truck.Ignore(e => e.Items);
            });
            modelBuilder.Entity<Instance>().OwnsMany(e => e.Items, item =>
            {
                item.Ignore(e => e.Position);
            });

            // Users
            modelBuilder.Entity<User>()
                        .ToContainer("Users")
                        .HasPartitionKey(e => e.Username);
            modelBuilder.Entity<User>(instance =>
            {
                instance.Property(e => e.Id).HasValueGenerator<GuidValueGenerator>();
            });

            // Results
            modelBuilder.Entity<Result>()
                        .ToContainer("Results")
                        .HasPartitionKey(e => e.UserId);
            modelBuilder.Entity<Result>(result =>
            {
                result.Property(e => e.Id).HasValueGenerator<GuidValueGenerator>();
                result.Property(e => e.UserId).HasConversion(guidToStringConverter);
                result.Property(e => e.InstanceId).HasConversion(guidToStringConverter);
            });
            modelBuilder.Entity<Result>().OwnsMany(e => e.Trucks, truck =>
            {
                truck.OwnsOne(e => e.FrontAxle);
                truck.OwnsOne(e => e.RearAxle);
                truck.OwnsMany(e => e.Items);
            });

            base.OnModelCreating(modelBuilder);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}