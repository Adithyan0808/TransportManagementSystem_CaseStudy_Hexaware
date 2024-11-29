using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Bookings
    {

        private int bookingID;
        private int tripID;
        private int passengerID;
        private String bookingDate;
        private String status;

        public Bookings()
        {
        }

        public Bookings(int tripID, int passengerID, string bookingDate, string status)
        {

            this.TripID = tripID;
            this.PassengerID = passengerID;
            this.BookingDate = bookingDate;
            this.Status = status;
        }

        public int BookingID { get => bookingID; set => bookingID = value; }
        public int TripID { get => tripID; set => tripID = value; }
        public int PassengerID { get => passengerID; set => passengerID = value; }
        public String BookingDate { get => bookingDate; set => bookingDate = value; }
        public string Status { get => status; set => status = value; }

        
        public override string ToString()
        {
            return $"\tbookingId : {bookingID}\n\tTrip ID : {tripID}\n\tPassenger ID : {passengerID}\n\tBooking Date : {BookingDate}\n\tStatus : {Status}\n\t-------------------------\n";
        }


    }
}
