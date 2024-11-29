using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.myexceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(String msg) : base(msg)
        {

        }
    }
}
