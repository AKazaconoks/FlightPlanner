using System.ComponentModel.DataAnnotations.Schema;

namespace FlightPlanner.Models;

public class FlightModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Airport From { get; set; }
    public Airport To { get; set; }
    public string Carrier { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}