using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System.Diagnostics;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Controller that allows one to manage their cart.
    /// </summary>
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPizzaWebsiteRepository _pizzaRepository;
        private readonly IUserIdentityRepository _userIdentityRepository;

        public CartController(ILogger<CartController> logger,
            UserManager<IdentityUser> userManager,
            IPizzaWebsiteRepository pizzaRepository,
            IUserIdentityRepository userIdentityRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _pizzaRepository = pizzaRepository;
            _userIdentityRepository = userIdentityRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Checkout")]
        public IActionResult Checkout()
        {
            // Checkout can only be accessed if the user has items in their cart.
            if (_pizzaRepository.GetCurrentCartItems().Count <= 0)
            {
                // Assisted by https://stackoverflow.com/questions/10785245/redirect-to-action-in-another-controller
                return RedirectToAction("Index", "Menu", new { area = "" });
            }

            ViewBag.Title = "Checkout";

            return View();
        }

        [HttpGet("CheckoutPickup")]
        public IActionResult CheckoutPickup()
        {
            // Checkout can only be accessed if the user has items in their cart.
            if (_pizzaRepository.GetCurrentCartItems().Count <= 0)
            {
                // Assisted by https://stackoverflow.com/questions/10785245/redirect-to-action-in-another-controller
                return RedirectToAction("Index", "Menu", new { area = "" });
            }

            ViewBag.Title = "Checkout Pickup";

            UserData currentUserData = _pizzaRepository.GetCurrentUserData();
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();

            if (currentUserData != null)
            {
                checkoutViewModel.FirstName = currentUserData.FirstName;
                checkoutViewModel.LastName = currentUserData.LastName;
                checkoutViewModel.Email = _userIdentityRepository.GetCurrentUser().Email;
            }

            return View(checkoutViewModel);
        }

        [HttpGet("CheckoutDelivery")]
        public IActionResult CheckoutDelivery()
        {
            // Checkout can only be accessed if the user has items in their cart.
            if (_pizzaRepository.GetCurrentCartItems().Count <= 0)
            {
                // Assisted by https://stackoverflow.com/questions/10785245/redirect-to-action-in-another-controller
                return RedirectToAction("Index", "Menu", new { area = "" });
            }

            ViewBag.Title = "Checkout Delivery";

            UserData currentUserData = _pizzaRepository.GetCurrentUserData();
            DeliveryCheckoutViewModel deliveryCheckoutViewModel = new DeliveryCheckoutViewModel();

            if (currentUserData != null)
            {
                deliveryCheckoutViewModel.FirstName = currentUserData.FirstName;
                deliveryCheckoutViewModel.LastName = currentUserData.LastName;
                deliveryCheckoutViewModel.Email = _userIdentityRepository.GetCurrentUser().Email;
                deliveryCheckoutViewModel.PostalCode = currentUserData.PostalCode;
                deliveryCheckoutViewModel.DeliveryArea = currentUserData.DeliveryArea;
                deliveryCheckoutViewModel.DeliveryAddress = currentUserData.DeliveryAddress;
            }

            return View(deliveryCheckoutViewModel);
        }

        [HttpGet("Items")]
        public IActionResult Items()
        {
            var cartItems = _pizzaRepository.GetCurrentCartItems();

            decimal total = 0;

            foreach (var cartItem in cartItems)
            {
                total += cartItem.UnitPrice * cartItem.Quantity;
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems,
                Total = total
            };

            return View("Items", cartViewModel);
        }

        [HttpPost()]
        public IActionResult AddCartItem(MenuItemViewModel menuItemViewModel)
        {
            int portionId = _pizzaRepository.GetPortionIdByLabel(menuItemViewModel.ChosenProductPortion);
            ProductPortion productPortion = _pizzaRepository.GetProductAndPortionById(menuItemViewModel.ChosenProductId, portionId);
            CartItem cartItem = _pizzaRepository.GetCurrentCartItemByPortionIdAndProductId(menuItemViewModel.ChosenProductId, portionId);

            // If the selected product of the selected portion was not added yet to the cart
            if (cartItem == null)
            {
                // Add a new CartItem to the current Order
                _pizzaRepository.AddCurrentCartItemToDatabase(productPortion, menuItemViewModel.ChosenProductQuantity);
            }
            // If the selected product of the selected portion was already added to the cart, then increase its quantity accordingly
            else
            {
                cartItem.Quantity += menuItemViewModel.ChosenProductQuantity;
                _pizzaRepository.Update(cartItem);

                // save changes
                if (!_pizzaRepository.SaveAll())
                {
                    // redirect to an error page
                    return RedirectToAction("Error", "Home", new ErrorViewModel
                    {
                        Message = "Failed to update item in the cart."
                    });
                }
            }


            switch (productPortion.Product.Category)
            {
                case ProductCategory.Pizza:
                    return RedirectToAction("Pizzas", "Menu", new { area = "" });
                case ProductCategory.Burger:
                    return RedirectToAction("Burgers", "Menu", new { area = "" });
                case ProductCategory.Drink:
                    return RedirectToAction("Drinks", "Menu", new { area = "" });
                case ProductCategory.Side:
                    return RedirectToAction("Sides", "Menu", new { area = "" });
                default:
                    return RedirectToAction("Index", "Menu", new { area = "" });
            }
        }

        public IActionResult Delete(int productId, int portionId)
        {
            CartItem cartItem = _pizzaRepository.GetCurrentCartItemByPortionIdAndProductId(productId, portionId);

            if (cartItem == null)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "There is no such item in the cart to remove."
                });
            }

            _pizzaRepository.Remove(cartItem);

            // save changes
            if (!_pizzaRepository.SaveAll())
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to remove item in the cart."
                });
            }

            return RedirectToAction("Items");
        }

        [HttpPost()]
        public IActionResult AddPickupOrder(CheckoutViewModel checkoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to checkout the order."
                });
            }

            _pizzaRepository.AddNewOrder(checkoutViewModel);
            return View("CheckoutSuccess", checkoutViewModel);
        }

        [HttpPost()]
        public IActionResult AddDeliveryOrder(DeliveryCheckoutViewModel deliveryCheckoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to checkout the order."
                });
            }

            _pizzaRepository.AddNewOrder(deliveryCheckoutViewModel);
            return View("CheckoutSuccess", deliveryCheckoutViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
