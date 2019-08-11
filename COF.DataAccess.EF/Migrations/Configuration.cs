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
            CreatePartner(context);
            CreateSizes(context);
            CreateRoles(context);
            CreateUser(context);
        }

        private void CreateUser(EFContext context)
        {
            var partner = context.Partners.FirstOrDefault();
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new EFContext()));
            if (manager.Users.Count() == 0)
            {
                var roleManager = new RoleManager<AppRole>(new RoleStore<AppRole>(new EFContext()));

                var partnerAdmin = new AppUser()
                {
                    UserName = "CofAdmin",
                    Email = "hoang.phannhat1996@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Phan Nhật Hoàng",
                    Avatar = "",
                    Gender = true,
                    PartnerId = partner.Id,
                    ShopHasUsers = new List<ShopHasUser>
                    {
                        new ShopHasUser
                        {
                            PartnerId = partner.Id,
                            ShopId = partner.Shops.FirstOrDefault().Id,
                        }
                    }
                };

                var cashier = new AppUser()
                {
                    UserName = "CofCashier",
                    Email = "cof@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Thu ngân",
                    Avatar = "",
                    Gender = true,
                    PartnerId = partner.Id,
                    ShopHasUsers = new List<ShopHasUser>
                    {
                        new ShopHasUser
                        {
                            PartnerId = partner.Id,
                            ShopId = partner.Shops.FirstOrDefault().Id,
                        },
                        new ShopHasUser
                        {
                            PartnerId = partner.Id,
                            ShopId = partner.Shops.LastOrDefault().Id,
                        }
                    }
                };


                if (manager.Users.Count(x => x.UserName == partnerAdmin.UserName) == 0)
                {
                    manager.Create(partnerAdmin, "123456");

                    var adminUser = manager.FindByName(partnerAdmin.UserName);

                    manager.AddToRoles(adminUser.Id, new string[] { "PartnerAdmin" });
                }

                if (manager.Users.Count(x => x.UserName == cashier.UserName) == 0)
                {
                    manager.Create(cashier, "123456");

                    var cashierUser = manager.FindByName(cashier.UserName);

                    manager.AddToRoles(cashier.Id, new string[] { "Cashier" });
                }
            }
            

            var shopAdmin = new AppUser()
            {
                UserName = "shopadmin",
                Email = "hoang.phannhat1996@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Nguyễn Văn Hiếu",
                Avatar = "",
                Gender = true,
                PartnerId = partner.Id,
                ShopHasUsers = new List<ShopHasUser>
                    {
                        new ShopHasUser
                        {
                            PartnerId = partner.Id,
                            ShopId = partner.Shops.FirstOrDefault().Id,
                        }
                    }
            };
            manager.Create(shopAdmin, "123456");

            var shopAdminUser = manager.FindByName(shopAdmin.UserName);

            manager.AddToRoles(shopAdminUser.Id, new string[] { "ShopManager" });
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

        private void CreatePartner(EFContext context)
        {
            if (context.Partners.Count() == 0)
            {
                var partner = new Partner
                {
                    Name = "COF",
                    Email = "cof@gmail.com",
                    PhoneNumber = "0946 848 036",
                    ParticipationDate = DateTime.UtcNow,
                    Shops = new List<Shop>
                {
                    new Shop
                    {
                        ShopName = "Moda House Coffee Tô Ký",
                        Address = "263/90 Tô Ký, Trung Mỹ Tây, Quận 12, Hồ Chí Minh",
                        PhoneNumber = "093 834 65 38",
                    },
                    new Shop
                    {
                        ShopName = "Moda House Coffee Nguyễn Oanh",
                        Address = "11 Nguyễn Oanh, P.10, Quận Gò Vấp, Phường 10, Quận Gò Vấp, Gò Vấp, Hồ Chí Minh",
                        PhoneNumber = "093 815 19 69"
                    }
                }
                };


                context.Partners.Add(partner);
                context.SaveChanges();
            }
        }

        private void CreateRoles(EFContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<AppRole>
                {
                    new AppRole { Name = "PartnerAdmin", Description = "Partner Admin" },
                    new AppRole { Name = "ShopManager", Description = "Shop Manager"},
                    new AppRole { Name = "Cashier", Description = "Cashier"},
                    new AppRole { Name = "Staff" , Description = "Staff"}
                };
                roles.ForEach(x => { context.Roles.Add(x); });
                context.SaveChanges();

            }
            
        }

      
    }
}
