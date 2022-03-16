using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ILogger<FavoriteController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public FavoriteController(ILogger<FavoriteController> logger,
           UserManager<IdentityUser> userManager,
           IPizzaWebsiteRepository pizzaRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _pizzaRepository = pizzaRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Favorites")]
        public async Task<IActionResult> FavoritesAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var Favorites = _pizzaRepository.GetAllFavoriteItemByUserId(user.Id);
            var products = new List<Product>();
            foreach (var Favorite in Favorites)
            {
                products.Add(_pizzaRepository.GetProductById(Favorite.ProductId));
            }
            FavoriteItemViewModel favoriteItemViewModel = new FavoriteItemViewModel()
            {
                FavoriteItems = Favorites,
                Products = products
            };
            return View("Favorites", favoriteItemViewModel);

        }
        [HttpPost()]
        public async Task<IActionResult> AddFavoritesAsync(MenuItemViewModel menuItemViewModel)
        {
            var id = menuItemViewModel.ChosenProductId;

            var user = await _userManager.GetUserAsync(HttpContext.User);
         
            _pizzaRepository.AddFavoriteItems(id, user.Id);
            var product = _pizzaRepository.GetProductById(id);
              switch (product.Category)
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
              };
           
        }
        
        public IActionResult DeleteFavorite(int productId)
        {
            var favoriteItem = _pizzaRepository.GetFavoriteItemByProductId(productId);

            if (favoriteItem == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "There is no such favoriteItem  to remove."
                });
            }
            _pizzaRepository.RemoveFavorite(favoriteItem);

            if (!_pizzaRepository.SaveAll())
            {

                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to remove favoriteItem."
                });
            }

            return RedirectToAction("Favorites");
        }


    }
}
