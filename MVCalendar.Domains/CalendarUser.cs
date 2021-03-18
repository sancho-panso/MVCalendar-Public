using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCalendar.Domains
{
    public class CalendarUser:IdentityUser
    {
        //this class describe user of program
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string FullName { get {return Name + " " + Lastname;} }
        public DateTime Birthday { get; set; }
        public List<Family> FamilyName { get; set; }
        public List<FamiliesCalendarUsers> FamiliesCalendarUsers { get; set; }
    }
}
