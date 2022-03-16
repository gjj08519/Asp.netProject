using Microsoft.AspNetCore.Identity;

namespace PizzaWebsite.Data.Identity
{
    public class FrontRole : IdentityRole
    {
        private static readonly string _name = "Front";

        public static string RoleName => _name;

        public FrontRole() : base(_name)
        {

        }
    }
}
