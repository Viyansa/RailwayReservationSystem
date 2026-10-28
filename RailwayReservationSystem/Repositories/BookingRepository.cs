using Microsoft.Data.SqlClient;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Repositories;

public class BookingRepository
{
    private readonly string _connectionString;

    public BookingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public string BookTicket(Booking booking)
    {
        using SqlConnection connection =
            new SqlConnection(_connectionString);

        connection.Open();

        using SqlTransaction transaction =
            connection.BeginTransaction();

        try
        {
            const string passengerQuery = @"
                SELECT COUNT(1)
                FROM Passengers
                WHERE PassengerId = @PassengerId;";

            using (SqlCommand passengerCommand =
                new SqlCommand(passengerQuery, connection, transaction))
            {
                passengerCommand.Parameters.AddWithValue(
                    "@PassengerId", booking.PassengerId);

                if (Convert.ToInt32(passengerCommand.ExecuteScalar()) == 0)
                {
                    throw new InvalidOperationException(
                        "Passenger does not exist.");
                }
            }

            const string seatQuery = @"
                UPDATE Trains
                SET AvailableSeats = AvailableSeats - @Seats
                WHERE TrainId = @TrainId
                  AND AvailableSeats >= @Seats;";

            using (SqlCommand seatCommand =
                new SqlCommand(seatQuery, connection, transaction))
            {
                seatCommand.Parameters.AddWithValue(
                    "@Seats", booking.NumberOfSeats);

                seatCommand.Parameters.AddWithValue(
                    "@TrainId", booking.TrainId);

                int rowsUpdated = seatCommand.ExecuteNonQuery();

                if (rowsUpdated == 0)
                {
                    throw new InvalidOperationException(
                        "Train not found or insufficient seats available.");
                }
            }

            const string fareQuery = @"
                SELECT Fare
                FROM Trains
                WHERE TrainId = @TrainId;";

            decimal farePerSeat;

            using (SqlCommand fareCommand =
                new SqlCommand(fareQuery, connection, transaction))
            {
                fareCommand.Parameters.AddWithValue(
                    "@TrainId", booking.TrainId);

                farePerSeat = Convert.ToDecimal(
                    fareCommand.ExecuteScalar());
            }

            booking.TotalFare = farePerSeat * booking.NumberOfSeats;
            booking.PNR = Guid.NewGuid().ToString("N")[..10].ToUpper();
            booking.Status = "Confirmed";

            const string bookingQuery = @"
                INSERT INTO Bookings
                    (PNR, PassengerId, TrainId, JourneyDate,
                     NumberOfSeats, TotalFare, Status)
                VALUES
                    (@PNR, @PassengerId, @TrainId, @JourneyDate,
                     @NumberOfSeats, @TotalFare, @Status);";

            using (SqlCommand bookingCommand =
                new SqlCommand(bookingQuery, connection, transaction))
            {
                bookingCommand.Parameters.AddWithValue("@PNR", booking.PNR);
                bookingCommand.Parameters.AddWithValue(
                    "@PassengerId", booking.PassengerId);
                bookingCommand.Parameters.AddWithValue(
                    "@TrainId", booking.TrainId);
                bookingCommand.Parameters.AddWithValue(
                    "@JourneyDate", booking.JourneyDate.Date);
                bookingCommand.Parameters.AddWithValue(
                    "@NumberOfSeats", booking.NumberOfSeats);
                bookingCommand.Parameters.AddWithValue(
                    "@TotalFare", booking.TotalFare);
                bookingCommand.Parameters.AddWithValue(
                    "@Status", booking.Status);

                bookingCommand.ExecuteNonQuery();
            }

            transaction.Commit();

            return booking.PNR;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public BookingDetailView? GetBookingByPNR(string pnr)
    {
        const string query = @"
            SELECT
                b.PNR,
                p.Name AS PassengerName,
                t.TrainName,
                t.Source,
                t.Destination,
                b.JourneyDate,
                b.NumberOfSeats,
                b.TotalFare,
                b.Status
            FROM Bookings b
            INNER JOIN Passengers p
                ON b.PassengerId = p.PassengerId
            INNER JOIN Trains t
                ON b.TrainId = t.TrainId
            WHERE b.PNR = @PNR;";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PNR", pnr);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new BookingDetailView
        {
            PNR = Convert.ToString(reader["PNR"]) ?? "",
            PassengerName =
                Convert.ToString(reader["PassengerName"]) ?? "",
            TrainName = Convert.ToString(reader["TrainName"]) ?? "",
            Source = Convert.ToString(reader["Source"]) ?? "",
            Destination =
                Convert.ToString(reader["Destination"]) ?? "",
            JourneyDate = Convert.ToDateTime(reader["JourneyDate"]),
            NumberOfSeats = Convert.ToInt32(reader["NumberOfSeats"]),
            TotalFare = Convert.ToDecimal(reader["TotalFare"]),
            Status = Convert.ToString(reader["Status"]) ?? ""
        };
    }
}