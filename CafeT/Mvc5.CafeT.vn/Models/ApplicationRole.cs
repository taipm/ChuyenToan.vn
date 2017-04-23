using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Mvc5.CafeT.vn.Models
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        // Add any custom Role properties/code here
    }
}