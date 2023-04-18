using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class ManageRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;

        public ManageRequestService() 
        {
            _requestRepository = Injector.CreateInstance<IRequestRepository>();
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public void DeclineRequest(Request selectedRequest)
        {
            _requestRepository.DeclineRequest(selectedRequest);
        }

        internal void AcceptRequest(Request selectedRequest)
        {
            _requestRepository.AcceptRequest(selectedRequest);
            _accommodationReservationRepository.AcceptRequest(selectedRequest);
        }
    }
}
