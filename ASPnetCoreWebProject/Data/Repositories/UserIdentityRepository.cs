using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static PizzaWebsite.Data.Seeder.UserIdentityDataSeeder;

namespace PizzaWebsite.Data.Repositories
{
    public interface IUserIdentityRepository
    {
        IdentityUser GetUserById(string id);
        IdentityUser GetCurrentUser();
        List<FullUserInfo> GetAllFullEmployeeInfos();
        Task AddEmployeeUser(IdentityUser user, UserData userData, Roles role);
        Task UpdateUserRole(IdentityUser user, Roles newRole);
        Task RemoveEmployeeUser(IdentityUser user, UserData userData, Roles role);
        bool SaveAll();
    }

    public class FullUserInfo
    {
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public IdentityRole Role { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly ILogger<UserIdentityRepository> _logger;
        private readonly UserIdentityDbContext _context;
        private readonly IPizzaWebsiteRepository _pizzaWebsiteRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;

        public UserIdentityRepository(ILogger<UserIdentityRepository> logger,
                                        UserIdentityDbContext context,
                                        IPizzaWebsiteRepository pizzaWebsiteRepository,
                                        UserManager<IdentityUser> userManager,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _pizzaWebsiteRepository = pizzaWebsiteRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IdentityUser GetUserById(string id)
        {
            try
            {
                return _context.Users.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public IdentityUser GetCurrentUser()
        {
            return GetUserById(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task AddEmployeeUser(IdentityUser user, UserData userData, Roles role)
        {
            try
            {
                if (role == Roles.Customer)
                {
                    throw new InvalidOperationException("Cannot create a customer.");
                }

                // create the new user with password
                IdentityResult result = await _userManager.CreateAsync(user, role + "@123");

                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not create user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }

                // add user data
                userData.UserId = user.Id;
                _pizzaWebsiteRepository.Add(userData);

                result = await _userManager.AddToRoleAsync(user, role.ToString());

                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not add the role to the user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }

                if (!_pizzaWebsiteRepository.SaveAll())
                {
                    throw new Exception("Could not create user data.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create employee user: {e}");
                return;
            }
        }

        public async Task UpdateUserRole(IdentityUser user, Roles newRole)
        {
            try
            {
                var user_role = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);

                if (user_role == null)
                {
                    throw new Exception("Could not find the user.");
                }

                var oldRole = _context.Roles.FirstOrDefault(r => r.Id == user_role.RoleId);

                if (oldRole == null)
                {
                    throw new Exception("Could not find the old role for the user.");
                }

                IdentityResult result = await _userManager.RemoveFromRoleAsync(user, oldRole.Name);

                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not remove the old role from the user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }

                result = await _userManager.AddToRoleAsync(user, newRole.ToString());

                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not add the new role to the user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task RemoveEmployeeUser(IdentityUser user, UserData userData, Roles role)
        {
            try
            {
                IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role.ToString());

                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not remove the role from the user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }

                _pizzaWebsiteRepository.Remove(userData);

                if (!_pizzaWebsiteRepository.SaveAll())
                {
                    throw new Exception("Could not remove user data.");
                }

                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public List<FullUserInfo> GetAllFullEmployeeInfos()
        {
            try
            {
                _logger.LogInformation("Getting all employee users ...");

                List<FullUserInfo> employeeUserInfos =
                    _context.Roles
                    .Join(
                        _context.UserRoles,
                        role => role.Id,
                        user_role => user_role.RoleId,
                        (role, user_role) => new
                        {
                            user_role.UserId,
                            Role = role
                        }
                    )
                    .Where(user_role => user_role.Role.Name != Roles.Customer.ToString())
                    .Join(
                        _context.Users,
                        user_role => user_role.UserId,
                        user => user.Id,
                        (user_role, user) => new FullUserInfo
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Role = user_role.Role,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber
                        }
                    ).ToList();

                foreach (FullUserInfo info in employeeUserInfos)
                {
                    var userData = _pizzaWebsiteRepository.GetUserDataByUserId(info.UserId);

                    if (userData == null) continue;

                    info.CreatedAt = userData.CreatedAt;
                    info.FirstName = userData.FirstName;
                    info.LastName = userData.LastName;
                }

                return employeeUserInfos;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all employee users: {e}");

                return null;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
