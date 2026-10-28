using Microsoft.Data.SqlClient;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Repositories;

public class TrainRepository
{
    private readonly string _connectionString;

    public TrainRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Train> GetAllTrains()
    {
        List<Train> trains = new List<Train>();

        const string query = @"
            SELECT TrainId, TrainName, Source, Destination,
                   TotalSeats, AvailableSeats, Fare
            FROM Trains
            ORDER BY TrainId;";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Train train = new Train
            {
                TrainId = Convert.ToInt32(reader["TrainId"]),
                TrainName = Convert.ToString(reader["TrainName"]) ?? "",
                Source = Convert.ToString(reader["Source"]) ?? "",
                Destination = Convert.ToString(reader["Destination"]) ?? "",
                TotalSeats = Convert.ToInt32(reader["TotalSeats"]),
                AvailableSeats = Convert.ToInt32(reader["AvailableSeats"]),
                Fare = Convert.ToDecimal(reader["Fare"])
            };

            trains.Add(train);
        }

        return trains;
    }

    public Train? GetTrainById(int trainId)
    {
        const string query = @"
            SELECT TrainId, TrainName, Source, Destination,
                   TotalSeats, AvailableSeats, Fare
            FROM Trains
            WHERE TrainId = @TrainId;";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@TrainId", trainId);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Train
        {
            TrainId = Convert.ToInt32(reader["TrainId"]),
            TrainName = Convert.ToString(reader["TrainName"]) ?? "",
            Source = Convert.ToString(reader["Source"]) ?? "",
            Destination = Convert.ToString(reader["Destination"]) ?? "",
            TotalSeats = Convert.ToInt32(reader["TotalSeats"]),
            AvailableSeats = Convert.ToInt32(reader["AvailableSeats"]),
            Fare = Convert.ToDecimal(reader["Fare"])
        };
    }
}