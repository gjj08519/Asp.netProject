using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using PizzaWebsite.Models;

namespace PizzaWebsite.Data.Repositories
{
    public interface IPizzaWebsiteRepository
    {
        #region Cart
        /// <summary>
        /// Gets the current <see cref="Cart"/>, which is determined by the user's session and account.
        /// </summary>
        /// <param name="getCartItems">If true, fill the current <see cref="Cart"/> with its items, otherwise leave the list of <see cref="Cart.CartItems"/> as null.</param>
        /// <returns>The current <see cref="Cart"/>, which is determined by the user's session and account.</returns>
        Cart GetCurrentCart(Boolean getCartItems = true);

        /// <summary>
        /// Gets the <see cref="List{CartItem}"/> stored in the current <see cref="Cart"/> object's <see cref="Cart.CartItems"/>.
        /// </summary>
        /// <returns>The <see cref="List{CartItem}"/> stored in the current <see cref="Cart"/>'s <see cref="Cart.CartItems"/>.</returns>
        public List<CartItem> GetCurrentCartItems();

        /// <summary>
        /// Adds a new <see cref="CartItem"/> to the database as one of the current <see cref="Cart"/> object's <see cref="Cart.CartItems"/>.
        /// </summary>
        /// <param name="productPortion">
        /// Holds the <see cref="Product.Id"/> and <see cref="Portion.Id"/> of the new <see cref="CartItem"/>'s <see cref="CartItem.Product"/> and <see cref="CartItem.Portion"/>, respectively.
        /// </param>
        /// <param name="quantity">The new <see cref="CartItem"/>'s <see cref="CartItem.Quantity"/>.</param>
        public void AddCurrentCartItemToDatabase(ProductPortion productPortion, int quantity);

        /// <summary>
        /// Gets the <see cref="CartItem"/> with a corresponding <see cref="CartItem.ProductId"/> and <see cref="CartItem.PortionId"/>
        /// in the user's current <see cref="Cart"/> if it exists, or <see cref="null"/> otherwise.
        /// </summary>
        /// <param name="productId">Id of the <see cref="Product"/>.</param>
        /// <param name="portionId">Id of the <see cref="Portion"/>.</param>
        /// <returns>
        /// The <see cref="CartItem"/> with a corresponding <see cref="CartItem.ProductId"/> and <see cref="CartItem.PortionId"/> 
        /// in the user's current <see cref="Cart"/> if it exists, or <see cref="null"/> otherwise.
        /// </returns>
        public CartItem GetCurrentCartItemByPortionIdAndProductId(int productId, int portionId);
        #endregion

        #region Order

        public void AddNewOrder(CheckoutViewModel checkoutViewModel);
        public void Update(Order order);
        public List<Order> GetAllOrders();
        public List<Order> GetAllOrdersSortByTime();
        public void Remove(Order order);
        public decimal GetOrderTotal(int id);
        Order GetOrderById(int orderId);
        List<Order> GetAllOrdersbyUserId(string id);

        #endregion

        #region User Data
        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of all <see cref="UserData"/> from the database.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> of all <see cref="UserData"/> from the database.</returns>
        List<UserData> GetAllUserDatas();
        void Add(UserData userData);
        /// <summary>
        /// Retrieves a <see cref="UserData"/> with the given user id from the database.
        /// </summary>
        /// <param name="userId">Id of the <see cref="IdentityUser"/>.</param>
        /// <returns>The <see cref="UserData"/> with the given user id from the database if it exists, null otherwise.</returns>
        UserData GetUserDataByUserId(string userId);

        /// <summary>
        /// Retrieves a <see cref="UserData"/> relating to the current user from the database.
        /// </summary>
        /// <returns>The <see cref="UserData"/> relating to the current user from the database if it exists, null otherwise.</returns>
        public UserData GetCurrentUserData();

        /// <summary>
        /// Updates the given <see cref="UserData"/> in the database.
        /// </summary>
        /// <param name="userData">The <see cref="UserData"/> to update.</param>
        void Update(UserData userData);

        void Remove(UserData userData);
        #endregion

        #region Product
        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of all <see cref="Product"/> from the database.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> of all <see cref="Product"/> from the database.</returns>
        List<Product> GetAllProducts();

        /// <summary>
        /// Retieves a <see cref="Product"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="Product"/>.</param>
        /// <returns>The <see cref="Product"/> with the given id from the database if it exists, null otherwise.</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of <see cref="Product"/> with the given <see cref="ProductCategory"/> from the database.
        /// </summary>
        /// <param name="productCategory">The category of the <see cref="Product"/>.</param>
        /// <returns>The <see cref="List{T}"/> of <see cref="Product"/> with the given <see cref="ProductCategory"/> from the database.</returns>
        List<Product> GetProductsByCategory(ProductCategory productCategory);
        #endregion

        #region Portion
        /// <summary>
        /// Retrieves a <see cref="Portion"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="Portion"/> with the given id from the database if it exists, null otherwise.</returns>
        Portion GetPortionById(int id);

        /// <summary>
        /// Retrieves a <see cref="Portion"/> with the given label from the database.
        /// </summary>
        /// <param name="portionLabel">The label of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="Portion"/> with the given label from the database if it exists, null otherwise.</returns>
        int GetPortionIdByLabel(string portionLabel);
        #endregion

        #region Product & Portion
        /// <summary>
        /// Retrieves a <see cref="ProductPortion"/> with the given product id and portion id from the database.
        /// </summary>
        /// <param name="productId">Id of the <see cref="Product"/>.</param>
        /// <param name="portionId">Id of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="ProductPortion"/> with the given product id and portion id from the database if it exists, null otherwise.</returns>
        ProductPortion GetProductAndPortionById(int productId, int portionId);
        #endregion

        #region Cart Item
        /// <summary>
        /// Retrieves a <see cref="CartItem"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="CartItem"/>.</param>
        /// <param name="attachNavigation">Whether to attach <see href="https://docs.microsoft.com/en-us/ef/ef6/fundamentals/relationships">navigation properties</see> to the <see cref="CartItem"/>.</param>
        /// <returns>The <see cref="CartItem"/> with the given id from the database if it exists, null otherwise.</returns>
        CartItem GetCartItemById(int id, bool attachNavigation = true);
     
        List<CartItem> GetCartItemDetailsByCardId(int cartId);
        /// <summary>
        /// Adds the given <see cref="CartItem"/> to the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to add.</param>
        void Add(CartItem cartItem);

        /// <summary>
        /// Updates the given <see cref="CartItem"/> in the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to update.</param>
        void Update(CartItem cartItem);

        /// <summary>
        /// Removes the given <see cref="CartItem"/> from the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to remove.</param>
        void Remove(CartItem cartItem);
        #endregion
        #region FavoriteItem
        List<FavoriteItem> GetAllFavoriteItemByUserId(string id);
        void AddFavoriteItems(int productId, string id);
        FavoriteItem GetFavoriteItemById(int id);
        FavoriteItem GetFavoriteItemByProductId(int productId);
        void RemoveFavorite(FavoriteItem favoriteItem);
        #endregion

        void Add(EmployeeRemovalReason employeeRemovalReason);

        /// <summary>
        /// Saves all changes made by the previous CRUD operations before the call of this method.
        /// </summary>
        /// <returns>True if the saving succeeds, false otherwise.</returns>
        bool SaveAll();
    }

    public class PizzaWebsiteRepository : IPizzaWebsiteRepository
    {
        public const string SESSION_KEY_CART_ID = "_CartId";

        private IHttpContextAccessor _httpContextAccessor;
        private readonly PizzaWebsiteDbContext _context;
        private readonly ILogger<PizzaWebsiteRepository> _logger;

        public PizzaWebsiteRepository(ILogger<PizzaWebsiteRepository> logger, PizzaWebsiteDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Cart
        public Cart GetCurrentCart(Boolean getCartItems = true)
        {
            // Get the user's id (null if a guest called this method)
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart currentCart = null;

            // If the user is signed in
            if (currentUserId != null)
            {
                // Get their account's current cart, if it exists
                currentCart = GetCartInUseByUserId(currentUserId);
            }

            // If the user still lacks a cart and a cart id is remembered
            if (currentCart == null && _httpContextAccessor.HttpContext.Session.GetInt32(SESSION_KEY_CART_ID) != null)
            {
                // Get the cart with the matching Id
                Int32 cartId = (Int32)_httpContextAccessor.HttpContext.Session.GetInt32(SESSION_KEY_CART_ID);
                currentCart = GetCartInUseById(cartId);

                // If the cart's userId does not match the User's own Id (and an actual cart was claimed)
                if (currentCart != null && currentCart.UserId != currentUserId)
                {
                    // If a signed in user is trying to access a guest's cart
                    if (currentCart.UserId == null && currentUserId != null)
                    {
                        // The user likely just signed in after filling their cart, so hand it to them
                        // If this system is exploited to steal a guest's cart, the hacker should only have access to someone's glorified shopping list
                        currentCart.UserId = currentUserId;
                        _context.Carts.Update(currentCart);

                        // Save any changes made to the database
                        _context.SaveChanges();
                    }
                    // If any other outcome occurred
                    else
                    {
                        // Then it was an invalid match, so revoke the currentCart
                        currentCart = null;
                    }
                }
            }

            // If the user still has no cart, then make a new one
            if (currentCart == null)
            {
                currentCart = AddNewCartToDatabase(currentUserId);
            }

            // Get the Cart's CartItems if requested
            if (getCartItems)
            {
                currentCart.CartItems = _context.CartItems.Where(ci => ci.CartId == currentCart.Id).ToList();
                foreach (CartItem cartItem in currentCart.CartItems)
                {
                    ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);
                    cartItem.Product = productPortion.Product;
                    cartItem.Portion = productPortion.Portion;
                    cartItem.UnitPrice = productPortion.UnitPrice;
                    cartItem.Cart = currentCart;
                }
            }

            // Save the Cart's Id in the session
            _httpContextAccessor.HttpContext.Session.SetInt32(SESSION_KEY_CART_ID, currentCart.Id);

            return currentCart;
        }

        private Cart GetCartInUseByUserId(string userId)
        {
            return _context.Carts.FirstOrDefault(c => c.UserId == userId && !c.CheckedOut);
        }

        private Cart GetCartInUseById(int cartId)
        {
            return _context.Carts.FirstOrDefault(c => c.Id == cartId && !c.CheckedOut);
        }

        private Cart AddNewCartToDatabase(string currentUserId)
        {
            // Create a new Cart
            Cart newCurrentCart = new Cart()
            {
                CheckedOut = false,
                UserId = currentUserId
            };

            // Add the cart to the database
            _context.Carts.Add(newCurrentCart);

            // Save any changes made to the database
            _context.SaveChanges();

            return newCurrentCart;
        }
        
        public List<CartItem> GetCurrentCartItems()
        {
            return GetCurrentCart().CartItems;
        }

        public void AddCurrentCartItemToDatabase(ProductPortion productPortion, int quantity)
        {
            CartItem currentCartItem = new CartItem
            {
                ProductId = productPortion.ProductId,
                PortionId = productPortion.PortionId,
                Quantity = quantity,
                CartId = GetCurrentCart().Id
            };

            _context.CartItems.Add(currentCartItem);
            _context.SaveChanges();
        }

        public CartItem GetCurrentCartItemByPortionIdAndProductId(int productId, int portionId)
        {
            _logger.LogInformation($"Getting cart item in the current cart ...");

            return GetCurrentCart().CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.PortionId == portionId);
        }
        #endregion

        #region Order
        public void AddNewOrder(CheckoutViewModel checkoutViewModel)
        {
            _logger.LogInformation($"Checking out the user's Cart to make a new Order.");
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Get the user's cart and mark it as checked out so that it can no longer be accessed
            Cart orderCart = GetCurrentCart(false);
            orderCart.CheckedOut = true;

            // Create a new order with all relevant information

            Order order = new Order()
            {
                UserId = currentUserId,
                CartId = orderCart.Id,
                Status = Status.Ordered,
                OrderTime = DateTime.Now,
                CustomerFirstName = checkoutViewModel.FirstName,
                CustomerLastName = checkoutViewModel.LastName,
                CustomerEmail = checkoutViewModel.Email,
                ReceptionMethod = ReceptionMethod.Pickup
            };

            if (checkoutViewModel is DeliveryCheckoutViewModel)
            {
                DeliveryCheckoutViewModel deliveryCheckoutViewModel = checkoutViewModel as DeliveryCheckoutViewModel;
                order.PostalCode = deliveryCheckoutViewModel.PostalCode;
                order.DeliveryArea = deliveryCheckoutViewModel.DeliveryArea;
                order.DeliveryAddress = deliveryCheckoutViewModel.DeliveryAddress;
                order.ReceptionMethod = ReceptionMethod.Delivery;
            }

            // Add the order to the database and update the cart
            _context.Carts.Update(orderCart);
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public Order GetOrderById(int orderId)
        {

            try
            {
                _logger.LogInformation($"Getting order by id {orderId} ...");
                Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                order.Cart = GetCartById(order.CartId);
                _logger.LogInformation($"It's card id is {order.Cart.Id} ...");

                return order;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get order by id {orderId}: {e}");

                return null;
            }

        }

        public void Update(Order order)
        {
            try
            {
                _logger.LogInformation("Updating cart item ...");

                // Set all related objects to null to avoid EF jank
                order.Cart = null;

                _logger.LogInformation(order.Id.ToString());
                _logger.LogInformation(order.Status.ToString());


                this._context.Orders.Update(order);
                _logger.LogInformation("saving changes");

                this._context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update order: {e}");
            }
        }
        public void Remove(Order order)
        {
            try
            {
                _logger.LogInformation("Deleting cart item ...");

                // Set all related objects to null to avoid EF jank
                order.Cart = null;

                _context.Orders.Remove(order);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete cart item: {e}");
            }
        }

        public List<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("Getting all orders...");



                List<Order> orders = _context.Orders.ToList();

                foreach (Order order in orders)
                {
                    if (order.Cart == null)
                    {
                        order.Cart = GetCartById(order.CartId);

                    }
                }

                return orders;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders: {e}");

                return null;
            }
        }
        #endregion

        #region User Data
        public List<UserData> GetAllUserDatas()
        {
            try
            {
                _logger.LogInformation("Getting all user datas ...");

                return _context.UserDatas.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all user datas: {e}");

                return null;
            }
        }

        public UserData GetUserDataByUserId(string userId)
        {
            try
            {
                _logger.LogInformation($"Getting user data by user id {userId} ...");

                return _context.UserDatas.FirstOrDefault(ud => ud.UserId == userId);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get user data by user id {userId}: {e}");

                return null;
            }
        }

        public UserData GetCurrentUserData()
        {
            return GetUserDataByUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public void Update(UserData userData)
        {
            try
            {
                _logger.LogInformation($"Updating user data with id {userData.Id} ...");

                _context.Update(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update user data with id {userData.Id}: {e}");
            }
        }
        public void Add(UserData userData)
        {
            try
            {
                _logger.LogInformation("Adding userData ...");

                _context.UserDatas.Add(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add userData: {e}");
            }
        }
        public void Remove(UserData userData)
        {
            try
            {
                _logger.LogInformation("Removing userData ...");

                _context.UserDatas.Remove(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to remove userData: {e}");
            }
        }
        #endregion

        public void Add(EmployeeRemovalReason employeeRemovalReason)
        {
            try
            {
                _logger.LogInformation("Adding employee removal reason ...");

                _context.EmployeeRemovalReasons.Add(employeeRemovalReason);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add employee removal reason: {e}");
            }
        }

        #region Product
        public List<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products ...");

                List<Product> products = _context.Products
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all products: {e}");
                return null;
            }
        }

        public Product GetProductById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product by id {id} ...");

                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                FillProductListFields(product);
                return product;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get product by id {id}: {e}");

                return null;
            }
        }

        public List<Product> GetProductsByCategory(ProductCategory productCategory)
        {
            try
            {
                _logger.LogInformation($"Getting products by category {productCategory} ...");

                List<Product> products = _context.Products
                    .Where(p => p.Category == productCategory)
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get products by category {productCategory}: {e}");
                return null;
            }
        }

        private void FillProductListFields(Product product)
        {
            List<ProductPortion> productPortions = _context.ProductPortions
                   .Where(pp => pp.ProductId == product.Id)
                   .OrderBy(pp => pp.Id)
                   .ToList();

            foreach (ProductPortion productPortion in productPortions)
            {
                Portion portion = _context.Portions
                    .Where(p => p.Id == productPortion.PortionId)
                    .First();

                product.Portions.Add(portion);
                product.Prices.Add(productPortion.UnitPrice);
            }
            SortPortionsAndPrices(product);
        }

        private void FillProductListFields(List<Product> products)
        {
            foreach (Product product in products)
            {
                FillProductListFields(product);
            }
        }

        private void SortPortionsAndPrices(Product product)
        {
            // Bubble sort assisted by http://anh.cs.luc.edu/170/notes/CSharpHtml/sorting.html
            int i;
            for (int j = product.Prices.Count - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (product.Prices[i] > product.Prices[i + 1])
                        SwapPortionAndPriceOfProduct(product, i, i + 1);
                }
            }
        }

        private void SwapPortionAndPriceOfProduct(Product product, int firstIndex, int secondIndex)
        {
            decimal swappedPrice = product.Prices[firstIndex];
            Portion swappedPortion = product.Portions[firstIndex];

            product.Prices[firstIndex] = product.Prices[secondIndex];
            product.Portions[firstIndex] = product.Portions[secondIndex];

            product.Prices[secondIndex] = swappedPrice;
            product.Portions[secondIndex] = swappedPortion;
        }
        #endregion

        #region Portion
        public Portion GetPortionById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting portion by id {id} ...");

                var portion = _context.Portions.FirstOrDefault(p => p.Id == id);

                return portion;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get portion by user id {id}: {e}");

                return null;
            }
        }

        public int GetPortionIdByLabel(string portionLabel)
        {
            // TODO: Make portion label unique in the database
            try
            {
                _logger.LogInformation($"Getting portion id by name {portionLabel} ...");

                var portion = _context.Portions.FirstOrDefault(p => p.Label == portionLabel);

                if (portion == null)
                    return -1;

                return portion.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get portion id by name {portionLabel}: {e}");

                return -1;
            }
        }
        #endregion

        #region Product & Portion
        public ProductPortion GetProductAndPortionById(int productId, int portionId)
        {
            try
            {
                _logger.LogInformation($"Getting product by id {productId} and portion id {portionId} ...");

                var productPortion = _context.ProductPortions.FirstOrDefault(pp => pp.ProductId == productId && pp.PortionId == portionId);

                if (productPortion == null) return null;

                productPortion.Product = GetProductById(productId);
                productPortion.Portion = GetPortionById(portionId);

                return productPortion;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get product by id {productId} and portion id {portionId}: {e}");

                return null;
            }
        }
        #endregion

        #region Cart Item
        public CartItem GetCartItemById(int id, bool attachReferences = true)
        {
            try
            {
                _logger.LogInformation($"Getting cart item by id {id} ...");

                var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == id);

                if (cartItem == null) return null;

                ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);

                if (attachReferences)
                {
                    // attach product obj on each corresponding cart item
                    cartItem.Product = productPortion.Product;
                    cartItem.Portion = productPortion.Portion;
                }

                cartItem.UnitPrice = productPortion.UnitPrice;

                return cartItem;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get cart item by id {id}: {e}");

                return null;
            }
        }
        
        public List<CartItem> GetCartItemDetailsByCardId(int cartId)
        {
            try
            {
                _logger.LogInformation($"Getting cart items by cardId {cartId} ...");

                List<CartItem> cartItems = _context.CartItems.Where(ci => ci.CartId == cartId).ToList();

                foreach (CartItem cartItem in cartItems)
                {
                    ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);
                    cartItem.Product = productPortion.Product;
                    cartItem.Portion = productPortion.Portion;
                    cartItem.UnitPrice = productPortion.UnitPrice;
                 }
                
                return cartItems;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get cart items cardId {cartId}: {e}");

                return null;
            }
        }
        public void Add(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Adding cart item ...");

                _context.CartItems.Add(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add cart item: {e}");
            }
        }

        public void Update(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Updating cart item ...");

                // Set all related objects to null to avoid EF jank
                cartItem.Cart = null;
                cartItem.Portion = null;
                cartItem.Product = null;

                _context.CartItems.Update(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update cart item: {e}");
            }
        }

        public void Remove(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Deleting cart item ...");

                // Set all related objects to null to avoid EF jank
                cartItem.Cart = null;
                cartItem.Portion = null;
                cartItem.Product = null;

                _context.CartItems.Remove(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete cart item: {e}");
            }
        }
        #endregion
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public List<Order> GetAllOrdersSortByTime()
        {
            try
            {
                _logger.LogInformation("Getting all orders ...");

                List<Order> orders = _context.Orders
                    .OrderBy(O => O.OrderTime)
                    .ToList();

                return orders;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders: {e}");
                return null;
            }
        }

        public decimal GetOrderTotal(int id)
        {
            decimal total = 0;
            var cart = GetCartById(id);
          
            foreach (var cartItem in cart.CartItems)
            {
                total += cartItem.UnitPrice * cartItem.Quantity;
            }
            return total;
        }

        private Cart GetCartById(int id)
        {

            Cart currentCart = _context.Carts.FirstOrDefault(c => c.Id == id);

            if (currentCart != null)
            {
                currentCart.CartItems = _context.CartItems.Where(ci => ci.CartId == currentCart.Id).ToList();
                foreach (CartItem cartItem in currentCart.CartItems)
                {
                    ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);
                    cartItem.Product = productPortion.Product;
                    cartItem.Portion = productPortion.Portion;
                    cartItem.UnitPrice = productPortion.UnitPrice;
                    cartItem.Cart = currentCart;
                }
            }

            return currentCart;
        }

        public List<FavoriteItem> GetAllFavoriteItemByUserId(string id)
        {
            try
            {
                _logger.LogInformation("GetAllFavoriteItemByUserId was called...");
                return _context.FavoriteItems
                    .Where(f => f.UserId==id)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all favorite items :{e}");
                return null;
            }
        }

        

        public void AddFavoriteItems(int productId, string id)
        {
            try
            {
                _logger.LogInformation("GetFavoriteProduct was called...");
              
                FavoriteItem item = new FavoriteItem()
                {
                   
                    ProductId = productId,
                    UserId = id

                };
                _context.FavoriteItems.Add(item);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add  favorite items :{e}");


            }
        }

        public FavoriteItem GetFavoriteItemById(int id)
        {
            return _context.FavoriteItems.FirstOrDefault(f => f.Id == id);
        }

        public void RemoveFavorite(FavoriteItem favoriteItem)
        {
            try
            {
                _logger.LogInformation("Deleting favoriteItem ...");

                _context.FavoriteItems.Remove(favoriteItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete favoriteItem: {e}");
            }
        }

        public FavoriteItem GetFavoriteItemByProductId(int productId)
        {
            return _context.FavoriteItems.FirstOrDefault(f => f.ProductId == productId);
        }

        public List<Order> GetAllOrdersbyUserId(string id)
        {
            try
            {
                _logger.LogInformation("GetAllOrdersbyUserId was called...");
                return _context.Orders
                    .Where(o => o.UserId == id)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all favorite items :{e}");
                return null;
            }
        }
    }
}
