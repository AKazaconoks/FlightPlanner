using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Flight> Flights { get; set; }
    DbSet<Airport> Airports { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}