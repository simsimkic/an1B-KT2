using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IRequestRepository
    {
        public List<Request> GetAll();
        public Request Save(DateTime newStartDate, DateTime newEndDate, RequestStatus status, AccommodationReservation reservation);
        public void SaveAllRequests();
        public int NextId();
        public void DeclineRequest(Request selectedRequest);
        public void AcceptRequest(Request selectedRequest);
    }
}
