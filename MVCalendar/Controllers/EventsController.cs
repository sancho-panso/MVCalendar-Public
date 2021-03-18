using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCalendar.Data;
using MVCalendar.Domains;
using MVCalendar.Models;
using MVCalendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IFamilyService _familyService;
        private readonly UserManager<CalendarUser> _userManager;
        private readonly IEventService  _eventService;
        private readonly SmsController _smsService;
        private readonly FamiliesController _famControlService;

        public EventsController(IEventService eventService,
                                SmsController smsService,
                                FamiliesController famControlService,
                                IFamilyService familyService,
                                UserManager<CalendarUser> userManager)
        {
            _eventService = eventService;
            _smsService = smsService;
            _famControlService = famControlService;
            _userManager = userManager;
            _familyService = familyService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            CalendarUser user = await _userManager.GetUserAsync(User);
            List<Family> userFamilies = await _familyService.GetUserFamilies(user);
            // check if user has created group, otherwise redirected to create view
            if (userFamilies.Count() == 0)
            {
                TempData["warningMessage"] = "In order to visit Calendar,please create family group first";
                return RedirectToAction("Create", "Families");
            }
            // generate view model of user families and members in SelectedItemLists, set active group
            var family = userFamilies.First();
            var model = new FamilyViewModel();
            model = await UpdateModel(user, family, model);

            return View(model);
        }


        // set active group on user choice from option list, which contains all users groups
        [HttpPost]
        public async Task<IActionResult> Index(FamilyViewModel model)
        {
            ModelState.Clear();
            CalendarUser user = await _userManager.GetUserAsync(User);
            Guid guidID = Guid.Parse(model.SelectedFamily);
            var family = await _familyService.GetFamilyById(guidID);
            model = await UpdateModel(user, family, model);

            return View(model);

        }



        // API request for user events from frontend
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventViewModel>>> GetEventsAsync()
        {
            CalendarUser user = await _userManager.GetUserAsync(User);
            return await _eventService.GetAllEvents(user);
        }
        // API request to save or update event in DB
        [HttpPost]
        public async Task<ActionResult> SaveEvent(EventViewModel e)
        {
            if (ModelState.IsValid)
            {
                if (e.ID != null)
                {
                    if (_eventService.EventExists(e.ID))
                    {
                        await _eventService.UpdateEvent(e);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var result = await _eventService.SaveNewEvent(e);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        await _smsService.SmsEvent(result); // send SMS to user/users, after it is saved in DB
                    }
                    
                }
                return Ok();
            }
            return BadRequest();
        }
        // API request to delete event from DB
        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(Guid eventID)
        {
            if (!_eventService.EventExists(eventID))
            {
                return NotFound();
            }
            await _eventService.Delete(eventID);
            return NoContent();
        }

        //mapper for view model, when active user group is changed
        private async Task<FamilyViewModel> UpdateModel(CalendarUser user, Family family, FamilyViewModel model)
        {
            
            var selectedUsers = await _famControlService.MembersSelectList(family);
            var allUsers = await _famControlService.UsersSelectList(family);
            var selectedFamilies = await _famControlService.FamiliesSelectList(user, family);
            model.SelectedUser = user.Id; ;
            model.SelectedMember = " ";
            model.SelectedFamily = family.ID.ToString();
            model.Family = family;
            model.AllUsers = allUsers;
            model.AllMembers = selectedUsers;
            model.AllFamilies = selectedFamilies;

            return model;
        }
    }
}
