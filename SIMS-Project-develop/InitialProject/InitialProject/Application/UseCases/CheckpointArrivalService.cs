using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class CheckpointArrivalService
    {
        private readonly ICheckpointArrivalRepository _checkpointArrivalRepository;
        private readonly TourReservationService _tourReservationService;
        private readonly CheckpointService _checkpointService;
        public CheckpointArrivalService()
        {
            _checkpointArrivalRepository = Injector.CreateInstance<ICheckpointArrivalRepository>();
            _tourReservationService = new TourReservationService();
            _checkpointService = new CheckpointService();
        }

        public IEnumerable<CheckpointArrival> GetAll()
        {
            List<CheckpointArrival> arrivals = new();
            foreach (var arrival in _checkpointArrivalRepository.GetAll())
            {
                arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
                arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
                arrivals.Add(arrival);
            }
            return arrivals;
        }

        public IEnumerable<CheckpointArrival> GetAllByTour(Tour tour)
        {
            List<CheckpointArrival> arrivals = new();
            foreach (var arrival in _checkpointArrivalRepository.GetAll())
            {
                arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
                arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
                if (arrival.Checkpoint.TourId == tour.Id)
                {
                    arrivals.Add(arrival);
                }
            }
            return arrivals;
        }

        public CheckpointArrival GetById(int id)
        {
            var arrival = _checkpointArrivalRepository.GetById(id);
            arrival.Reservation = _tourReservationService.GetById(arrival.ReservationId);
            arrival.Checkpoint = _checkpointService.GetById(arrival.CheckpointId);
            return arrival;
        }
    }
}
