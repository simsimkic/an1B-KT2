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
    public class RequestRepository : IRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/requests.csv";

        private readonly Serializer<Request> _serializer;

        private List<Request> _requests;

        public RequestRepository()
        {
            _serializer = new Serializer<Request>();
            _requests = _serializer.FromCSV(FilePath);
        }

        public List<Request> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Request Save(DateTime newStartDate, DateTime newEndDate, RequestStatus status, AccommodationReservation reservation)
        {
            int id = NextId();

            Request request = new Request(id, newStartDate, newEndDate, status, reservation);
            _requests.Add(request);
            SaveAllRequests();
            return request;
        }

        public void SaveAllRequests()
        {
            _serializer.ToCSV(FilePath, _requests);
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(FilePath);

            if (_requests.Count < 1)
            {
                return 1;
            }

            return _requests.Max(c => c.Id) + 1;
        }

        public void DeclineRequest(Request selectedRequest)
        {
            _requests = _serializer.FromCSV(FilePath);

            _requests.Find(r => r.Id == selectedRequest.Id).Status = RequestStatus.DECLINED;
            _requests.Find(r => r.Id == selectedRequest.Id).Comment = selectedRequest.Comment;

            _serializer.ToCSV(FilePath, _requests);
        }

        public void AcceptRequest(Request selectedRequest)
        {
            _requests = _serializer.FromCSV(FilePath);

            _requests.Find(r => r.Id == selectedRequest.Id).Status = RequestStatus.ACCEPTED;

            _serializer.ToCSV(FilePath, _requests);
        }      
    }
}
