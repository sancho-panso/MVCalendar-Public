using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVCalendar.Domains;
using MVCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<CalendarUser> _userManager;
        private readonly SignInManager<CalendarUser> _signInManager;  
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistrationController(UserManager<CalendarUser> userManager,
                                      SignInManager<CalendarUser> signInManager,
                                      IConfiguration configuration,
                                      RoleManager<IdentityRole> roleManager
                                      )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //method to change users password
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("UserUpdated");
            }

            return View(model);
        }

        // method, where user can edit his registration details
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> EditUser()
        {
            CalendarUser user = await _userManager.GetUserAsync(User);
            EditUserViewModel model = new EditUserViewModel();
            model.Name = user.Name;
            model.Lastname = user.Lastname;
            model.Email = user.Email;
            model.Phone = user.PhoneNumber;
            model.Birthday = user.Birthday;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid) {
                    CalendarUser user = await _userManager.GetUserAsync(User);

                    user.UserName = model.Name;
                    user.Name = model.Name;
                    user.Lastname = model.Lastname;
                    user.Email = model.Email;
                    user.PhoneNumber = model.Phone;
                    user.Birthday = model.Birthday;

                    var userUpdate = await _userManager.UpdateAsync(user);
                    if (!userUpdate.Succeeded)
                    {
                        ViewBag.ErrorMessage = "Something gone wrong, please try again";
                        return View();
                    }
                    return View("UserUpdated");
            }
            return View(model);
        }
       
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            ViewBag.successMessage = TempData["successMessage"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignInViewModel model, string returnUrl)
        {
            CalendarUser user = await _userManager.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync(); 

            if (!ModelState.IsValid || user == null)
            {
                ViewBag.AuthError = "Wrong sign in details";
                return View("Login", model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.Name, model.Password, false, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AuthError = "Wrong sign in details";
            return View("Login", model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await EmailAlreadyInUse(model.Email))
                {
                    ViewBag.ErrorEmailMessage = ($"email {model.Email} is already in use");
                    return View("Index");
                } 
                if (await PhoneAlreadyInUse(model.Phone))
                {
                    ViewBag.ErrorPhoneMessage = ($"phone number {model.Phone} is already in use");
                    return View("Index");
                }
                CalendarUser newUser = new CalendarUser()
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Birthday = model.Birthday
                };

                var result = await _userManager.CreateAsync(newUser, model.ConfirmPassword);
                if (result.Succeeded)
                {
                    if (EmailCheckForAdmin(model.Email))
                    {
                        await _userManager.AddToRoleAsync(newUser, "Admin");
                    }
                    if (CheckForAge(model))
                    {
                        await _userManager.AddToRoleAsync(newUser, "Adult");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, "Kid");

                    }

                    TempData["successMessage"] = "Your registration was successfull, now you can login";
                    return RedirectToAction("Login");
                }
            }
            return View("~/Views/Registration/Index.cshtml", model);
        }

        // check user's email to set Admin role
        private bool EmailCheckForAdmin(string email)
        {
            return email.EndsWith(_configuration["ADMIN"]);
        }
        // check to set user role either adult or kid
        private bool CheckForAge(UserViewModel model)
        {
            DateTime today = DateTime.Now;
            int age = today.Year - model.Birthday.Year;
            return (age >= 18);
        }

        // methods checks if email is already in use
        private async Task<bool> EmailAlreadyInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return (user != null);
        }        
         
        //methods checks if phone is already in use
        private async Task<bool> PhoneAlreadyInUse(string phone)
        {
            var user = await _userManager.Users.Where(u=>u.PhoneNumber == phone).FirstOrDefaultAsync();
            return (user != null);
        }

    }
}
