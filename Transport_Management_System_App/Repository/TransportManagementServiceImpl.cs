using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Management_System_App.Models;
using Transport_Management_System_App.util;
using Transport_Management_System_App.myexceptions;
using System.Transactions;
namespace Transport_Management_System_App.Repository
{

    public class TransportManagementServiceImpl : TransportManagementService
    {
       
        //string connectionString = "Server=DESKTOP-6CA5ALC; Database=Transport Management System; Trusted_Connection=True";
        public bool addVehicle(Vehicles vehicle)
        {
            string query = "insert into vehicles values(@model, @capacity, @type, @status)";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@status", vehicle.Status);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

        }


        public void updateVehicle(int vehicleId,Vehicles vehicle)
        {
            //if(!checkVehicle(vehicleId))
            //{
            //    return false;
            //}

            StringBuilder query = new StringBuilder("UPDATE vehicles SET model = @model, capacity = @capacity, type = @type, status = @status ");
            query.Append("WHERE vehicleID = @vehicleID");
            String query_s = query.ToString();
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query_s, conn))
                {
                    cmd.Parameters.AddWithValue("@model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@status", vehicle.Status);
                    cmd.Parameters.AddWithValue("@vehicleID", vehicleId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (!(rowsAffected > 0))
                    {
                        throw new VehicleNotFoundException("Vehicle not found Exception!");
                        
                    }
                }
            }
            
        }


        // Helper method for Update vehicle , gett all available vehichle
        public static List<Vehicles> GetAllVehicleAvailable()
        {
            List<Vehicles> vehicles = new List<Vehicles>();
            String Query = "select * from Vehicles where status = 'Available'";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            Vehicles vehicle = new Vehicles();
                            vehicle.VehicleID = (int)reader["vehicleId"];
                            vehicle.Model = (String)reader["Model"];
                            vehicle.Capacity = (Decimal)reader["Capacity"];
                            vehicle.Status = (String)reader["Status"];
                            vehicle.Type = (String)reader["Type"];
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }
            return vehicles;

        }

        public static List<Vehicles> GetAllVehicle()
        {
            List<Vehicles> vehicles = new List<Vehicles>();
            String Query = "select * from Vehicles";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Vehicles vehicle = new Vehicles();
                            vehicle.VehicleID = (int)reader["vehicleId"];
                            vehicle.Model = (String)reader["Model"];
                            vehicle.Capacity = (Decimal)reader["Capacity"];
                            vehicle.Status = (String)reader["Status"];
                            vehicle.Type = (String)reader["Type"];
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }
            return vehicles;

        }



        public void checkVehicle(int id)
        {
            String query = "select * from vehicles where vehicleId = @vehicleId and status = 'Available'";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId", id);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return;
                        }
                        
                    }
                 
                }
            }
            throw new VehicleNotFoundException($"Vehicle Not Found Exception | Vehicle ID : {id}");
        }


        public bool deleteVehicle(int vehicleId)
        {
            string query = "delete from vehicles where vehicleID = @vehicleID";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleID", vehicleId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new VehicleNotFoundException($"Vehicle not found exception vehicle ID : {vehicleId}");
                    }
                }
            }

        }
        //d
        //f
        //check if the trip cancelled or not
        public static bool TripExistsOrNot(int tripId)
        {
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                String query = "select Status from Trips where tripId = @tripId and Status <> 'Cancelled'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripId", tripId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Trip is already Cancelled!");
                            Console.ResetColor();
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool cancelTrip(int tripId)
        {
            if(!TripExistsOrNot(tripId))
            {
                return false;
            }
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // trip status to canceled
                    string cancelTripQuery = "update Trips set Status = 'Cancelled' where TripID = @TripID";

                    using (SqlCommand cmd = new SqlCommand(cancelTripQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TripID", tripId);
                        cmd.Parameters.AddWithValue("@CancelledDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // booking status to cancelled
                    string cancelBookingsQuery = "update Bookings set Status = 'Cancelled' where TripID = @TripID";

                    using (SqlCommand cmd = new SqlCommand(cancelBookingsQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TripID", tripId);
                        cmd.ExecuteNonQuery();
                    }

                    // update driver status to 0
                    string updateDriverQuery ="update Driver set Status = 0 where driverId = (select driverId from Trips where tripId = @tripId)";

                    using (SqlCommand cmd = new SqlCommand(updateDriverQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TripID", tripId);
                        cmd.ExecuteNonQuery();
                    }

                   
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }



        //h,i

        public List<Bookings> getBookingByPassenger(int passengerId)
        {
            List<Bookings> bookings = new List<Bookings>();

            string query = "select * from Bookings where passengerID = @passengerID";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passengerID", passengerId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bookings booking_detail = new Bookings();
                        booking_detail.BookingID = (int)reader["bookingID"];
                        booking_detail.TripID = (int)reader["tripID"];
                        booking_detail.PassengerID = (int)reader["passengerID"];
                        booking_detail.BookingDate = (String)reader["bookingDate"];
                        booking_detail.Status = (string)reader["status"];
                        bookings.Add(booking_detail);
                    }
                }
            }

            return bookings;

        }



        public List<Bookings> getBookingsByTrip(int tripId)
        {
            List<Bookings> bookings = new List<Bookings>();

            string query = "select * from Bookings where tripID = @tripId";

            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripId", tripId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Bookings booking_detail = new Bookings();
                        booking_detail.BookingID = (int)reader["bookingID"];
                        booking_detail.TripID = (int)reader["tripID"];
                        booking_detail.PassengerID = (int)reader["passengerID"];
                        booking_detail.BookingDate = (String)reader["bookingDate"];
                        booking_detail.Status = (string)reader["status"];
                        bookings.Add(booking_detail);

                    }
                }
            }

            return bookings;

        }

        public void cancelBooking(int bookingId)
        {
            String query = "delete from bookings where bookingID = @bookingID ";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@bookingID", bookingId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(!(rowsAffected>0))
                    {
                        throw new BookingNotFoundException($"Booking ID : {bookingId} not found exception !");
                    }
                }
            }
            
        }

        public List<Routes> GetAllRoutes()
        {
            List<Routes> routes = new List<Routes>();
            String query = "select * from Routes";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using(SqlDataReader  reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Routes r = new Routes();
                            r.RouteID = (int)reader["RouteId"];
                            r.Distance = (Decimal)reader["Distance"];
                            r.StartDestination = (String)reader["StartDestination"];
                            r.EndDestination = (String)reader["EndDestination"];
                            routes.Add(r);
                        }
                    }
                }
            }
            return routes;

        }

        // Checks the Route ID available or not
        public bool CheckRoute(int routeId)
        {
            String Query = "select * from Routes where routeID = @routeID";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(Query,conn))
                {
                    cmd.Parameters.AddWithValue("@routeID",routeId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return true;
                    }

                }
            }
            return false;
        }


        public bool scheduleTrip(int vehicleId, int routeId, string departureDate, string arrivalDate)
        {
            //String query = null;
            //if(!vehicleAvailable(vehicleId))
            //{
            //    throw new VehicleNotFoundException($"Vehicle Not Found Exception | Vehicle ID : {vehicleId}");
            //}
            String query = "update vehicles set status = 'On Trip' where vehicleId = @vehicleId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId",vehicleId);
                    cmd.ExecuteNonQuery();
                }
            }
            

            query = "insert into trips values(@vehicleId,@routeId,@departureDate,@arrivalDate,'Scheduled','passenger',50,1)";
            using( SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                    cmd.Parameters.AddWithValue("@routeId",routeId);
                    cmd.Parameters.AddWithValue("@departureDate",departureDate);
                    cmd.Parameters.AddWithValue("@arrivalDate",arrivalDate);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }


        }

        // helper method for schedule Trip
        public static bool vehicleAvailable(int vehicleId)
        {
            String query = "Select status from vehicles where vehicleId = @vehicleId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@vehicleId",vehicleId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            String val = (String)reader["status"];
                            if (val == "Available")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //To check if the booking already exists or not
        public bool CheckBookTrip(int tripId,int passengerId)
        {
            String query = "select * from bookings where @tripId = tripId and passengerId = @passengerId and status = 'Confirmed'";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tripId", tripId);
                    cmd.Parameters.AddWithValue("@passengerId", passengerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {                          
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public bool bookTrip(int tripId, int passengerId, string bookingDate)
        {

            if (!TripAvailable(tripId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Trip not available !");
                Console.ResetColor();
                return false;
            }
            if (!PassengerAvailable(passengerId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("passenger not available");
                Console.ResetColor();
                return false;
            }           

            String query = "insert into bookings values(@tripId , @passengerId,@bookingDate , 'confirmed')";
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {   
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@tripId", tripId);
                        cmd.Parameters.AddWithValue("@passengerId", passengerId);
                        cmd.Parameters.AddWithValue("@bookingDate", bookingDate);
                        int rowsAffected = cmd.ExecuteNonQuery(); ;
                    }
                    query = "update trips set status = 'Scheduled' where tripId = @tripId";
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@tripId", tripId);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
                
            
            
            
        }

       

        // Helper class for bookTrip , check the existence of tripId
        public static bool TripAvailable(int tripId)
        {
            String query = "select * from trips where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@tripId",tripId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if( reader.Read())
                        {
                            return true;                    
                            
                        }
                    }
                }
            }
            return false;
        }


        // Helper class for bookTrip , check the existence of passenger
        public static bool PassengerAvailable(int passengerId)
        {
            String query = "select * from Passengers where passengerId = @passengerId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@passengerId",passengerId);
                    cmd.ExecuteNonQuery();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        

        public bool allocateDriver(int tripId, int driverId)
        {
            if(!TripAvailable(tripId))
            {
               
                Console.WriteLine("Trip not available !");
                return false;
            }
            if(!DriverAvailable(driverId))
            {
                Console.WriteLine("Driver not available");
                return false;
            }

            String query = "update trips set driverId = @driverId where tripId = @tripId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    cmd.Parameters.AddWithValue("@tripId",tripId);
                    cmd.ExecuteNonQuery();
                }
            }
            query = "update driver set status = 1 where driverId = @driverId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
           
        }

        // Helper method for allocate Driver method , check the driver is avaialble or not
        public static bool DriverAvailable(int driverId)
        {
            String query = "select * from Driver where driverId = @driverId";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@driverId",driverId);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            int status = (int)reader["status"];
                            if(status == 0)
                                return true;
                        }
                    }
                }
            }
            return false;

        }

        public bool deallocateDriver(int tripId)
        {
        try
        {
            using (SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string updateDriverStatusQuery = "update Driver set status = 0 where DriverId = (SELECT DriverId from Trips where TripID = @TripID)";
                    using (SqlCommand command = new SqlCommand(updateDriverStatusQuery, conn, transaction))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@TripID", tripId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("No driver found for the given trip.");
                        }
                    }

                    string updateTripQuery = "update Trips set DriverId = NULL WHERE TripID = @TripID";
                    using (SqlCommand command = new SqlCommand(updateTripQuery, conn, transaction))
                    {
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@TripID", tripId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("Trip not found.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ResetColor();
                    return false;
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }
          

        

        // driver id is foreign key , so do (on delete set null) , manually assign null , we cannt assign null to foreign key

        public List<Driver> getAvailableDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            string query = "select * from Driver where status = 0";
            using(SqlConnection conn = DBConnection.getConnection())
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Driver driver_detail = new Driver();
                            driver_detail.DriverName = (string)reader["DriverName"];
                            driver_detail.DriverId = (int)reader["driverId"];
                            driver_detail.Status = (int)reader["status"];
                            drivers.Add(driver_detail);
                        }
                        
                    }
                    
                }
            }
            return drivers;
        }

       





    }
}
