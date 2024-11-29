using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.myexceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(String msg):base(msg) 
        {
            
        }
    }
}
