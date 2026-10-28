using RailwayReservationSystem.Models;
using RailwayReservationSystem.Repositories;

namespace RailwayReservationSystem.Services;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly PassengerRepository _passengerRepository;
    private readonly TrainRepository _trainRepository;

    public BookingService(
        BookingRepository bookingRepository,
        PassengerRepository passengerRepository,
        TrainRepository trainRepository)
    {
        _bookingRepository = bookingRepository;
        _passengerRepository = passengerRepository;
        _trainRepository = trainRepository;
    }

    public string BookTicket(Booking booking)
    {
        if (!_passengerRepository.PassengerExists(booking.PassengerId))
        {
            throw new ArgumentException("Passenger ID does not exist.");
        }

        Train? train = _trainRepository.GetTrainById(booking.TrainId);

        if (train == null)
        {
            throw new ArgumentException("Train ID does not exist.");
        }

        if (booking.NumberOfSeats <= 0)
        {
            throw new ArgumentException(
                "Number of seats must be greater than zero.");
        }

        if (booking.JourneyDate.Date < DateTime.Today)
        {
            throw new ArgumentException(
                "Journey date cannot be in the past.");
        }

        if (booking.NumberOfSeats > train.AvailableSeats)
        {
            throw new ArgumentException(
                "Not enough seats are available.");
        }

        return _bookingRepository.BookTicket(booking);
    }

    public BookingDetailView? GetBookingByPNR(string pnr)
    {
        if (string.IsNullOrWhiteSpace(pnr))
        {
            throw new ArgumentException("PNR is required.");
        }

        return _bookingRepository.GetBookingByPNR(pnr.Trim());
    }
}