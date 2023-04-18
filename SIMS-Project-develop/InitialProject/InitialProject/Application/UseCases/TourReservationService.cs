using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourReservationService
    {
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly TourService _tourService;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            _tourService = new TourService();
        }

        public TourReservation GetById(int id)
        {
            var tourReservation = _tourReservationRepository.GetById(id);
            tourReservation.Tour = _tourService.GetById(tourReservation.TourId);
            return tourReservation;
        }

        public TourReservation GetByTourIdAndUserId(int tourId, int userId)
        {
            var reservations = _tourReservationRepository.GetAll();
            foreach (var reservation in reservations)
            {
                if(reservation.TourId == tourId && reservation.UserId == userId)
                {
                    return reservation;
                }
            }
            return null;
        }
    }
}
