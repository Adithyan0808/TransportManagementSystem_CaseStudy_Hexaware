using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_Management_System_App.Models
{
    public class Passengers
    {
        private int passengerID;
        private String firstName;
        private String gender;
        private int age;
        private String email;
        private String phone;

        public Passengers()
        {
        }

        public Passengers(int passengerID, string firstName, string gender, int age, string email, string phone)
        {
            this.PassengerID = passengerID;
            this.FirstName = firstName;
            this.Gender = gender;
            this.Age = age;
            this.Email = email;
            this.Phone = phone;
        }

        public int PassengerID { get => passengerID; set => passengerID = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string Gender { get => gender; set => gender = value; }
        public int Age { get => age; set => age = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
    }
}
