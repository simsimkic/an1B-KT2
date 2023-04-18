using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;

namespace InitialProject.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(DateTime startDate, DateTime endDate, int lenghtOfStay, Accommodation accommodation, int accommodationId, User guest, int guestId)
        {
            int id = NextId();

            AccommodationReservation accommodationReservation = new AccommodationReservation(id, startDate, endDate, lenghtOfStay, accommodation, guest);

            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }

            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public List<AccommodationReservation> GetAllByOwnerId(int ownerId, AccommodationRepository accommodationRepository, UserRepository userRepository)
        {
            List<AccommodationReservation> _reservations = new List<AccommodationReservation>();
            List<int> accommodationIdsForOwner = accommodationRepository.AccommodationIdsByOwnerId(ownerId);
            List<Accommodation> _accommodations = new List<Accommodation>();

            foreach (var reservation in _accommodationReservations)
            {
                if (accommodationIdsForOwner.Find(a => a == reservation.AccommodationId) != 0)
                {
                    reservation.Guest = userRepository.GetAll().Find(g => g.Id == reservation.GuestId);
                    _reservations.Add(reservation);
                }
            }

            return _reservations;
        }

        public List<AccommodationReservation> GetAllByGuestId(int guestId)
        {
            var guestReservations = new List<AccommodationReservation>();

            foreach (var reservation in GetAll())
            {
                if (reservation.GuestId == guestId)
                {
                    guestReservations.Add(reservation);
                }
            }

            return guestReservations;
        }

        public void Remove(AccommodationReservation reservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Remove(_accommodationReservations.Find(x => x.Id == reservation.Id));
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public AccommodationReservation GetById(int reservationId)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);

            return _accommodationReservations.FirstOrDefault(r => r.Id == reservationId);
        }

        public string IsAvailable(DateTime newStartDate, DateTime newEndDate, int reservationId, int accommodationId)
        {
            List<DateTime> allSingleDates = FindDatesBetween(newStartDate, newEndDate);

            foreach (var date in allSingleDates)
            {
                if (!IsSingleDateAvailable(date, reservationId, accommodationId))
                {
                    return "no";
                }
            }

            return "yes";
        }

        private bool IsSingleDateAvailable(DateTime date, int reservationId, int accommodationId)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            foreach (var accommodationReservation in _accommodationReservations)
            {
                if (accommodationReservation.Id != reservationId && accommodationReservation.AccommodationId == accommodationId)
                {
                    if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<DateTime> FindDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> resultingDates = new List<DateTime>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                resultingDates.Add(date);
            }

            return resultingDates;
        }

        public void AcceptRequest(Request selectedRequest)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);

            _accommodationReservations.Find(r => r.Id == selectedRequest.ReservationId).StartDate = selectedRequest.NewStartDate;
            _accommodationReservations.Find(r => r.Id == selectedRequest.ReservationId).EndDate = selectedRequest.NewEndDate;

            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

    }
}
