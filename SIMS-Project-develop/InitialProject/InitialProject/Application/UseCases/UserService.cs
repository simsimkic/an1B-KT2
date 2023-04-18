using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = Injector.CreateInstance<IUserRepository>();
        }

        internal User FindOwnerById(int ownerId)
        {
            User owner = _userRepository.GetById(ownerId);

            return owner;
        }
    }
}
