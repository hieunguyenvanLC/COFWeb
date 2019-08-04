namespace COF.DataAccess.EF
{
    using COF.DataAccess.EF.Infrastructure;
    using COF.DataAccess.EF.Models;
    using EntityFramework.DynamicFilters;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Reflection;

    public class EFContext : IdentityDbContext<AppUser>
    {
        private int _partnerId = 0;


        public EFContext()
            : base("name=COFContext")
        {
            //this.InitializeDynamicFilters();
            //this.EnableFilter("SoftDelete");         
        }

        public EFContext(IPartnerContext partnerContext, String connectionString) : base(connectionString)
        {
            //this.InitializeDynamicFilters();
            //if (partnerContext.PartnerId > 0) SetPartnerId(partnerContext.PartnerId);
            //this.EnableFilter("SoftDelete");

        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TableHasOrder> TableHasOrders { get; set; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().HasKey<int>(i => i.Id).ToTable("UserClaim");
            ConfigureEntities(modelBuilder);
            modelBuilder.Filter("SoftDelete", (BaseEntity d) => d.IsDeleted, false);

        }


        public void SetPartnerId(int partnerId)
        {
            _partnerId = partnerId;
            this.SetFilterScopedParameterValue("Partner", "partnerId", _partnerId);
            this.SetFilterGlobalParameterValue("Partner", "partnerId", _partnerId);
        }

        private void ConfigureEntities(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !string.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }

        public static EFContext Create()
        {
            return new EFContext();
        }

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }


    }
}