using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Management_System_App.Models;

namespace Transport_Management_System_App.Repository
{
    public interface TransportManagementService
    {

        public bool addVehicle(Vehicles vehicle);
        public void updateVehicle(int vehicleId, Vehicles vehicle);
        public bool deleteVehicle(int vehicleID);
        public bool scheduleTrip(int vehicleId, int routeId, string departureDate, string arrivalDate);
        public bool cancelTrip(int tripId);
        public bool bookTrip(int tripId, int passengerId, string bookingDate);
        public void cancelBooking(int bookingId);
        public bool allocateDriver(int tripId, int driverId);
        public bool deallocateDriver(int tripId);
        public List<Bookings> getBookingByPassenger(int passengerId);
        public List<Bookings> getBookingsByTrip(int tripId);
        public List<Driver> getAvailableDrivers();




    }
}
