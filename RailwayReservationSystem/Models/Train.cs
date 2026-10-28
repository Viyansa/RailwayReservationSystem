namespace RailwayReservationSystem.Models;

public class Train
{
    public int TrainId { get; set; }
    public string TrainName { get; set; } = "";
    public string Source { get; set; } = "";
    public string Destination { get; set; } = "";
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Fare { get; set; }
}