using Microsoft.Data.SqlClient;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Repositories;

public class PassengerRepository
{
    private readonly string _connectionString;

    public PassengerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddPassenger(Passenger passenger)
    {
        const string query = @"
            INSERT INTO Passengers (Name, Age, Gender, Phone, Email)
            OUTPUT INSERTED.PassengerId
            VALUES (@Name, @Age, @Gender, @Phone, @Email);";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", passenger.Name);
        command.Parameters.AddWithValue("@Age", passenger.Age);
        command.Parameters.AddWithValue("@Gender", passenger.Gender);
        command.Parameters.AddWithValue("@Phone", passenger.Phone);
        command.Parameters.AddWithValue("@Email", passenger.Email);

        connection.Open();

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public List<Passenger> GetAllPassengers()
    {
        List<Passenger> passengers = new List<Passenger>();

        const string query = @"
            SELECT PassengerId, Name, Age, Gender, Phone, Email
            FROM Passengers
            ORDER BY PassengerId;";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Passenger passenger = new Passenger
            {
                PassengerId = Convert.ToInt32(reader["PassengerId"]),
                Name = Convert.ToString(reader["Name"]) ?? "",
                Age = Convert.ToInt32(reader["Age"]),
                Gender = Convert.ToString(reader["Gender"]) ?? "",
                Phone = Convert.ToString(reader["Phone"]) ?? "",
                Email = Convert.ToString(reader["Email"]) ?? ""
            };

            passengers.Add(passenger);
        }

        return passengers;
    }

    public bool PassengerExists(int passengerId)
    {
        const string query = @"
            SELECT COUNT(1)
            FROM Passengers
            WHERE PassengerId = @PassengerId;";

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PassengerId", passengerId);

        connection.Open();

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}