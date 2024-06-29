using APBDProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDProject.Context;

public class RevenueRecognitionContext : DbContext
{
    public RevenueRecognitionContext(DbContextOptions<RevenueRecognitionContext> options) : base(options) { }
    public DbSet<Client> Clients { get; set; }
    public DbSet<SoftwareSystem> SoftwareSystems { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User_Role> UserRoles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
    }
}