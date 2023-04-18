using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Domain.Models
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LenghtOfStay { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public User Guest { get; set; }
        public int GuestId { get; set; }

        public AccommodationReservation() { }

        public AccommodationReservation(int id, DateTime startDate, DateTime endDate, int lenghtOfStay, Accommodation accommodation, User guest)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            LenghtOfStay = lenghtOfStay;
            Accommodation = accommodation;
            AccommodationId = accommodation.Id;
            Guest = guest;
            GuestId = guest.Id;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                LenghtOfStay.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateTime.Parse(values[1]);
            EndDate = DateTime.Parse(values[2]);
            LenghtOfStay = Convert.ToInt32(values[3]);
            AccommodationId = Convert.ToInt32(values[4]);
            GuestId = Convert.ToInt32(values[5]);
        }
    }
}
