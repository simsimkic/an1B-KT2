using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourReviewImageService
    {
        private readonly ITourRatingImageRepository _tourRatingImageRepository;

        public TourReviewImageService()
        {
            _tourRatingImageRepository = Injector.CreateInstance<ITourRatingImageRepository>();
        }

        public void SaveImage(string url, int reviewId)
        {
            _tourRatingImageRepository.Save(url, reviewId);
        }
    }
}
