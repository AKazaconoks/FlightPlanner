namespace FlightPlanner.Models;

public class FlightSearchRequest
{
    public List<Flight> Items { get; set; }
    public int Page { get; set; }
    public int TotalItems { get; set; }
}