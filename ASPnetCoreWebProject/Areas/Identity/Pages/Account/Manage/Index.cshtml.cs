using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;

namespace PizzaWebsite.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPizzaWebsiteRepository _pizzaWebsiteRepository;

        public IndexModel(UserManager<IdentityUser> userManager,
                          SignInManager<IdentityUser> signInManager,
                          IPizzaWebsiteRepository pizzaWebsiteRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _pizzaWebsiteRepository = pizzaWebsiteRepository;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public List<CartItem> Items { get; set; }
        public Dictionary<Order, List<CartItem>> OrderDetails { get; set; } = new Dictionary<Order, List<CartItem>>();
        public Dictionary<Order, decimal> TotalForeachOrder { get; set; } = new Dictionary<Order, decimal>();

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.PostalCode)]
            public string PostalCode { get; set; }

            [Display(Name = "Delivery Area")]
            public string DeliveryArea { get; set; }

            [Display(Name = "Delivery Address")]
            public string DeliveryAddress { get; set; }
            public string Email { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            var userName = await _userManager.GetUserNameAsync(user);
            Username = userName;

            // get user data
            var userData = _pizzaWebsiteRepository.GetUserDataByUserId(user.Id);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);


            Orders = _pizzaWebsiteRepository.GetAllOrdersbyUserId(user.Id);

            foreach (var order in Orders) {
                TotalForeachOrder.Add(order, _pizzaWebsiteRepository.GetOrderTotal(order.CartId));
                Items = _pizzaWebsiteRepository.GetCartItemDetailsByCardId(order.CartId);
                OrderDetails.Add(order, Items);
            }
         
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = email,
                ProfilePicture = userData.ProfilePicture,
                DeliveryAddress = userData.DeliveryAddress,
                PostalCode = userData.PostalCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Email.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var userData = _pizzaWebsiteRepository.GetUserDataByUserId(user.Id);
            bool modified = false;
            if (Input.FirstName != userData.FirstName)
            {
                userData.FirstName = Input.FirstName;
                modified = true;
            }
            if (Input.LastName != userData.LastName)
            {
                userData.LastName = Input.LastName;
                modified = true;
            }
            if (Input.DeliveryAddress != userData.DeliveryAddress)
            {
                userData.DeliveryAddress = Input.DeliveryAddress;
                modified = true;
            }
            if (Input.PostalCode == Input.PostalCode)
            {
                userData.PostalCode = Input.PostalCode;
                modified = true;
            }
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    userData.ProfilePicture = dataStream.ToArray();
                    modified = true;
                }
            }

            // if the user data was modified
            if (modified)
            {
                // update the user data
                _pizzaWebsiteRepository.Update(userData);

                // save changes
                if (!_pizzaWebsiteRepository.SaveAll())
                {
                    StatusMessage = "Unexpected error when trying to update your profile.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
