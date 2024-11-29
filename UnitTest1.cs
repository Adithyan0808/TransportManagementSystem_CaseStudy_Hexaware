
using Transport_Management_System_App.Models;
using Transport_Management_System_App.Repository;
using Transport_Management_System_App.myexceptions;
using NPOI.OpenXmlFormats.Dml.Diagram;


namespace TransportManagementSystem
{
    
    public class Tests
    {
        TransportManagementServiceImpl repo = new TransportManagementServiceImpl();
        [Test]
        public void TestAllocateDriver()
        {
            int tripId = 1;
            int driverId = 4;
            Assert.IsFalse(repo.allocateDriver(tripId,driverId));
        }

        [Test]
        public void TestAddVehicle()
        {
            String model = "Test_Vehicle";
            decimal capacity = 10;
            String type = "Test_Type";
            String status = "Test_Available";

            Vehicles vehicle = new Vehicles(model, capacity, type, status); 

            bool result = repo.addVehicle(vehicle);
            Assert.IsTrue(result);

        }
        [Test]
        public void TestBooking()
        {
            int tripId = 1;
            int passengerId = 2;
            string date = DateTime.Now.ToString();
    
            bool result = repo.bookTrip(tripId, passengerId, date);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestVehicleNotFoundExcpetion()
        {

            var ex =  Assert.Throws<VehicleNotFoundException>(() => { repo.deleteVehicle(1234); });
            Assert.That(ex.Message, Is.EqualTo("Vehicle not found exception vehicle ID : 1234"));

        }
        [Test]
        public void TestBookingNotFoundException()
        {
            var ex = Assert.Throws<BookingNotFoundException>(() => { repo.cancelBooking(1234); });
            Assert.That(ex.Message, Is.EqualTo("Booking ID : 1234 not found exception !"));
        }


    }
}