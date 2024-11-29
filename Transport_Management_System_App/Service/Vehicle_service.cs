using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using Transport_Management_System_App.Models;
using Transport_Management_System_App.myexceptions;
using Transport_Management_System_App.Repository;



namespace Transport_Management_System_App.Service
{
    public class Vehicle_service
    {
        TransportManagementServiceImpl repo = new TransportManagementServiceImpl();
      
        public void addVehicle()
        {
            try
            {
                // String interpolation {0,-20} -> left aligned , {0,20}-> right aligned , {0,^20} -> centre
                Console.Write("Enter model : ");
                String Model = Console.ReadLine();

                Console.Write("Enter capacity : ",-20);
                int Capacity = int.Parse(Console.ReadLine());

                Console.Write("Enter Type : ", -20);
                String Type = Console.ReadLine();

                Console.Write("Enter status : ", -20);
                String Status = Console.ReadLine();
                
                Vehicles vehicles = new Vehicles(Model,Capacity,Type,Status);

                repo.addVehicle(vehicles);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Vehicle added successfully...");
                Console.ResetColor();
                return;
            }
            catch (Exception e)
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }


        public void updateVehicle()
        {
            Console.WriteLine();
            List<Vehicles> veh = TransportManagementServiceImpl.GetAllVehicle();
            foreach (Vehicles v in veh)
            {
                Console.WriteLine(v.ToString());
            }
            Console.WriteLine();
            while (true)
            {
                try
                {
                    Console.Write("Enter Vechicle ID to update : ");
                    int id = int.Parse(Console.ReadLine());

                    try
                    {
                        repo.checkVehicle(id);
                    }
                    catch (VehicleNotFoundException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write("Enter model : ");
                    string model = Console.ReadLine();

                    Console.Write("Enter capacity : ");
                    int capacity = int.Parse(Console.ReadLine());

                    Console.Write("Enter Type : ");
                    string type = Console.ReadLine();

                    Console.Write("Enter status : ");
                    string status = Console.ReadLine();

                    Vehicles vehicle = new Vehicles(model, capacity, type, status);
                    try
                    {
                        repo.updateVehicle(id, vehicle);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Updated vehicle successfully !");
                        Console.ResetColor();
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.ToString());
                    Console.ResetColor();
                }
            }

        }

        public void deleteVehicle()
        {
            List<Vehicles> vehicle = TransportManagementServiceImpl.GetAllVehicle();
            foreach (Vehicles v in vehicle)
            {
                Console.WriteLine(v.ToString());
            }
            while (true)
            {
                try
                {
                    Console.WriteLine("\nEnter Vehicle ID : ");
                    int vehicleId = int.Parse(Console.ReadLine());

                    if (repo.deleteVehicle(vehicleId))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Deleted successfully !");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Vehicle not exist...");
                        Console.ResetColor();
                    }
                    return;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Try with valid vehicle ID");
                    Console.ResetColor();
                }
            }
        }

        public void scheduleTrip()
        {
            List<Vehicles> vehicle = TransportManagementServiceImpl.GetAllVehicleAvailable();
            foreach (Vehicles v in vehicle)
            {
                Console.WriteLine(v.ToString());
            }
            while (true)
            {
                Console.Write("Enter Vehicle ID : ");
                int vehicleId = int.Parse(Console.ReadLine());

                try
                {
                    repo.checkVehicle(vehicleId);
                }
                catch (VehicleNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    continue;
                }
                List<Routes> route =  repo.GetAllRoutes();
                foreach (Routes r in route)
                {
                    Console.WriteLine(r.ToString());
                }
                Console.Write("Enter Route ID : ");
                int routeId = int.Parse(Console.ReadLine());
                if (!repo.CheckRoute(routeId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter valid Route ID ");
                    Console.ResetColor();
                    continue;
                }

                Console.Write("Enter Departure Date : ");
                String departureDate = Console.ReadLine();

                Console.Write("Enter Arrival Date : ");
                string arrivalDate = Console.ReadLine();

                try
                {
                    if (repo.scheduleTrip(vehicleId, routeId, departureDate, arrivalDate))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Trip scheduled successfully...");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Trip schedule failed...");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return;
            }
        }


        public void cancelTrip()
        {
            Console.Write("Enter Trip Id : ");
            int tripId = int.Parse(Console.ReadLine());

            try
            {
                if(repo.cancelTrip(tripId))
                {
                    Console.ForegroundColor= ConsoleColor.Green;
                    Console.WriteLine("Trip Cancelled !");
                    Console.ResetColor();
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Trip Do Not Exist !");
                    Console.ResetColor();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void bookTrip()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Trip Id : ");
                    int tripId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Passenger ID : ");
                    int passengerId = int.Parse(Console.ReadLine());

                    if(repo.CheckBookTrip(tripId,passengerId))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Booking already exists!");
                        Console.ResetColor();
                        return;
                    }

                    DateTime date = DateTime.Now;
                    String bookingDate = date.ToString();


                    if (repo.bookTrip(tripId, passengerId, bookingDate))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Trip Booked Successfully !");
                        Console.ResetColor();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(ex.ToString());
                    Console.ResetColor();
                }
            }
        }

            

        public void cancelBooking()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Booking ID : ");
                    int bookingId = int.Parse(Console.ReadLine());


                    repo.cancelBooking(bookingId);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Cancelled Booking Successfully !");
                    Console.ResetColor();
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
        }


        public void allocateDriver()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter Trip ID : ");
                    int tripId = int.Parse(Console.ReadLine());

                    List<Driver> Driver_list = repo.getAvailableDrivers();
                    foreach(Driver d in Driver_list)
                    {
                        Console.WriteLine(d.ToString());
                    }
                    Console.WriteLine("Enter Driver ID : ");
                    int driverId = int.Parse(Console.ReadLine());

                    
                    if (repo.allocateDriver(tripId, driverId))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Driver Allocated !");
                        Console.ResetColor();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Driver Allocation Failed !");
                        Console.ResetColor();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        public void getBookingsByPassenger()
        {

            try
            {
                Console.Write("Enter Passenger ID : ");
                int passengerId = int.Parse(Console.ReadLine());


                List<Bookings> bookings = repo.getBookingByPassenger(passengerId);
                foreach (Bookings detail in bookings)
                {
                    Console.WriteLine(detail.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            
        }

        public void getBookingsByTrip()
        {
            while (true)
            {
                Console.Write("Enter Trip ID : ");
                int tripId = int.Parse(Console.ReadLine());

                try
                {
                    List<Bookings> bookings = repo.getBookingsByTrip(tripId);
                    foreach (Bookings item in bookings)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }

        }

        public void getAvailableDrivers()
        {
            try
            {
                List<Driver> driver_list = repo.getAvailableDrivers();
                foreach (Driver driver in driver_list)
                {
                    Console.WriteLine(driver.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }

        public void deallocateDriver()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Trip ID : ");
                    int tripId = int.Parse(Console.ReadLine());
                    if (repo.deallocateDriver(tripId))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Driver Deallocated Successfully !");
                        Console.ResetColor();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    Console.ResetColor();
                }
            }
        }

    }
}
