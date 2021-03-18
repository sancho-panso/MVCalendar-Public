using System;
using System.Collections.Generic;

namespace MVCalendar.Domains
{
    //this class describes users'group, family
    public class Family
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CreatorID { get; set; }
        public List<CalendarUser> FamilyMembers { get; set; }
        public List<FamiliesCalendarUsers> FamiliesCalendarUsers { get; set; }

        public Family()
        {
            Guid ID = Guid.NewGuid();
            FamilyMembers = new List<CalendarUser>();
        }
    }
}