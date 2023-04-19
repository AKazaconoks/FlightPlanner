using FlightPlanner.Interfaces;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services;

public class DbServices
{
    private readonly IApplicationDbContext _context;

    public DbServices(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Flight>> GetFlightsAsync()
    {
        return await _context.Flights.Include(f => f.From).Include(f => f.To).ToListAsync();
    }

    public async Task<Flight> GetFlightByIdAsync(int id)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);
        return flight;
    }

    public async Task<Airport> FindOrCreateAirportAsync(Airport airport)
    {
        var existingAirport = await _context.Airports.FirstOrDefaultAsync(a => a.Country == airport.Country &&
                                                                               a.City == airport.City &&
                                                                               a.AirportName == airport.AirportName);
        if (existingAirport != null)
        {
            return existingAirport;
        }

        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return airport;
    }

    public async Task<Flight> FindOrCreateFlightAsync(Flight flight)
    {
        var existingFlight = await _context.Flights.FirstOrDefaultAsync(f => f.From.Id == flight.From.Id &&
                                                                             f.To.Id == flight.To.Id &&
                                                                             f.DepartureTime == flight.DepartureTime && 
                                                                             f.ArrivalTime == flight.ArrivalTime);
        if (existingFlight != null)
        {
            return existingFlight;
        }
        
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight;
    }
    
    public async Task DeleteFlightAsync(Flight flight)
    {
        if (flight == null)
        {
            return;
        }
        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Airport>> GetAirportsAsync()
    {
        return await _context.Airports.ToListAsync();
    }
}