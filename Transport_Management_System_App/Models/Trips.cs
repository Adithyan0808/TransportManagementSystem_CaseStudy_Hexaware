using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Trips
    {

        private int tripID;
        private int vechicleID;
        private int routeID;
        private String departureDate;
        private String arrivalDate;
        private String status;
        private String tripType;
        private int maxPassengers;
        private int driverId;

        public Trips(int vechicleID, int routeID, String departureDate, String arrivalDate, string status, string tripType, int maxPassengers,int driverId)
        {

            this.VechicleID = vechicleID;
            this.RouteID = routeID;
            this.DepartureDate = departureDate;
            this.arrivalDate = arrivalDate;
            this.Status = status;
            this.TripType = tripType;
            this.MaxPassengers = maxPassengers;
            this.DriverId = driverId;
        }

        public int TripID { get => tripID; set => tripID = value; }
        public int VechicleID { get => vechicleID; set => vechicleID = value; }
        public int RouteID { get => routeID; set => routeID = value; }
        public String DepartureDate { get => departureDate; set => departureDate = value; }
        public String ArrivalDate { get => ArrivalDate; set => ArrivalDate = value; }
        public string Status { get => status; set => status = value; }
        public string TripType { get => tripType; set => tripType = value; }
        public int MaxPassengers { get => maxPassengers; set => maxPassengers = value; }
        public int DriverId { get => driverId; set => driverId = value; }
    }
}
