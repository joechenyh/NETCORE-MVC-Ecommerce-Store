using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETCORE_CA_8A.DB;
using NETCORE_CA_8A.Models;


// This controller is for the Gallery. 

namespace NETCORE_CA_8A.Controllers
{
    public class ProductController : Controller
    {
        protected StoreDbContext _dbcontext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(StoreDbContext dbcontext, ILogger<ProductController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }
        public IActionResult View2(string newid)
        {
            ViewData["newid"] = newid;
            ViewBag.Product = GetAllProducts(newid);
            ViewBag.Recommendation = GetAllRecommend(newid);
            ViewBag.Review = GetAllReview(newid);


            ViewBag.ItemCount = HttpContext.Session.GetInt32("cartItemCount");
            string uname = HttpContext.Session.GetString("Username");
            ViewBag.Username = uname; uname = HttpContext.Session.GetString("Username");
            ViewBag.Username = uname;

            if (ViewBag.Product.Count == 0)
            {
                ViewBag.search = "not found";
            }
            return View();
        }

        public List<Product> GetAllProducts(string newid)
        {
            if (newid == "")
            {
                return _dbcontext.Products.ToList();
            }

            if (newid == null)
            {
                return _dbcontext.Products.ToList();
            }



            return _dbcontext.Products.Where(p =>
                    p.Id.ToLower() == newid.ToLower()).ToList();
        }

        public List<Product> GetAllRecommend(string newid)
        {
            

            string CategoryId = _dbcontext.Products.Where(p => p.Id == newid).Select(Category => Category.CategoryId).Single();

            List<Product> Recommendations = _dbcontext.Products.Where(c => c.CategoryId == CategoryId && c.Id != newid).ToList();

            return Recommendations;

        }

        public List<Review> GetAllReview(string newid)
        {
            if (newid == "")
            {
                return _dbcontext.Review.ToList();
            }

            if (newid == null)
            {
                return _dbcontext.Review.ToList();
            }



            return _dbcontext.Review.Where(p =>
                    p.ProductId.ToLower() == newid.ToLower()).ToList();
        }

        public IActionResult SubmitReview(string comments, int stars, string newid)
        {

            if (HttpContext.Session.GetString("UserId") == null)
            {

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }
            else
            {
                Review review = new Review();
                review.Id = Guid.NewGuid().ToString();
                review.ProductId = newid;
                review.Comments = comments;
                review.Stars = stars;
                review.CreationTime = DateTime.Now;
                review.CustomerId = HttpContext.Session.GetString("UserId");
                _dbcontext.Review.Add(review);
                _dbcontext.SaveChanges();

                ViewBag.ItemCount = HttpContext.Session.GetInt32("cartItemCount");
                string uname = HttpContext.Session.GetString("Username");
                ViewBag.Username = uname;

                return RedirectToRoute(new { controller = "Product", action = "View2", newid = newid });


            }
        }
    }
}


   
    

   