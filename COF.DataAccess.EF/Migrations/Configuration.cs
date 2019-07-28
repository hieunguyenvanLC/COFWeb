namespace COF.DataAccess.EF.Migrations
{
    using COF.DataAccess.EF.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<COF.DataAccess.EF.EFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EFContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            CreateUser(context);
            CreateSizes(context);
            CreateShop(context);
        }

        private void CreateUser(EFContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new EFContext()));
            if (manager.Users.Count() == 0)
            {
                var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new EFContext()));

                var user = new AppUser()
                {
                    UserName = "hoangpn",
                    Email = "hoang.phannhat1996@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Phan Nhật Hoàng",
                    Avatar = "",
                    Gender = true
                };
                if (manager.Users.Count(x => x.UserName == "admin") == 0)
                {
                    manager.Create(user, "123456");

                    if (!roleManager.Roles.Any())
                    {
                        roleManager.Create(new AppRole { Name = "Admin", Description = "Quản trị viên" });
                        roleManager.Create(new AppRole { Name = "Member", Description = "Người dùng" });
                    }

                    var adminUser = manager.FindByName("hoangpn");

                    manager.AddToRoles(adminUser.Id, new string[] { "Admin", "Member" });
                }
            }
        }

        private void CreateShop(EFContext context)
        {
            if (context.Shops.Count() == 0)
            {
                var shops = new List<Shop>
                {
                    new Shop
                    {
                        ShopName = "Moda House Coffee Tô Ký",
                        Address = "263/90 Tô Ký, Trung Mỹ Tây, Quận 12, Hồ Chí Minh",
                        PhoneNumber = "093 834 65 38",
                        //Products = new List<Product>
                        //{
                        //    new Product
                        //    {
                        //        ProductName = 
                        //    }
                        //}
                    },
                    new Shop
                    {
                        ShopName = "Moda House Coffee Nguyễn Oanh",
                        Address = "11 Nguyễn Oanh, P.10, Quận Gò Vấp, Phường 10, Quận Gò Vấp, Gò Vấp, Hồ Chí Minh",
                        PhoneNumber = "093 815 19 69"
                    }
                };
                foreach (var shop in shops)
                {
                    context.Shops.Add(shop);
                }
                context.SaveChanges();
            }

        }

        private void CreateSizes(EFContext context)
        {


            if (context.Sizes.Count() == 0)
            {
                var sizes = new List<Size>
                {
                    new Size
                    {
                         Name = "C"
                    },
                    new Size
                    {
                         Name = "O"
                    },
                     new Size
                    {
                         Name = "F"
                    },
                };
                context.Sizes.AddRange(sizes);
                context.SaveChanges();
            }
        }
    }
}
