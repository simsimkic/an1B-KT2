using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class RatingService
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingService() 
        { 
            _ratingRepository = Injector.CreateInstance<IRatingRepository>();
        }

        public Rating FindRatingByReservationId(int reservationId)
        {
            Rating rating = _ratingRepository.GetByReservationId(reservationId);

            return rating;
        }
    }
}
