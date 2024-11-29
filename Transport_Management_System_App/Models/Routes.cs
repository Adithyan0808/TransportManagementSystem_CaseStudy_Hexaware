using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Routes
    {

        private int routeID;
        private String startDestination;
        private String endDestination;
        private Decimal distance;

        public Routes() { }

        public Routes(int routeID, string startDestination, string endDestination, Decimal distance)
        {
            this.RouteID = routeID;
            this.StartDestination = startDestination;
            this.EndDestination = endDestination;
            this.Distance = distance;
        }

        public int RouteID { get => routeID; set => routeID = value; }
        public string StartDestination { get => startDestination; set => startDestination = value; }
        public string EndDestination { get => endDestination; set => endDestination = value; }
        public Decimal Distance { get => distance; set => distance = value; }

        public override string ToString()
        {
            return $"\tRoute ID : {routeID,-5} Start Destination : {StartDestination,-15} End Destination : {endDestination,-15} Distance : {distance}";
        }


    }
}
