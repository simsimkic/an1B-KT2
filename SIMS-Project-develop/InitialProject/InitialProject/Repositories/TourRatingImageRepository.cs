using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class TourRatingImageRepository : ITourRatingImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRatingImages.csv";
        private readonly Serializer<TourRatingImage> _serializer;

        private List<TourRatingImage> _images;

        public TourRatingImageRepository()
        {
            _serializer = new Serializer<TourRatingImage>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<TourRatingImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourRatingImage> LoadAllImages()
        {
            return _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);

            if (_images.Count < 1)
            {
                return 1;
            }

            return _images.Max(c => c.Id) + 1;
        }

        public TourRatingImage Save(string url, int reviewId)
        {
            int id = NextId();
            
            TourRatingImage image = new TourRatingImage(url, reviewId);
            image.Id = id;
            _images.Add(image);
            SaveAllImages();
            return image;
        }

        public void SaveAllImages()
        {
            _serializer.ToCSV(FilePath, _images);
        }
    }
}
