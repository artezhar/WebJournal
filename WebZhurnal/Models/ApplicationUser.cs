using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebZhurnal.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        private void test()

        {

        }

        /*public UserType Type
        {
            get
            {
                return ennu Claims.SingleOrDefault(c => c.ClaimType == "Type").ClaimValue
                    }
        }*/
    }
}
