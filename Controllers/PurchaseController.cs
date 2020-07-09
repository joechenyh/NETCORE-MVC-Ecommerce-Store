using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCORE_CA_8A.Models;
using NETCORE_CA_8A.DB;
using Microsoft.AspNetCore.Http;


namespace NETCORE_CA_8A.Controllers
{
    public class PurchaseController : Controller
    {


        protected StoreDbContext _dbcontext;

        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(StoreDbContext dbcontext, ILogger<PurchaseController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }


        public IActionResult Purchase(String customerid)
        {
            HttpContext.Session.GetString("CustomerId");
            ViewData["CustomerId"] = customerid;
            ViewBag.Purchases = GetAllPurchases(customerid);
            


            return View();
        }


        
        public List<Purchase> GetAllPurchases(String customerid)
        {
            List<Purchase> purchase;

            if (customerid == null)
            {
                return null;
            }


            purchase = _dbcontext.Purchase
                    .Where(x => x.CustomerId == customerid)
                    .ToList();

            if (purchase == null)

            {
                return null;
            }

            
            return purchase;

        }
    }
}