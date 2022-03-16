using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Identity
{
    public class ManagerRole : IdentityRole
    {
        private static readonly string _name = "Manager";

        public static string RoleName => _name;

        public ManagerRole() : base(_name)
        {

        }
    }
}
