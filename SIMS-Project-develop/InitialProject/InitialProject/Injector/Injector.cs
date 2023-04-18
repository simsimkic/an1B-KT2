using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;

namespace InitialProject
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(IRatingRepository), new RatingRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IAccommodationRatingRepository), new AccommodationRatingRepository() },
            { typeof(ICheckpointArrivalRepository), new CheckpointArrivalRepository() },
            { typeof(ITourReviewRepository), new TourReviewRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(ICheckpointRepository), new CheckpointRepository() },
            { typeof(IRequestRepository), new RequestRepository() },
            { typeof(IAccommodationRatingImageRepository), new AccommodationRatingImageRepository() },
            { typeof(ITourRatingImageRepository), new TourRatingImageRepository() },
            { typeof(ITourNotificationRepository), new TourNotificationRepository() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
