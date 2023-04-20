using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models;

public class SearchFlightRequest
{
    public string From { get; set; }
    public string To { get; set; }
    public string DepartureDate { get; set; }
}