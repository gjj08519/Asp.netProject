using Microsoft.AspNetCore.Identity;

namespace PizzaWebsite.Data.Identity
{
    public class CookRole : IdentityRole
    {
        private static readonly string _name = "Cook";

        public static string RoleName => _name;

        public CookRole() : base(_name)
        {

        }
    }
}
