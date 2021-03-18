using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCalendar.Domains
{
    //this class stores many-to-many relationshipes between users and families
    public class FamiliesCalendarUsers
    {
        public string CalendarUserId { get; set; }
        public CalendarUser CalendarUser { get; set; }

        public Guid FamilyId { get; set; }
        public Family Family { get; set; }
    }
}
