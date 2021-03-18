using System;
using System.Collections.Generic;
using System.Text;

namespace MVCalendar.Domains
{
    // enum describes possible statuses of calendar events
    public enum Status {publicEvent, privateEvent, parentsOnly, kidsOnly}
    // his class describe calendar event entity stored in DB
    public class Event
    {
        public Guid ID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string MessageFrom { get; set; }
        public string MesageTo { get; set; }
        public Family Family { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
        public bool SendToAll { get; set; }
        public bool SendReminder { get; set; }
        public Status EventStatus { get; set; }
        public DateTime LastModified { get; set; }

        public Event()
        {
            Guid ID = Guid.NewGuid();
        }
    }
}
