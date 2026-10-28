//Console.WriteLine("Hello, World!");
using Microsoft.Data.SqlClient;
using RailwayReservationSystem.Models;
using RailwayReservationSystem.Repositories;
using RailwayReservationSystem.Services;

namespace RailwayReservationSystem;

public class Program
{
    private static readonly string connectionString =
        @"Server=.\SQLEXPRESS;Database=RailwayReservationDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Main()
    {
        try
        {
            using SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();
            Console.WriteLine("Database connected successfully.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Database connection failed.");
            Console.WriteLine(ex.Message);
            Console.WriteLine(
                "Check your SQL Server instance and connection string.");
            return;
        }

        PassengerRepository passengerRepository =
            new PassengerRepository(connectionString);

        TrainRepository trainRepository =
            new TrainRepository(connectionString);

        BookingRepository bookingRepository =
            new BookingRepository(connectionString);

        PassengerService passengerService =
            new PassengerService(passengerRepository);

        TrainService trainService =
            new TrainService(trainRepository);

        BookingService bookingService =
            new BookingService(
                bookingRepository,
                passengerRepository,
                trainRepository);

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("===== RAILWAY RESERVATION SYSTEM =====");
            Console.WriteLine("1. Register Passenger");
            Console.WriteLine("2. View Passengers");
            Console.WriteLine("3. View Trains");
            Console.WriteLine("4. Check Train Availability");
            Console.WriteLine("5. Book Ticket");
            Console.WriteLine("6. Check Booking by PNR");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        RegisterPassenger(passengerService);
                        break;

                    case "2":
                        ViewPassengers(passengerService);
                        break;

                    case "3":
                        ViewTrains(trainService);
                        break;

                    case "4":
                        CheckAvailability(trainService);
                        break;

                    case "5":
                        BookTicket(bookingService);
                        break;

                    case "6":
                        CheckPNR(bookingService);
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("Thank you for using the system.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation failed: " + ex.Message);
            }
        }
    }

    private static void RegisterPassenger(PassengerService service)
    {
        Passenger passenger = new Passenger();

        Console.Write("Enter passenger name: ");
        passenger.Name = Console.ReadLine()?.Trim() ?? "";

        passenger.Age = ReadInt("Enter age: ");

        Console.Write("Enter gender: ");
        passenger.Gender = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Enter phone number: ");
        passenger.Phone = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Enter email: ");
        passenger.Email = Console.ReadLine()?.Trim() ?? "";

        int passengerId = service.RegisterPassenger(passenger);

        Console.WriteLine("Passenger registered successfully.");
        Console.WriteLine("Passenger ID: " + passengerId);
    }

    private static void ViewPassengers(PassengerService service)
    {
        List<Passenger> passengers = service.GetAllPassengers();

        if (passengers.Count == 0)
        {
            Console.WriteLine("No passengers found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("===== PASSENGER DETAILS =====");

        foreach (Passenger passenger in passengers)
        {
            Console.WriteLine(
                $"ID: {passenger.PassengerId} | " +
                $"Name: {passenger.Name} | " +
                $"Age: {passenger.Age} | " +
                $"Gender: {passenger.Gender} | " +
                $"Phone: {passenger.Phone} | " +
                $"Email: {passenger.Email}");
        }
    }

    private static void ViewTrains(TrainService service)
    {
        List<Train> trains = service.GetAllTrains();

        if (trains.Count == 0)
        {
            Console.WriteLine("No trains found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("===== TRAIN DETAILS =====");

        foreach (Train train in trains)
        {
            Console.WriteLine(
                $"ID: {train.TrainId} | " +
                $"Name: {train.TrainName} | " +
                $"{train.Source} to {train.Destination} | " +
                $"Available Seats: {train.AvailableSeats} | " +
                $"Fare: {train.Fare:C}");
        }
    }

    private static void CheckAvailability(TrainService service)
    {
        int trainId = ReadInt("Enter train ID: ");

        Train? train = service.GetTrainById(trainId);

        if (train == null)
        {
            Console.WriteLine("Train not found.");
            return;
        }

        Console.WriteLine("Train: " + train.TrainName);
        Console.WriteLine("Route: " + train.Source + " to " + train.Destination);
        Console.WriteLine("Available seats: " + train.AvailableSeats);
        Console.WriteLine("Fare per seat: " + train.Fare.ToString("C"));
    }

    private static void BookTicket(BookingService service)
    {
        Booking booking = new Booking();

        booking.PassengerId = ReadInt("Enter passenger ID: ");
        booking.TrainId = ReadInt("Enter train ID: ");

        Console.Write("Enter journey date (yyyy-MM-dd): ");

        if (!DateTime.TryParse(
            Console.ReadLine(),
            out DateTime journeyDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        booking.JourneyDate = journeyDate;
        booking.NumberOfSeats = ReadInt("Enter number of seats: ");

        string pnr = service.BookTicket(booking);

        Console.WriteLine("Ticket booked successfully.");
        Console.WriteLine("PNR: " + pnr);
        Console.WriteLine("Total fare: " + booking.TotalFare.ToString("C"));
    }

    private static void CheckPNR(BookingService service)
    {
        Console.Write("Enter PNR: ");
        string pnr = Console.ReadLine()?.Trim() ?? "";

        BookingDetailView? booking = service.GetBookingByPNR(pnr);

        if (booking == null)
        {
            Console.WriteLine("Booking not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("===== BOOKING DETAILS =====");
        Console.WriteLine("PNR: " + booking.PNR);
        Console.WriteLine("Passenger: " + booking.PassengerName);
        Console.WriteLine("Train: " + booking.TrainName);
        Console.WriteLine("Route: " + booking.Source + " to " + booking.Destination);
        Console.WriteLine("Journey date: " + booking.JourneyDate.ToString("yyyy-MM-dd"));
        Console.WriteLine("Number of seats: " + booking.NumberOfSeats);
        Console.WriteLine("Total fare: " + booking.TotalFare.ToString("C"));
        Console.WriteLine("Status: " + booking.Status);
    }

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid whole number.");
        }
    }
}
