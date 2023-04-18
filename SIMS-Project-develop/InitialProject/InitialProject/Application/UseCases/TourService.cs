using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;

namespace InitialProject.Application.UseCases
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly ITourReservationRepository _tourReservationRepository;

        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
            _locationRepository = Injector.CreateInstance<ILocationRepository>();
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
        }

        public IEnumerable<Tour> GetFutureTours()
        {
            var futureTours = _tourRepository.GetAll().Where(t => t.StartTime.Subtract(DateTime.Now).TotalHours > 0);
            LoadLocations(futureTours);
            return futureTours;
        }

        public IEnumerable<Tour> GetPastTours()
        {
            var pastTours = _tourRepository.GetAll().Where(t => t.StartTime.Subtract(DateTime.Now).TotalHours < 0);
            LoadLocations(pastTours);
            return pastTours;
        }

        public void CancelTour(Tour tour)
        {
            tour.Status = TourStatus.CANCELED;
            _tourRepository.Update(tour);
        }

        public ObservableCollection<TourCheckpoint> GetReservedTours(User user)
        {
            var reservedTours = new ObservableCollection<TourCheckpoint>();
            var checkpoints = _checkpointRepository.GetAll();
            var usersReservedTours = _tourReservationRepository.GetAll().Where(t => t.UserId == user.Id);

            foreach (var reservation in usersReservedTours)
            {
                foreach(var checkpoint in checkpoints)
                {
                    if(checkpoint.TourId == reservation.TourId && checkpoint.Active == true && reservation.UserId == user.Id)
                    {
                        Tour tour = _tourRepository.GetById(reservation.TourId); //Podrazumeva se da ce se prikazati samo aktivne i finished jer su samo u tim slucajevima checkpointi active!
                        var ReservedTour = new TourCheckpoint(tour.Id, tour.Name, tour.Status, checkpoint.Id, checkpoint.Name, user.Id);
                        reservedTours.Add(ReservedTour);
                    }
                }
            }
            return reservedTours;
        }

        private void LoadLocations(IEnumerable<Tour> tours)
        {
            foreach (var tour in tours)
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
            }
        }

        public int GetGuideId(int tourId)
        {
            var tours = _tourRepository.GetAll();
            foreach (var tour in tours)
            {
                if(tour.Id == tourId)
                {
                    return tour.GuideId;
                }
            }
            return -1;
        }

        public Tour GetById(int id)
        {
            var tour = _tourRepository.GetAll().FirstOrDefault(t => t.Id == id);
            tour.Location = _locationRepository.GetById(tour.LocationId);
            return tour;
        }

        public IEnumerable<DateTime> GetAllStartTimesForTour(Tour tour)
        {
            return _tourRepository.GetAll().Where(t => t.Name == tour.Name).Select(t => t.StartTime);
        }
    }
}
