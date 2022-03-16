using Microsoft.AspNetCore.Identity;

namespace PizzaWebsite.Data.Identity
{
    public class OwnerRole : IdentityRole
    {
        private static readonly string _name = "Owner";

        public static string RoleName => _name;

        public OwnerRole() : base(_name)
        {

        }
    }
}
