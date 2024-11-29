using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Driver
    {
        private int driverId;
        private String driverName;
        private int status;

        public Driver()
        {
        }

        public Driver(int driverId, string driverName, int status)
        {
            this.DriverId = driverId;
            this.DriverName = driverName;
            this.Status = status;
        }

        public int DriverId { get => driverId; set => driverId = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public int Status { get => status; set => status = value; }

        public override string ToString()
        {
            return $"Driver ID : {driverId}\tDriver Name : {driverName}";
        }
    }
}
