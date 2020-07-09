using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NETCORE_CA_8A.DB;
using NETCORE_CA_8A.Models;


namespace NETCORE_CA_8A.Controllers
{
    public class CartController : Controller
    {
        protected StoreDbContext db;
        private readonly ILogger<CartController> _logger;
        int? userId;

        public CartController(StoreDbContext dbcontext, ILogger<CartController> logger)
        {
            db = dbcontext;
            _logger = logger;
        }

        public ActionResult AddtoCart(string productId, string fromProdDetail = "", string searchKeyword = "")
        {
            ViewBag.UserId = userId;
            ViewBag.ItemCount = AddItemToCart(productId, 1);
            string uname = HttpContext.Session.GetString("Username");
            ViewBag.Username = uname;

            HttpContext.Session.SetInt32("cartItemCount", (int)ViewBag.ItemCount);
            if (fromProdDetail == "true")
            {
                return RedirectToRoute(new { controller = "Product", action = "View2", newid = productId, itemCount = ViewBag.ItemCount });
            }
            return RedirectToRoute(new { controller = "Gallery", action = "Gallery", itemCount = ViewBag.ItemCount, keyword = searchKeyword });
        }

        public ActionResult AddItemFromCart(string productId)
        {
            ViewBag.ItemCount = AddItemToCart(productId, 1);
            string uname = HttpContext.Session.GetString("Username");
            ViewBag.Username = uname;
            HttpContext.Session.SetInt32("cartItemCount", (int)ViewBag.ItemCount);
            return RedirectToRoute(new { controller = "Cart", action = "Cart", itemCount = ViewBag.ItemCount });
        }
        public ActionResult RemoveItemFromCart(string productId)
        {
            ViewBag.ItemCount = AddItemToCart(productId, -1);
            HttpContext.Session.SetInt32("cartItemCount", (int)ViewBag.ItemCount);
            return RedirectToRoute(new { controller = "Cart", action = "Cart", itemCount = ViewBag.ItemCount });
        }

        [Route("/Cart")]
        public ActionResult Cart()
        {
            Cart cart;
            Cart cart1;
            string userId;
            string uname;
            string SessionId = HttpContext.Session.GetString("SessionId");

            if (HttpContext.Session.GetString("UserId") != null)
            {
                userId = HttpContext.Session.GetString("UserId");

                cart1 = db.Cart.Where(x => x.CustomerId == userId && x.IsCheckOut == 0).FirstOrDefault();
                if (cart1 == null)
                {
                    cart = db.Cart.Where(x => x.SessionId == SessionId && x.IsCheckOut == 0).FirstOrDefault();

                    if (cart != null)
                    {
                        cart.CustomerId = userId;
                        db.Cart.Update(cart);
                        db.SaveChanges();

                        ViewBag.ItemCount = GetItemCount();
                        ViewBag.CartItems = GetAllCartItems();

                    }
                    else
                    {
                        ViewBag.ItemCount = 0;
                    }
                }
                else
                {
                    ViewBag.ItemCount = GetItemCount();
                    ViewBag.CartItems = GetAllCartItems();
                }
               
                uname = HttpContext.Session.GetString("Username");
                ViewBag.Username = uname;

                return View();

            }
            else
            {
                ViewBag.ItemCount = GetItemCount();
                ViewBag.CartItems = GetAllCartItems();
                uname = HttpContext.Session.GetString("Username");
                ViewBag.Username = uname;

                return View();
            }

        }



        public ActionResult CheckoutCart()
        {
            Cart cart;
            String userId;
            string uname;
            string SessionId = HttpContext.Session.GetString("SessionId");

            if (HttpContext.Session.GetString("UserId") == null)
            {

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }
            else
            {
                userId = HttpContext.Session.GetString("UserId");
                cart = db.Cart.Where(x => x.CustomerId == userId && x.IsCheckOut == 0).First();
                cart.CustomerId = userId;
                db.Cart.Update(cart);


                uname = HttpContext.Session.GetString("Username");
                ViewBag.Username = uname;

                ViewBag.ItemCount = GetItemCount();

                db.SaveChanges();

            }

            using (var dbTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    cart = db.Cart.FirstOrDefault(x => x.CustomerId == userId && x.IsCheckOut == 0);
                    if (cart == null)
                    {
                        throw new Exception("Cart not found");
                    }


                    cart.CartItems = db.CartItem.Where(x => x.CartId == cart.Id).ToList();


                    if (cart.CartItems.Count == 0)
                    {
                        throw new Exception("Cart is empty");
                    }


                    foreach (CartItem cartItem in cart.CartItems)
                    {
                        Product product = db.Products.FirstOrDefault(x => x.Id == cartItem.ProductId);


                        if (product == null)
                        {
                            throw new Exception("Product not exists");
                        }

                        for (int i = 0; i < cartItem.Quantity; i++)
                        {
                            string activationCode = GetActivationCode();
                            Purchased purchased = new Purchased(cartItem.Id, activationCode);
                            db.Purchased.Add(purchased);
                            db.SaveChanges();
                        }

                    }

                    cart.IsCheckOut = 1;
                    cart.CheckoutTime = DateTime.Now;

                    db.SaveChanges();
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            ViewBag.cartItems = GetCartItems(cart.Id);
            ViewBag.ItemCount = 0;
            HttpContext.Session.SetInt32("cartItemCount", 0);
            return View();
        }


        public int AddItemToCart(string productId, int quantity)
        {
            string SessionId = HttpContext.Session.GetString("SessionId");
            string uid;
            Cart cart;


             ViewBag.SessionId = SessionId;

            using (IDbContextTransaction dbTransaction = db.Database.BeginTransaction())
            {
                
                try
                {
                    if (HttpContext.Session.GetString("UserId") != null)
                    {
                        uid = HttpContext.Session.GetString("UserId");
                        cart = db.Cart.FirstOrDefault(x => x.CustomerId == uid && x.IsCheckOut == 0);
                        if (cart == null)
                        {
                            cart = new Cart(SessionId, uid);
                            db.Cart.Add(cart);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        cart = db.Cart.FirstOrDefault(x => x.SessionId == SessionId && x.IsCheckOut == 0);
                        if (cart == null)
                        {
                            cart = new Cart(SessionId);
                            db.Cart.Add(cart);
                            db.SaveChanges();
                        }
                    }

                   
                    Product product = db.Products
                             .Where(x => x.Id == productId)
                             .FirstOrDefault();

                    if (product == null)
                    {
                        throw new Exception("Product does not exist");
                    }

                    CartItem cartItem = db.CartItem.FirstOrDefault(x => x.CartId == cart.Id && x.ProductId == productId);

                    if (cartItem == null)
                    {
                        cartItem = new CartItem(cart.Id, productId);
                        db.CartItem.Add(cartItem);
                        db.SaveChanges();
                    }
                    else
                    {
                        cartItem.Quantity += quantity;
                    }

                    if (cartItem.Quantity == 0)
                    {
                        db.CartItem.Remove(cartItem);
                        db.SaveChanges();
                    }

                    cart.Quantity += quantity;

                    db.SaveChanges();

                    dbTransaction.Commit();
                    HttpContext.Session.SetInt32("cartItemCount", cart.Quantity);
                    ViewBag.ItemCount = cart.Quantity;

                    return cart.Quantity;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    throw new Exception(e.Message);
                }


            }
        }


        public List<CartItem> GetAllCartItems()
        {
            string SessionId = HttpContext.Session.GetString("SessionId");
            String uid;
            var cartId= 0;

            if (HttpContext.Session.GetString("UserId") != null)
            {
                uid = HttpContext.Session.GetString("UserId");
                cartId = db.Cart.Where(cart => cart.CustomerId == uid && cart.IsCheckOut == 0).Select(cart => cart.Id).FirstOrDefault();
            }
            else
            {
                cartId = db.Cart.Where(cart => cart.SessionId == SessionId && cart.IsCheckOut == 0).Select(cart => cart.Id).FirstOrDefault();
            }

            ViewBag.SessionId = SessionId;
            return GetCartItems(cartId);
        }

        public ActionResult GetPurchasedHistory()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }

            ViewBag.ItemCount = GetItemCount();
            ViewBag.cartItems = GetPurchasedItems();
            string uname = HttpContext.Session.GetString("Username");
            ViewBag.Username = uname;
            return View();
        }

        public int GetItemCount()
        {
            string SessionId = HttpContext.Session.GetString("SessionId");
            string uid;

            Cart cart;

            ViewBag.SessionId = SessionId;

            if (HttpContext.Session.GetString("UserId") != null)
            {
                uid = HttpContext.Session.GetString("UserId");
                cart = db.Cart.FirstOrDefault(x => x.CustomerId == uid && x.IsCheckOut == 0);

                if (cart == null)
                {
                    cart = new Cart(SessionId,uid);
                    db.Cart.Add(cart);
                    db.SaveChanges();
                    return 0;
                }
            }
            else
            {
               cart = db.Cart.FirstOrDefault(x => x.SessionId == SessionId && x.IsCheckOut == 0);
                if (cart == null)
                {
                    cart = new Cart(SessionId);
                    db.Cart.Add(cart);
                    db.SaveChanges();
                    return 0;
                }
            }
         
            return cart.Quantity;
        }

        public List<CartItem> GetCartItems(int cartId)
        {
            var query = db.CartItem.Where(cartItem => cartItem.CartId == cartId)
                    .Join(db.Products, item => item.ProductId, product => product.Id,
                        (item, product) => new
                        {
                            Id = item.Id,
                            CartId = item.CartId,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            Product = product,
                            CheckoutTime = DateTime.Now,
                            ActivationCodes = db.Purchased.Where(p => p.CartItemId == item.Id).Select(p => p.ActivationCode).ToList()
                        }).ToList();

            return query.Select(x => new CartItem
            {
                Id = x.Id,
                CartId = x.CartId,
                ProductId = (x.Id).ToString(),
                Quantity = x.Quantity,
                Product = x.Product,
                CheckoutTime = x.CheckoutTime,
                ActivationCodes = x.ActivationCodes
            }).ToList();

        }

        private static string GetActivationCode()
        {

            return Guid.NewGuid().ToString();

        }

        public List<CartItem> GetPurchasedItems()
        {
            string SessionId = HttpContext.Session.GetString("SessionId");

            ViewBag.SessionId = SessionId;
            String uid = HttpContext.Session.GetString("UserId");

            List<Cart> purchaseCarts = db.Cart.Where(cart => cart.CustomerId == uid && cart.IsCheckOut == 1).OrderBy(cart => cart.CheckoutTime).ToList();

            List<CartItem> cartItems = new List<CartItem>();
            foreach (Cart cart in purchaseCarts)
            {
                cartItems.AddRange(GetCartItems(cart.Id));
            }

            return cartItems;
        }

    }
}