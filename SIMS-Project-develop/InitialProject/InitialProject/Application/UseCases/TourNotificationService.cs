using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class TourNotificationService
    {
        private readonly ITourNotificationRepository _tourNotificationRepository;
        public TourNotificationService()
        {
            _tourNotificationRepository = Injector.CreateInstance<ITourNotificationRepository>();
        }

        //TO-DO implementirati potrebne metode.
    }
}
