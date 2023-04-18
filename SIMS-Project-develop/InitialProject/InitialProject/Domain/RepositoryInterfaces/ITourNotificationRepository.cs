using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourNotificationRepository
    {
        public List<TourNotification> GetAll();
        public int NextId();
        public TourNotification Save(TourNotification tourNotification);
    }
}
