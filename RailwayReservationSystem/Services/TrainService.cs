using RailwayReservationSystem.Models;
using RailwayReservationSystem.Repositories;

namespace RailwayReservationSystem.Services;

public class TrainService
{
    private readonly TrainRepository _repository;

    public TrainService(TrainRepository repository)
    {
        _repository = repository;
    }

    public List<Train> GetAllTrains()
    {
        return _repository.GetAllTrains();
    }

    public Train? GetTrainById(int trainId)
    {
        if (trainId <= 0)
        {
            throw new ArgumentException("Enter a valid train ID.");
        }

        return _repository.GetTrainById(trainId);
    }
}