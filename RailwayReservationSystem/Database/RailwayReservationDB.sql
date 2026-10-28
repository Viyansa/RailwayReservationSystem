IF DB_ID('RailwayReservationDB') IS NULL
BEGIN
    CREATE DATABASE RailwayReservationDB;
END;
GO

USE RailwayReservationDB;
GO

IF OBJECT_ID('dbo.Bookings', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bookings
    (
        BookingId INT IDENTITY(1,1) PRIMARY KEY,
        PNR VARCHAR(10) NOT NULL UNIQUE,
        PassengerId INT NOT NULL,
        TrainId INT NOT NULL,
        JourneyDate DATE NOT NULL,
        NumberOfSeats INT NOT NULL,
        TotalFare DECIMAL(10,2) NOT NULL,
        Status VARCHAR(20) NOT NULL DEFAULT 'Confirmed',

        CONSTRAINT FK_Bookings_Passengers
            FOREIGN KEY (PassengerId)
            REFERENCES dbo.Passengers(PassengerId),

        CONSTRAINT FK_Bookings_Trains
            FOREIGN KEY (TrainId)
            REFERENCES dbo.Trains(TrainId),

        CONSTRAINT CK_Bookings_Seats
            CHECK (NumberOfSeats > 0),

        CONSTRAINT CK_Bookings_Fare
            CHECK (TotalFare >= 0)
    );
END;
GO