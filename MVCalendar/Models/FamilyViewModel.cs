using Microsoft.AspNetCore.Mvc.Rendering;
using MVCalendar.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCalendar.Models
{
    public class FamilyViewModel
    {
        public Family Family { get; set; }
        public string SelectedUser { get; set; }
        public string SelectedMember { get; set; }
        public string SelectedFamily { get; set; }
        public List<SelectListItem> AllUsers { get; set; }
        public List<SelectListItem> AllMembers { get; set; }
        public List<SelectListItem> AllFamilies { get; set; }

    }
}
