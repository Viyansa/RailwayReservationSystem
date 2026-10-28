namespace RailwayReservationSystem.Models;

public class Passenger
{
    public int PassengerId { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Gender { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
}
