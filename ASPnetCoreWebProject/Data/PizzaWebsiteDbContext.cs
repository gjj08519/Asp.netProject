using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data.Entities;

namespace PizzaWebsite.Data
{
    public class PizzaWebsiteDbContext : DbContext
    {
        public PizzaWebsiteDbContext(DbContextOptions<PizzaWebsiteDbContext> options) : base(options)
        {
        } 

        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<EmployeeRemovalReason> EmployeeRemovalReasons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPortion> ProductPortions { get; set; }
        public DbSet<Portion> Portions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductPortion>()
                .Property(pp => pp.UnitPrice)
                .HasColumnType("money");

            // set up M-M relationship between Product and Portion
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Portions)
                .WithMany(p => p.Products)
                .UsingEntity<ProductPortion>
                (pp => pp.HasOne<Portion>().WithMany(),
                 pp => pp.HasOne<Product>().WithMany());

            // set up 1-M relationship between Cart and CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems);

            // set up 1-1 relationship between Cart and Order?
            // assisted by https://stackoverflow.com/questions/39505932/entity-framework-1-to-1-relationship
            //modelBuilder.Entity<Order>()
            //.HasKey(o => o.CartId);

            //ALSO NEED TO ACCOUNT FOR THE FACT THAT DELIVERIES WILL MAKE IT SO A USER ID (FOR THE DELIVERER) IS STORED IN ORDER

            // make Portion label unique
            modelBuilder.Entity<Portion>()
                .HasIndex(p => p.Label)
                .IsUnique();
        }
    }
}
