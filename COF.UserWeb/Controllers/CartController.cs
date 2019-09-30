using COF.BusinessLogic.Services;
using COF.UserWeb.Constants;
using COF.UserWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COF.UserWeb.Controllers
{
    public class CartController : Controller
    {
        public readonly IProductService _productService;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("gio-hang")]
        public ActionResult Index()
        {
            return View();
        }

        //[Route("checkout.html", Name = "Checkout")]
        //[HttpGet]
        //public ActionResult Checkout()
        //{
        //    var model = new CheckoutViewModel();
        //    var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
        //    if (session.Any(x => x.Color == null || x.Size == null))
        //    {
        //        return Redirect("/cart.html");
        //    }

        //    model.Carts = session;
        //    return View(model);
        //}
        //[Route("checkout.html", Name = "Checkout")]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> Checkout(CheckoutViewModel model)
        //{
        //    var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

        //    if (ModelState.IsValid)
        //    {
        //        if (session != null)
        //        {
        //            var details = new List<BillDetailViewModel>();
        //            foreach (var item in session)
        //            {
        //                details.Add(new BillDetailViewModel()
        //                {
        //                    Product = item.Product,
        //                    Price = item.Price,
        //                    ColorId = item.Color.Id,
        //                    SizeId = item.Size.Id,
        //                    Quantity = item.Quantity,
        //                    ProductId = item.Product.Id
        //                });
        //            }
        //            var billViewModel = new BillViewModel()
        //            {
        //                CustomerMobile = model.CustomerMobile,
        //                BillStatus = BillStatus.New,
        //                CustomerAddress = model.CustomerAddress,
        //                CustomerName = model.CustomerName,
        //                CustomerMessage = model.CustomerMessage,
        //                BillDetails = details
        //            };
        //            if (User.Identity.IsAuthenticated == true)
        //            {
        //                billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
        //            }
        //            _billService.Create(billViewModel);
        //            try
        //            {

        //                _billService.Save();

        //                //var content = await _viewRenderService.RenderToStringAsync("Cart/_BillMail", billViewModel);
        //                //Send mail
        //                //await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "New bill from Panda Shop", content);
        //                ViewData["Success"] = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                ViewData["Success"] = false;
        //                ModelState.AddModelError("", ex.Message);
        //            }

        //        }
        //    }
        //    model.Carts = session;
        //    return View(model);
        //}
        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCart()
        {
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return Json(new { Status = true, Data = session });
        }

        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return Json(new { Status = true });
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId, int quantity, int size)
        {
            //Get product detail
            var product = await _productService.GetByIdAsync(productId);
            var allProductSizes = product.Sizes;
            var productSize = allProductSizes.FirstOrDefault(x => x.SizeId == size);

            //Get session with item list from cart
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                var item = session.FirstOrDefault(x => x.ProductId == productId && x.Size.SizeId == size);

                //Check exist with item product id
                if (item != null)
                {
                    item.Quantity += quantity;
                    item.Price = productSize.Cost;
                    hasChanged = true;
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product.Name,
                        ProductId = product.Id,
                        ImageUrl = product.Image,
                        Quantity = quantity,

                        Size = new ProductSizeViewModel
                        {
                            SizeId = productSize.SizeId,
                            Size = productSize.Size,
                            ProductSizeId = productSize.Id
                        },
                        Price = productSize.Cost,
                        AllSizes = product.Sizes.Select(x => new ProductSizeViewModel
                        {
                            Size = x.Size,
                            ProductSizeId = x.Id,
                            SizeId = x.SizeId
                        }).ToList()
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    Session[CommonConstants.CartSession] = session;
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product.Name,
                    ProductId = product.Id,
                    ImageUrl = product.Image,
                    Quantity = quantity,
                   
                    Size = new ProductSizeViewModel
                    {
                        SizeId = productSize.SizeId,
                        Size = productSize.Size,
                        ProductSizeId = productSize.Id
                    },
                    Price = productSize.Cost
                });
                Session[CommonConstants.CartSession] = cart;
            }
            return Json(new
            {
                Status = true,
                Data = productId
            });
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(int productId, int size)
        {
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;

            if (session != null)
            {
                var item = session.FirstOrDefault(x => x.ProductId == productId && x.Size.SizeId == size);
                if (item != null)
                {
                    session.Remove(item);
                }

            }
            return Json(new
            {
                Success = true
            });
        }

            /// <summary>
            /// Update product quantity
            /// </summary>
            /// <param name="productId"></param>
            /// <param name="quantity"></param>
            /// <returns></returns>
        public async Task<ActionResult> UpdateCart(int productId, int quantity, int size)
        {
      
            //Get session with item list from cart
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;
           
            if (session != null)
            {
                

                var item = session.FirstOrDefault(x => x.ProductId == productId && x.Size.SizeId == size);

                //Check exist with item product id
                if (item != null)
                {
                    item.Quantity = quantity;
                  
                }
                Session[CommonConstants.CartSession] = session;
                return Json(new
                {
                    Status = true,
                    Data = new
                    {
                        Total = session.Sum(x => x.Quantity * x.Price).ToString("N0"),
                        UnitTotalPrice = (item.Quantity * item.Price).ToString("N0")
                    }
                });
            }
            return new EmptyResult();
        }

        //[HttpGet]
        //public IActionResult GetColors()
        //{
        //    var colors = _billService.GetColors();
        //    return new OkObjectResult(colors);
        //}

        //[HttpGet]
        //public IActionResult GetSizes()
        //{
        //    var sizes = _billService.GetSizes();
        //    return new OkObjectResult(sizes);
        //}
        #endregion

        [ValidateInput(false)]
        public ActionResult HeaderCart()
        {
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;
            session = session != null ? session : new List<ShoppingCartViewModel>();
            var data = RenderPartialViewToString("HeaderCart", session);
            return Json(new { html = data });
        }

        [ValidateInput(false)]
        public ActionResult CartDetail()
        {
            var session = Session[CommonConstants.CartSession] as List<ShoppingCartViewModel>;
            session = session != null ? session : new List<ShoppingCartViewModel>();
            var data = RenderPartialViewToString("CartDetail", session);
            return Json(new { html = data });
        }


        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}