using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public class TourNotification : ISerializable
    {
        public int Id{ get; set; } 
        public string TextContent { get; set; }
        public bool Seen { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int UserId { get; set; }
        public int GuideId { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public TourNotification(string textContent, bool seen, DateTime arrivalTime, int userId, int guideId, int tourId)
        {
            TextContent = textContent;
            Seen = seen;
            ArrivalTime = arrivalTime;
            UserId = userId;
            GuideId = guideId;
            TourId = tourId;
        }
        public TourNotification()
        {
            
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TextContent, Seen.ToString(), ArrivalTime.ToString(), UserId.ToString(), GuideId.ToString(), TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TextContent = values[1];
            Seen = bool.Parse(values[2]);
            ArrivalTime = DateTime.Parse(values[3]);
            UserId = int.Parse(values[4]);
            GuideId = int.Parse(values[5]);
            TourId = int.Parse(values[6]);
        }
    }
}
