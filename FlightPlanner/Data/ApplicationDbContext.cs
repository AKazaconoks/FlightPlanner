using FlightPlanner.Interfaces;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airport> Airports { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>()
            .HasOne(f => f.From)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Flight>()
            .HasOne(f => f.To)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}