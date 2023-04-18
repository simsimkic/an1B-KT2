using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class SetOwnerRoleService
    {
        private readonly IAccommodationRatingRepository _accommodationRatingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public SetOwnerRoleService() 
        {
            _accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
            _userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        }

        public int CalculateNumberOfRatings(int ownerId)
        {
            int numberOfRatings = _accommodationRatingRepository.CalculateNumberOfRatings(ownerId);
            return numberOfRatings;
        }

        internal double CalculateTotalRating(int ownerId)
        {
            double totalRating = _accommodationRatingRepository.CalculateTotalRating(ownerId);
            return totalRating;
        }

        public void SetOwnerRole(int ownerId)
        {
            int numberOfRatings = CalculateNumberOfRatings(ownerId);
            double totalRating = CalculateTotalRating(ownerId);

            _userRepository.SetOwnerRole(ownerId, numberOfRatings, totalRating);
            _accommodationRepository.SetSuperOwnerMark(ownerId, numberOfRatings, totalRating);
        }
    }
}
