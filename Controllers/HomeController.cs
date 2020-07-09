using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCORE_CA_8A.Models;
using NETCORE_CA_8A.Controllers;
using NETCORE_CA_8A.DB;
using Microsoft.AspNetCore.Http;


namespace NETCORE_CA_8A.Controllers
{
    public class HomeController : Controller
    {
        protected StoreDbContext _dbcontext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(StoreDbContext dbcontext,ILogger<HomeController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ItemCount = HttpContext.Session.GetInt32("cartItemCount");
            return View();
        }
        public IActionResult Login(string username, string password)
        {
            string hashPassword = Utils.Crypto.Sha256(password);

            if (CheckAuthentication(username, hashPassword))
            {
                String uid = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = uid;
                
                string uname= HttpContext.Session.GetString("Username");
                ViewBag.Username = uname;

                
                if (HttpContext.Session.GetInt32("cartItemCount") != null)
                {
                    ViewBag.ItemCount = HttpContext.Session.GetInt32("cartItemCount");
                }
                
                return RedirectToRoute(new { controller = "Gallery", action = "Gallery", username = username });
            }
                  
            else
            {
                TempData["loginErrorMessage"] = "Invalid Username and password!";
                return RedirectToAction("Index", "Home");
            }
               
        }

        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult CreateAccount(string username, string password)
        {
            string hashPassword = Utils.Crypto.Sha256(password);
            try
            {
                Customer customer = new Customer(username, hashPassword);
                _dbcontext.Customers.Add(customer);
                _dbcontext.SaveChanges();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            var cust = _dbcontext.Customers.Where(x => x.Name == username)
                     .FirstOrDefault();
           
            HttpContext.Session.SetString("Username", cust.Name);
            HttpContext.Session.SetString("UserId", cust.Id);

            if (HttpContext.Session.GetInt32("cartItemCount") != null)
            {
                ViewBag.ItemCount = HttpContext.Session.GetInt32("cartItemCount");
            }

            return RedirectToRoute(new { controller = "Gallery", action = "Gallery", username = username });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("SessionId");
            
           HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public bool CheckAuthentication(string name, string password)
        {
            var cust = _dbcontext.Customers.Where(x => x.Name == name)
                     .FirstOrDefault();
            if (cust == null || cust.Password != password)
            {
                return false;
            }
            HttpContext.Session.SetString("Username", cust.Name);
            HttpContext.Session.SetString("UserId", cust.Id);
            return true;

        }

    }
}
