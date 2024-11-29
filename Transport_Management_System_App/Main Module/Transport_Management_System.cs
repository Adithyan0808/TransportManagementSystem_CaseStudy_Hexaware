using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport_Management_System_App.Service;


namespace Transport_Management_System_App.Main_Module
{
    internal class Transport_Management_System
    {
        public void application_main()
        {
            Vehicle_service app = new Vehicle_service();
         
            String exit = "yes";
            while(exit == "yes")
            {
                Console.WriteLine("                  ===== MENU =====    \n");
                Console.WriteLine("\t\t1. Add Vehicle");
                Console.WriteLine("\t\t2. update Vehicle");
                Console.WriteLine("\t\t3. Delete Vehicle");
                Console.WriteLine("\t\t4. Schedule Trip");
                Console.WriteLine("\t\t5. Cancel Trip");
                Console.WriteLine("\t\t6. Book Trip");
                Console.WriteLine("\t\t7. Cancel Booking");
                Console.WriteLine("\t\t8. Allocate Driver");
                Console.WriteLine("\t\t9. Deallocate Driver");
                Console.WriteLine("\t\t10. Get Bookings By Passenger");
                Console.WriteLine("\t\t11. Get Bookings By Trip");
                Console.WriteLine("\t\t12. Get Available Drivers");
                Console.WriteLine("\t\t13. EXIT");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------");
                Console.WriteLine();
                Console.Write("Enter Choice : ");
                int choice = int.Parse(Console.ReadLine());

                


                switch(choice)
                {
                    case 1:
                        app.addVehicle(); // working
                        break;
                    case 2:
                        app.updateVehicle(); // working
                        break;
                    case 3:
                        app.deleteVehicle(); // working
                        break;
                    case 4:
                        app.scheduleTrip(); // working
                        break;
                    case 5:
                        app.cancelTrip(); // working - 
                        break;
                    case 6:
                        app.bookTrip(); // working - logical
                        break;
                    case 7:
                        app.cancelBooking(); // working
                        break;
                    case 8:
                        app.allocateDriver(); // working
                        break;
                    case 9:
                        app.deallocateDriver(); // working
                        break;
                    case 10:
                        app.getBookingsByPassenger();
                        break;
                    case 11:
                        app.getBookingsByTrip();
                        break;
                    case 12:
                        app.getAvailableDrivers();// working
                        break;
                    case 13:
                        exit = "no";
                        break;

                }








            }
            













        }
    }
}
