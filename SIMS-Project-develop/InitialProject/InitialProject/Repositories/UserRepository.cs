using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetById(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void SetOwnerRole(int ownerId, int numberOfRatings, double totalRating)
        {
            _users = _serializer.FromCSV(FilePath);

            if (numberOfRatings > 50 && totalRating >= 4.5)
            {
                _users.FirstOrDefault(u => u.Id == ownerId).Role = UserRole.SUPER_OWNER;
            }
            else
            {
                _users.FirstOrDefault(u => u.Id == ownerId).Role = UserRole.OWNER;
            }

            _serializer.ToCSV(FilePath, _users);
        }
    }
}
