using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCalendar.Data;
using MVCalendar.Domains;
using MVCalendar.Models;
using MVCalendar.Services;

namespace MVCalendar.Controllers
{
    [Authorize]
    public class FamiliesController : Controller
    {
        private readonly Context _context;
        private readonly IFamilyService _familyService;
        private readonly UserManager<CalendarUser> _userManager;

        public FamiliesController(Context context,
                                UserManager<CalendarUser> userManager,
                                IFamilyService familyService)
        {
            _context = context;
            _familyService = familyService;
            _userManager = userManager;
        }

        // GET: Families
        public async Task<IActionResult> Index()
        {
            CalendarUser user = await _userManager.GetUserAsync(User);
            var families = await _familyService.GetUserFamilies(user);
            ViewBag.deletingMessage = TempData["tryDeleteMessage"];
            ViewBag.editingMessage = TempData["tryEditMessage"];
            return View(families);
        }
        // GET: Families/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.WarningMessage = TempData["warningMessage"];
            return View();
        }

        // POST: Families/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FamilyViewModel model)
        {
            if (ModelState.IsValid)
            {
                CalendarUser user = await _userManager.GetUserAsync(User);
                model.Family.FamilyMembers.Add(user);
                model.Family.CreatorID = user.Id;
                model.Family.ID = Guid.NewGuid();
                Family family = model.Family;
                await _familyService.WriteToDB(family);
                return RedirectToAction(nameof(Index));
            }   
            return View(model);
        }

        // GET: Families/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            CalendarUser user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            var family = await _familyService.GetFamilyById(id);
            if (family == null)
            {
                return NotFound();
            }
            if (family.CreatorID != user.Id.ToString())
            {
                TempData["tryEditMessage"] = "Only creator of group has right to edit it";
                return RedirectToAction("Index", "Families");
            }
            return View(family);
        }

        // POST: Families/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name")] Family family)
        {
            if (id != family.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(family);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyExists(family.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(family);
        }
        // GET: Families/AddMember
        [HttpGet]
        public async Task<IActionResult> AddMember(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var family = await _context.Families.Where(f => f.ID == id)
                                                .Include(f => f.FamilyMembers).FirstOrDefaultAsync();
            if (family == null)
            {
                return NotFound();
            }

            var selectedUsers = await UsersSelectList(family);
            var model = new FamilyViewModel();

            model.SelectedUser = " ";
            model.Family = family;
            model.AllUsers = selectedUsers;

            return View(model);
        }
        // POST: Families/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(Guid id, FamilyViewModel model)
        {
            if (id != model.Family.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var family = await _familyService.GetFamilyById(id);
                var user = await _familyService.GetUser(model.SelectedUser);
                await _familyService.IncludeUser(user, family);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        // GET: Families/RemoveMember
        [HttpGet]
        public async Task<IActionResult> RemoveMember(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var family = await _context.Families.Where(f => f.ID == id)
                                                .Include(f => f.FamilyMembers).FirstOrDefaultAsync();
            if (family == null)
            {
                return NotFound();
            }

            var selectedUsers = await MembersSelectList(family);
            var model = new FamilyViewModel();

            model.SelectedUser = " ";
            model.Family = family;
            model.AllMembers = selectedUsers;

            return View(model);
        }
        // POST: Families/RemoveMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(Guid id, FamilyViewModel model)
        {
            if (id != model.Family.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var family = await _familyService.GetFamilyById(id);
                var user = await _familyService.GetUser(model.SelectedMember);
                await _familyService.RemoveUser(user, family);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Families/Delete
        public async Task<IActionResult> Delete(Guid? id)
        {
            CalendarUser user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Families
                .FirstOrDefaultAsync(m => m.ID == id);
            if (family == null)
            {
                return NotFound();
            }
            if (family.CreatorID != user.Id.ToString())
            {
                TempData["tryDeleteMessage"] = "Only creator of group has right to delete it";
                return RedirectToAction("Index", "Families");
            }


            return View(family);
        }

        // POST: Families/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var family = await _context.Families.FindAsync(id);
            _context.Families.Remove(family);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyExists(Guid id)
        {
            return _context.Families.Any(e => e.ID == id);
        }

        //method generate options list, which contains all registered users names, who are not already assigned to family
        public async Task<List<SelectListItem>> UsersSelectList(Family family)
        {
            var familyUsers = family.FamilyMembers;
            List<string> membersIdList = new List<string>();
            foreach (var item in familyUsers)
            {
                membersIdList.Add(item.Id);
            }
            var selectList = await _userManager.Users.Where(u => !membersIdList.Contains(u.Id)).AsNoTracking()
                                                .OrderBy(u => u.Name)
                                                .Select(n => new SelectListItem
                                                {
                                                    Value = n.Id.ToString(),
                                                    Text = n.Name
                                                }).ToListAsync();

            return selectList;
        }

        //method generate options list, which contains all family members list
        public async Task<List<SelectListItem>> MembersSelectList(Family family)
        {
            var familyUsers = family.FamilyMembers;
            List<string> membersIdList = new List<string>();
            foreach (var item in familyUsers)
            {
                membersIdList.Add(item.Id);
            }
            var selectList = await _userManager.Users.Where(u => membersIdList.Contains(u.Id)).AsNoTracking()
                                                .OrderBy(u => u.Name)
                                                .Select(n => new SelectListItem
                                                {
                                                    Value = n.Id.ToString(),
                                                    Text = n.Name
                                                }).ToListAsync();

            return selectList;
        }

        //method generate options list, which contains all users groups(families)
        public async Task<List<SelectListItem>> FamiliesSelectList(CalendarUser user, Family family)
        {

            var familiesList = await _context.Families.Where(f => f.FamilyMembers.Contains(user)).AsNoTracking()
                                                .OrderBy(f => f.Name)
                                                .Select(n => new SelectListItem
                                                {
                                                    Value = n.ID.ToString(),
                                                    Text = n.Name,
                                                    Selected = (n.ID == family.ID)
                                                }).ToListAsync();
            return familiesList;
        }
    }
}
