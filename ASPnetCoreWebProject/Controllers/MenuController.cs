using Microsoft.AspNetCore.Mvc;
using PizzaWebsite.Data;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;

namespace PizzaWebsite.Controllers
{
    public class MenuController : Controller
    {
        private readonly PizzaWebsiteDbContext _context;
        private readonly IPizzaWebsiteRepository _pizzaWebsiteRepository;


        public MenuController(PizzaWebsiteDbContext context, IPizzaWebsiteRepository pizzaWebsiteRepository)
        {
            _context = context;
            _pizzaWebsiteRepository = pizzaWebsiteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Pizzas")]
        public IActionResult Pizzas()
        {
            ViewBag.Title = "Pizza Menu";

            MenuItemViewModel menuItemViewModel = new MenuItemViewModel
            {
                Products = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Pizza)
            };

            return View(menuItemViewModel);
        }

        [HttpGet("Drinks")]
        public IActionResult Drinks()
        {
            ViewBag.Title = "Drink Menu";

            MenuItemViewModel menuItemViewModel = new MenuItemViewModel
            {
                Products = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Drink)
            };

            return View(menuItemViewModel);
        }

        [HttpGet("Burgers")]
        public IActionResult Burgers()
        {
            ViewBag.Title = "Burger Menu";

            MenuItemViewModel menuItemViewModel = new MenuItemViewModel
            {
                Products = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Burger)
            };

            return View(menuItemViewModel);
        }

        [HttpGet("Sides")]
        public IActionResult Sides()
        {
            ViewBag.Title = "Side Menu";

            MenuItemViewModel menuItemViewModel = new MenuItemViewModel
            {
                Products = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Side)
            };

            return View(menuItemViewModel);
        }
    }
}
