using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Vehicles
    {

        private int vehicleID;
        private String model;
        private decimal capacity;
        private String type;
        private string status;

        public Vehicles()
        {
        }

        public Vehicles(string model, decimal capacity, string type, string status)
        {
            this.Model = model;
            this.Capacity = capacity;
            this.Type = type;
            this.Status = status;
        }

        public int VehicleID { get => vehicleID; set => vehicleID = value; }
        public string Model { get => model; set => model = value; }
        public decimal Capacity { get => capacity; set => capacity = value; }
        public string Type { get => type; set => type = value; }
        public string Status { get => status; set => status = value; }


        public override string ToString()
        {
            //return $"Vehicle ID : {vehicleID}\t\tModel : {model}\t\tCapacity : {capacity}\t\tType : {Type}\t\tStatus : {status}";
            return $"\tVehicle ID: {vehicleID,-3}  Model: {model,-22}  Capacity: {capacity,-6:N2}  Type: {type,-10}  Status: {status}";

        }


        // -22 in model represent model atleast occupy 22 chars event it dont have 22 chars

    }
}
