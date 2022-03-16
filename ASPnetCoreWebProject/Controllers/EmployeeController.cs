using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static PizzaWebsite.Data.Seeder.UserIdentityDataSeeder;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Owner, Cook, Manager, Front, Deliverer")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserIdentityRepository _identityRepository;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public EmployeeController(ILogger<EmployeeController> logger,
                                    UserManager<IdentityUser> userManager,
                                    IUserIdentityRepository identityRepository,
                                    IPizzaWebsiteRepository pizzaRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _identityRepository = identityRepository;
            _pizzaRepository = pizzaRepository;
        }

        public IActionResult Index()
        {
            IActionResult result;

            if (User.IsInRole("Owner"))
            {
                result = RedirectToAction("Owner", "Employee");
            }
            else if (User.IsInRole("Manager"))
            {
                result = RedirectToAction("Manager", "Employee");
            }
            else if (User.IsInRole("Front"))
            {
                result = RedirectToAction("Front", "Employee");
            }
            else if (User.IsInRole("Deliverer"))
            {
                result = RedirectToAction("Deliverer", "Employee");
            }
            else if (User.IsInRole("Cook"))
            {
                result = RedirectToAction("Cook", "Employee");
            }
            else
            {
                result = View();
            }

            return result;
        }

        [Authorize(Roles = "Owner")]
        public IActionResult Owner()
        {
            var employeeInfos = _identityRepository.GetAllFullEmployeeInfos();

            var viewModel = new OwnerViewModel
            {
                EmployeeInfos = employeeInfos
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("RegisterEmployee")]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel viewModel)
        {
            // if the view model is not valid
            if (!ModelState.IsValid)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to register the employee."
                });
            }

            IdentityUser user = new IdentityUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber
            };

            UserData userData = new UserData
            {
                CreatedAt = DateTime.Now,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName
            };

            await _identityRepository.AddEmployeeUser(user, userData, viewModel.Role);

            return RedirectToAction("Owner", "Employee");
        }

        public async Task<IActionResult> EditEmployee(string userId, string roleName)
        {
            IdentityUser user = _identityRepository.GetUserById(userId);

            if (user == null)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to update the employee role."
                });
            }

            Roles role = (Roles) Enum.Parse(typeof(Roles), roleName, false);

            await _identityRepository.UpdateUserRole(user, role);

            if (_userManager.GetUserId(User) == userId)
            {
                switch (role)
                {
                    case Roles.Owner:
                        return RedirectToAction("Owner", "Employee");
                    case Roles.Manager:
                        return RedirectToAction("Manager", "Employee");
                    case Roles.Cook:
                        return RedirectToAction("Cook", "Employee");
                    case Roles.Deliverer:
                        return RedirectToAction("Deliverer", "Employee");
                    case Roles.Front:
                        return RedirectToAction("Front", "Employee");
                    default:
                        // redirect to an error page
                        return RedirectToAction("Error", "Home", new ErrorViewModel
                        {
                            Message = "Something went wrong."
                        });
                }
            }

            return RedirectToAction("Owner", "Employee");
        }

        public IActionResult ConfirmRemovingEmployee(string userId, Roles role, string employeeName)
        {
            if (_userManager.GetUserId(User) == userId)
            {
                return RedirectToAction("Owner", "Employee");
            }

            return View("ConfirmRemovingEmployee", 
                new ConfirmRemovingEmployeeViewModel
                {
                    UserId = userId,
                    Role = role,
                    EmployeeName = employeeName
                });
        }

        public async Task<IActionResult> DeleteEmployee(ConfirmRemovingEmployeeViewModel viewModel)
        {
            if (_userManager.GetUserId(User) == viewModel.UserId)
            {
                return RedirectToAction("Owner", "Employee");
            }

            IdentityUser user = _identityRepository.GetUserById(viewModel.UserId);
            UserData userData = _pizzaRepository.GetUserDataByUserId(viewModel.UserId);
            await _identityRepository.RemoveEmployeeUser(user, userData, viewModel.Role);

            var reason = new EmployeeRemovalReason
            {
                EmployeeName = viewModel.EmployeeName,
                Reason = viewModel.Reason
            };

            _pizzaRepository.Add(reason);
            
            if (!_pizzaRepository.SaveAll())
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Something went wrong."
                });
            }

            return RedirectToAction("Owner", "Employee");
        }

        [Authorize(Roles = "Owner, Manager")]
        public IActionResult Manager(string searchString)
        {

            var orders = _pizzaRepository.GetAllOrdersSortByTime();
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.CustomerFirstName.Contains(searchString)).ToList();
            }
            Dictionary<Order, decimal> totalForeachOrder = new Dictionary<Order, decimal>();
            decimal total = 0;
            List<CartItem> cartItems;
       Dictionary<Order, List<CartItem>> orderDetails= new Dictionary<Order, List<CartItem>>();

            foreach (var order in orders)
            {

                cartItems = _pizzaRepository.GetCartItemDetailsByCardId(order.CartId);
                orderDetails.Add(order, cartItems);
                totalForeachOrder.Add(order, _pizzaRepository.GetOrderTotal(order.CartId));
                total += _pizzaRepository.GetOrderTotal(order.CartId);

            }

            ManageOrderViewModel manageOrderViewModel = new ManageOrderViewModel()
            {
                Orders = orders,
              
                OrderDetails = orderDetails,
                TotalForeachOrder = totalForeachOrder,
                Total = total
            };

            return View(manageOrderViewModel);
        }

        [Authorize(Roles = "Owner, Manager")]
        public IActionResult DeleteOrder(int id)
        {
            Order order = _pizzaRepository.GetOrderById(id);

            if (order == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "There is no such order  to remove."
                });
            }
            _pizzaRepository.Remove(order);
           
            if (!_pizzaRepository.SaveAll())
            {
                
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to remove order."
                });
            }

            return RedirectToAction("Manager");
        }

        [Authorize(Roles = "Front, Owner, Manager")]
        public IActionResult Front()
        {
            return View();
        }

        [Authorize(Roles = "Cook, Owner, Manager")]
        public IActionResult Cook()
        {
            ViewBag.Orders = _pizzaRepository.GetAllOrders();
            return View();
        }

        [Authorize(Roles = "Deliverer, Owner, Manager")]
        public IActionResult Deliverer()
        {
            ViewBag.Orders = _pizzaRepository.GetAllOrders();
            return View();
        }
        
        public IActionResult UpdateOrderStatus(int orderId, int cartId, Status newStatus, string redirectPage)
        {

            if(redirectPage == null || redirectPage.Length <= 0)
            {
                _logger.LogWarning("UpdateOrderStatus: the redirect page of is either null or has a length of 0 or less.");
            }

            Order order = _pizzaRepository.GetOrderById(orderId);

            _logger.LogDebug("UpdateOrderStatus: status is being set to " + newStatus + " on id " + orderId + " and updated in the db.");

            Status pastStatus = order.Status;
            order.Status = newStatus;

            switch (newStatus)
            {
                case Status.Preparing:
                    order.TimeAccepted = DateTime.Now;
                    break;
                case Status.Ready:
                    order.TimeCompleted = DateTime.Now;
                    break;
                case Status.Pending:
                    _logger.LogDebug("Status is Ready -> Pending");

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    order.DelivererId = userId;
                    break;
                case Status.Completed:
                    _logger.LogDebug("Status is Pending -> Complete");
                    if (order.DelivererId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    {
                        order.Status = pastStatus;
                        _logger.LogWarning("UpdateOrderStatus: An complete was made by a different user than accepted it.");
                        return RedirectToAction(redirectPage);
                    }

                    break;
            }


            _pizzaRepository.Update(order);

            return RedirectToAction(redirectPage);
        }
    }
}
