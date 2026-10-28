# Railway Reservation System

## Technologies
- C#
- .NET 10
- ADO.NET
- SQL Server
- Visual Studio

## Features
1. Register passenger
2. View passengers
3. View trains
4. Check train seat availability
5. Book train tickets
6. Check booking status using PNR
7. SQL database transactions

## Database Setup
1. Open SQL Server Management Studio.
2. Open Database/RailwayReservationDB.sql.
3. Execute the SQL script.
4. Verify the RailwayReservationDB database.

## Connection String
The default connection string is:

Server=.\SQLEXPRESS;
Database=RailwayReservationDB;
Trusted_Connection=True;
TrustServerCertificate=True;

Update the server name in Program.cs if necessary.

## Run in Visual Studio
1. Open RailwayReservation.csproj.
2. Restore NuGet packages.
3. Verify the database connection string.
4. Execute the project using Ctrl + F5.

## Transaction
The ticket-booking repository uses a SQL transaction.
Seat availability is updated and the booking is inserted
within the same transaction.

If an operation fails, the transaction is rolled back.
If all operations succeed, the transaction is committed.