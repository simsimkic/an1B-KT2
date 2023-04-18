using InitialProject.Domain.Models;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IRatingRepository
    {
        public List<Rating> GetAll();
        public Rating Save(string cleanliness, string followingTheRules, string comment, int theOneWhoIsRatedId, int raterId, int reservationId);
        public int NextId();
        public Rating GetByReservationId(int id);
    }
}
