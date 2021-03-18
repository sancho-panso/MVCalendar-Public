using MVCalendar.Domains;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCalendar.Services
{
    public interface IFamilyService
    {
        Task<Family> GetFamilyById(Guid? id); // get family class object from DB by object ID
        Task<CalendarUser> GetUser(string Id); // get user's class object from DB by users' ID
        Task<List<Family>> GetUserFamilies(CalendarUser user); // get all groups(families), where user is a member
        Task<Family> GetUserFamily(CalendarUser user); // get first user group
        Task IncludeUser(CalendarUser user, Family family); // add user to group
        Task RemoveUser(CalendarUser user, Family family); //  remove user from group
        Task WriteToDB(Family family); // write family object to DB
        Task<List<CalendarUser>> GetFamilyUsers(Guid id); // receive list of all family users
    }
}