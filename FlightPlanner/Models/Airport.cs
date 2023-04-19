using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models;

public class Airport
{
    [Key] [JsonIgnore] public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    [JsonPropertyName("airport")] public string AirportName { get; set; }
}