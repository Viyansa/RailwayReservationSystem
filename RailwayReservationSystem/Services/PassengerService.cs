using RailwayReservationSystem.Models;
using RailwayReservationSystem.Repositories;

namespace RailwayReservationSystem.Services;

public class PassengerService
{
    private readonly PassengerRepository _repository;

    public PassengerService(PassengerRepository repository)
    {
        _repository = repository;
    }

    public int RegisterPassenger(Passenger passenger)
    {
        if (string.IsNullOrWhiteSpace(passenger.Name))
        {
            throw new ArgumentException("Passenger name is required.");
        }

        if (passenger.Age < 1 || passenger.Age > 120)
        {
            throw new ArgumentException("Enter a valid age.");
        }

        if (string.IsNullOrWhiteSpace(passenger.Gender))
        {
            throw new ArgumentException("Gender is required.");
        }

        if (string.IsNullOrWhiteSpace(passenger.Phone))
        {
            throw new ArgumentException("Phone number is required.");
        }

        if (string.IsNullOrWhiteSpace(passenger.Email))
        {
            throw new ArgumentException("Email is required.");
        }

        return _repository.AddPassenger(passenger);
    }

    public List<Passenger> GetAllPassengers()
    {
        return _repository.GetAllPassengers();
    }

    public bool PassengerExists(int passengerId)
    {
        return _repository.PassengerExists(passengerId);
    }
}