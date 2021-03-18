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
    public class FamilyService : IFamilyService
    {
        private readonly Context _context;
        private readonly UserManager<CalendarUser> _userManager;

        public FamilyService(Context context,
                             UserManager<CalendarUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Family>> GetUserFamilies(CalendarUser user)
        {
            List<Family> families = await _context.Families.Where(f => f.FamilyMembers.Contains(user))
                                                   .Include(f => f.FamilyMembers).ToListAsync();
            return families;
        }

        public async Task<Family> GetUserFamily(CalendarUser user)
        {
            Family family = await _context.Families.Where(f => f.FamilyMembers.Contains(user))
                                                   .Include(f => f.FamilyMembers).FirstOrDefaultAsync();
            return family;
        }
        public async Task WriteToDB(Family family)
        {
            _context.Add(family);
            await _context.SaveChangesAsync();
        }

        public async Task<Family> GetFamilyById(Guid? id)
        {
            var family = await _context.Families.Where(f => f.ID == id)
                                               .Include(f => f.FamilyMembers)
                                               .FirstOrDefaultAsync();
            return family;
        }

        public async Task<CalendarUser> GetUser(string Id)
        {
            var user = await _userManager.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();
            return user;
        }

        public async Task IncludeUser(CalendarUser user, Family family)
        {
            family.FamilyMembers.Add(user);
            _context.Update(family);
            await _context.SaveChangesAsync();
        } 
        public async Task RemoveUser(CalendarUser user, Family family)
        {
            family.FamilyMembers.Remove(user);
            _context.Update(family);
            await _context.SaveChangesAsync();
        }

        public async Task <List<CalendarUser>> GetFamilyUsers(Guid id)
        {
            var family = await GetFamilyById(id);
            var users = family.FamilyMembers;
            return users;
        }



    }
}
