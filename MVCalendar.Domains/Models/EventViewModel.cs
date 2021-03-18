using System;
using System.Collections.Generic;
using System.Text;

namespace MVCalendar.Domains
{
    public class EventViewModel
    {
        //View model of fullcallendar fronend plugin event
        public Guid? ID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        public string FamilyID { get; set; }
        public string FamilyName { get; set; }
        public string EventStatus { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public bool SendToAll { get; set; }
        public bool SendReminder { get; set; }
    }
}
