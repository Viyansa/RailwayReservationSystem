namespace RailwayReservationSystem.Models;

public class BookingDetailView
{
    public string PNR { get; set; } = "";
    public string PassengerName { get; set; } = "";
    public string TrainName { get; set; } = "";
    public string Source { get; set; } = "";
    public string Destination { get; set; } = "";
    public DateTime JourneyDate { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal TotalFare { get; set; }
    public string Status { get; set; } = "";
}