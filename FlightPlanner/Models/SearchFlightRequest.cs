using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models;

public class SearchFlightRequest
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string DepartureDate { get; set; }
}