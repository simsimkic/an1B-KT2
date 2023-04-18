using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRatingImageRepository
    {
        public List<TourRatingImage> GetAll();
        public TourRatingImage Save(string url, int reviewId);
        public List<TourRatingImage> LoadAllImages();
        public void SaveAllImages();
        public int NextId();
    }
}
