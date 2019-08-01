﻿// <auto-generated />
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression

namespace COF.DataAccess.EF.Repositories
{
	using COF.DataAccess.EF.Infrastructure;
	using Models = COF.DataAccess.EF.Models;
	
	public partial interface IShopRepository : IRepository<Models.Shop> {}
	
	public partial class ShopRepository : EFRepository<Models.Shop>, IShopRepository { public ShopRepository(EFContext context) : base(context){} }
	
	public partial interface IOrderRepository : IRepository<Models.Order> {}
	
	public partial class OrderRepository : EFRepository<Models.Order>, IOrderRepository { public OrderRepository(EFContext context) : base(context){} }
	
	public partial interface ICategoryRepository : IRepository<Models.Category> {}
	
	public partial class CategoryRepository : EFRepository<Models.Category>, ICategoryRepository { public CategoryRepository(EFContext context) : base(context){} }
	
	public partial interface IPermissionRepository : IRepository<Models.Permission> {}
	
	public partial class PermissionRepository : EFRepository<Models.Permission>, IPermissionRepository { public PermissionRepository(EFContext context) : base(context){} }
	
	public partial interface ITableHasOrderRepository : IRepository<Models.TableHasOrder> {}
	
	public partial class TableHasOrderRepository : EFRepository<Models.TableHasOrder>, ITableHasOrderRepository { public TableHasOrderRepository(EFContext context) : base(context){} }
	
	public partial interface IPartnerRepository : IRepository<Models.Partner> {}
	
	public partial class PartnerRepository : EFRepository<Models.Partner>, IPartnerRepository { public PartnerRepository(EFContext context) : base(context){} }
	
	public partial interface ITableRepository : IRepository<Models.Table> {}
	
	public partial class TableRepository : EFRepository<Models.Table>, ITableRepository { public TableRepository(EFContext context) : base(context){} }
	
	public partial interface IProductRepository : IRepository<Models.Product> {}
	
	public partial class ProductRepository : EFRepository<Models.Product>, IProductRepository { public ProductRepository(EFContext context) : base(context){} }
	
	public partial interface IProductSizeRepository : IRepository<Models.ProductSize> {}
	
	public partial class ProductSizeRepository : EFRepository<Models.ProductSize>, IProductSizeRepository { public ProductSizeRepository(EFContext context) : base(context){} }
	
	public partial interface ISizeRepository : IRepository<Models.Size> {}
	
	public partial class SizeRepository : EFRepository<Models.Size>, ISizeRepository { public SizeRepository(EFContext context) : base(context){} }
	
	public partial interface ICustomerRepository : IRepository<Models.Customer> {}
	
	public partial class CustomerRepository : EFRepository<Models.Customer>, ICustomerRepository { public CustomerRepository(EFContext context) : base(context){} }
}

