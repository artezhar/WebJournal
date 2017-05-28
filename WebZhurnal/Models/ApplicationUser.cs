﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebZhurnal.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public Group Group { get; set; }
        public int? GroupId { get; set; }

        public List<TeacherGroup> TeacherGroups { get; set; }
    }
}
