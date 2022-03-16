using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Identity
{
    public class DelivererRole : IdentityRole
    {
        private static readonly string _name = "Deliverer";

        public static string RoleName => _name;

        public DelivererRole() : base(_name)
        {

        }
    }
}
