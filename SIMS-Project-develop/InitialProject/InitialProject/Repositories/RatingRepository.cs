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
    public class RatingRepository : IRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/ratings.csv";

        private readonly Serializer<Rating> _serializer;

        private List<Rating> _ratings;

        public RatingRepository()
        {
            _serializer = new Serializer<Rating>();
            _ratings = _serializer.FromCSV(FilePath);
        }

        public List<Rating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Rating Save(string cleanliness, string followingTheRules, string comment, int theOneWhoIsRatedId, int raterId, int reservationId)
        {
            int id = NextId();

            Rating rating = new Rating(id, Convert.ToInt32(cleanliness), Convert.ToInt32(followingTheRules), comment, theOneWhoIsRatedId, raterId, reservationId);

            _ratings = _serializer.FromCSV(FilePath);
            _ratings.Add(rating);
            _serializer.ToCSV(FilePath, _ratings);
            return rating;
        }

        public int NextId()
        {
            _ratings = _serializer.FromCSV(FilePath);

            if (_ratings.Count < 1)
            {
                return 1;
            }

            return _ratings.Max(t => t.Id) + 1;
        }

        public Rating GetByReservationId(int reservationId)
        {
            _ratings = _serializer.FromCSV(FilePath);

            Rating rating = _ratings.Find(r => r.ReservationId == reservationId);

            return rating;
        }
    }
}
