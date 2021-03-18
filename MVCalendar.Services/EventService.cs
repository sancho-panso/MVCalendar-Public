using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCalendar.Data;
using MVCalendar.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCalendar.Services
{
    public class EventService:IEventService
    {
        private readonly Context _context;
        private readonly IFamilyService _familyService;
        public EventService(Context context, 
                            IFamilyService familyService)
        {
            _context = context;
            _familyService = familyService;
        }
        public async Task Delete(Guid id)
        {
            _context.Events.Remove(GetEvent(id));
            await _context.SaveChangesAsync();
        }

        public async Task<dynamic> SaveNewEvent(EventViewModel e)
        {
            Guid familyGuid = Guid.Parse(e.FamilyID);
            var family = await _familyService.GetFamilyById(familyGuid);
            Event newEvent = new Event();
            Status eventStatus;
            Enum.TryParse(e.EventStatus, out eventStatus);
            newEvent.Subject = e.Subject;
            newEvent.Start = e.Start;
            newEvent.End = e.End;
            newEvent.Description = e.Description;
            newEvent.MessageFrom = e.MessageFrom;
            newEvent.MesageTo = e.MessageTo;
            newEvent.Family = family;
            newEvent.EventStatus = eventStatus;
            newEvent.IsFullDay = e.IsFullDay;
            newEvent.ThemeColor = e.ThemeColor;
            newEvent.SendToAll = e.SendToAll;
            newEvent.SendReminder = e.SendReminder;
            newEvent.LastModified = DateTime.Now;
            _context.Add(newEvent);
            try
            {
                await _context.SaveChangesAsync();
                return newEvent.ID;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                if (!EventExists(e.ID))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            };
        }

        public async Task<bool> UpdateEvent(EventViewModel e)
        {
            Event evnt = GetEvent(e.ID);
            Status eventStatus;
            Enum.TryParse(e.EventStatus, out eventStatus);
            evnt.Subject = e.Subject;
            evnt.Start = e.Start;
            evnt.End = e.End;
            evnt.Description = e.Description;
            evnt.MessageFrom = e.MessageFrom;
            evnt.MesageTo = e.MessageTo;
            evnt.EventStatus = eventStatus;
            evnt.IsFullDay = e.IsFullDay;
            evnt.ThemeColor = e.ThemeColor;
            evnt.SendToAll = e.SendToAll;
            evnt.SendReminder = e.SendReminder;
            evnt.LastModified = DateTime.Now;
            try // concurency check, if event was updated by another user, since data was retrieved from DB to memory by user
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                if (!EventExists(e.ID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            };

        }
        
        public async Task<List<EventViewModel>> GetAllEvents(CalendarUser user)
        {
            //create list of IDes of all user groups(families)
            var userFamilies = await _familyService.GetUserFamilies(user);
            List<Guid> evtList = new List<Guid>();
            foreach (var item in userFamilies)
            {
                evtList.Add(item.ID);
            }
            // query for all events which depends to user's families
            var events = await _context.Events.Where(evt => evtList.Contains(evt.Family.ID)).Include(evt=>evt.Family).ToListAsync();
            // create and map new list of events view models
            List<EventViewModel> model = new List<EventViewModel>();
            foreach (var item in events)
            {
                EventViewModel evnt = new EventViewModel();
                evnt.ID = item.ID;
                evnt.Subject = item.Subject;
                evnt.Description = item.Description;
                evnt.Start = item.Start;
                evnt.End = item.End;
                evnt.MessageFrom = item.MessageFrom;
                evnt.MessageTo = item.MesageTo;
                evnt.FamilyID = item.Family.ID.ToString();
                evnt.FamilyName = item.Family.Name;
                evnt.EventStatus = item.EventStatus.ToString();
                evnt.ThemeColor = item.ThemeColor;
                evnt.IsFullDay = item.IsFullDay;
                evnt.SendReminder = item.SendReminder;
                evnt.SendToAll = item.SendToAll;
                model.Add(evnt);
            }
            return model;
        }

        public bool EventExists(Guid? id)
        {
            return _context.Events.Any(e => e.ID == id);
        }

        public Event GetEvent(Guid? id)
        {
            Event evnt = _context.Events.Where(evn => evn.ID == id).Include(evn => evn.Family).FirstOrDefault();
            return evnt;
        }

        public async Task<List<Event>> CollectEventsReminders()
        {
            //query for all events which requires SMS reminders
            var events = await _context.Events.Include(evt => evt.Family).ToListAsync();
            // set the list of events due to send reminder and remove reminder flag from them to prevent reminder repreat
            // SMS reminder is send if rimender flag is true and less than 60 minutes left till the beginning of event
            List<Event> smsEvents = new List<Event>();
            foreach (var item in events)
            {
                if (item.SendReminder)
                {
                    if (item.Start < DateTime.Now && (DateTime.Now - item.Start).TotalMinutes <60)
                    {
                        item.SendReminder = false;
                        await _context.SaveChangesAsync();
                        smsEvents.Add(item);
                    }
                }
            }
            return smsEvents;
        }

        
    }
}
