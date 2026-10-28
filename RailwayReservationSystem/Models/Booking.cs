namespace RailwayReservationSystem.Models;

public class Booking
{
    public int BookingId { get; set; }
    public string PNR { get; set; } = "";
    public int PassengerId { get; set; }
    public int TrainId { get; set; }
    public DateTime JourneyDate { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal TotalFare { get; set; }
    public string Status { get; set; } = "Confirmed";
}
