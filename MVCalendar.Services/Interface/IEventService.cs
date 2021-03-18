using MVCalendar.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCalendar.Services
{
    public interface IEventService
    {
        public Task<dynamic> SaveNewEvent(EventViewModel e); // method to save new calendar event to DB
        public Task<bool> UpdateEvent (EventViewModel e); // method to update new calendar event in DB
        public Task Delete(Guid id); //method to remove calendar event from DB
        public Task<List<EventViewModel>> GetAllEvents(CalendarUser user); // method to collect all events of user's families
        public Task<List<Event>> CollectEventsReminders(); // method to collect all event, which are pending for reminder
        bool EventExists(Guid? iD); // check by ID if event exists in DB
        public Event GetEvent(Guid? id);// get event from DB
    }
}
