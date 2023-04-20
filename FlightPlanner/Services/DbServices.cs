using FlightPlanner.Interfaces;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FlightPlanner.Services;

public class DbServices
{
    private readonly IApplicationDbContext _context;
    private static SemaphoreSlim _flightSemaphore = new SemaphoreSlim(1, 1);
    private static SemaphoreSlim _airportSemaphore = new SemaphoreSlim(1, 1);
    
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
        var flight = await _context.Flights.Include(f => f.From).Include(f => f.To)
            .FirstOrDefaultAsync(f => f.Id == id);
        return flight;
    }

    public async Task<Airport> FindOrCreateAirportAsync(Airport airport)
    {
        await _airportSemaphore.WaitAsync();
        try
        {
            var existingAirport = await _context.Airports.FirstOrDefaultAsync(a =>
                a.Country.ToLower() == airport.Country.ToLower() &&
                a.AirportName.ToLower() == airport.AirportName.ToLower() &&
                a.City.ToLower() == airport.City.ToLower());

            if (existingAirport != null)
            {
                return existingAirport;
            }

            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
            return airport;
        }
        finally
        {
            _airportSemaphore.Release();
        }
    }

    public async Task<bool> FindOrCreateFlightAsync(Flight flight)
    {
        await _flightSemaphore.WaitAsync();
        try
        {
            var existingFlight = await _context.Flights.FirstOrDefaultAsync(f =>
                f.From.AirportName == flight.From.AirportName &&
                f.To.AirportName == flight.To.AirportName &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime);
            if (existingFlight != null)
            {
                return true;
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return false;
        }
        finally
        {
            _flightSemaphore.Release();
        }
    }

    public static bool CheckIfFlightIsValid(Flight flight)
    {
        var departureTime = DateTime.Parse(flight.DepartureTime);
        var arrivalTime = DateTime.Parse(flight.ArrivalTime);

        return !flight.From.City.IsNullOrEmpty() && !flight.From.AirportName.IsNullOrEmpty() &&
               !flight.From.Country.IsNullOrEmpty() &&
               !flight.To.City.IsNullOrEmpty() && !flight.To.AirportName.IsNullOrEmpty() &&
               !flight.To.Country.IsNullOrEmpty() &&
               !flight.DepartureTime.IsNullOrEmpty() && !flight.ArrivalTime.IsNullOrEmpty() &&
               !flight.Carrier.IsNullOrEmpty() &&
               !string.Equals(flight.From.AirportName, flight.To.AirportName,
                   StringComparison.CurrentCultureIgnoreCase) &&
               !string.Equals(flight.From.City, flight.To.City, StringComparison.CurrentCultureIgnoreCase) &&
               departureTime < arrivalTime;
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

    public async Task DeleteAllFlightsAsync()
    {
        _context.Flights.RemoveRange(_context.Flights);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAirportsAsync()
    {
        _context.Airports.RemoveRange(_context.Airports);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Airport>> GetAirportsAsync(string search)
    {
        return await _context.Airports
            .Where(a =>
                a.AirportName.ToLower().Contains(search.ToLower().Trim()) ||
                a.City.ToLower().Contains(search.ToLower().Trim()) ||
                a.Country.ToLower().Contains(search.ToLower().Trim()))
            .ToListAsync();
    }

    public async Task<List<Flight>> GetFlightsBySearchAsync(SearchFlightRequest request)
    {
        var flights = await _context.Flights
            .Include(f => f.From)
            .Include(f => f.To)
            .Where(f =>
                f.From.AirportName == request.From &&
                f.To.AirportName == request.To &&
                f.DepartureTime.Substring(0, 10) == request.DepartureDate)
            .ToListAsync();

        return flights;
    }
}