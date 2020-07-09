using Microsoft.VisualBasic.CompilerServices;
using NETCORE_CA_8A.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.DB
{
    public class DBSeeder
    {
        public DBSeeder(StoreDbContext dbcontext)
        {
            Customer cust1 = new Customer();
            cust1.Id = Guid.NewGuid().ToString();
            cust1.Name = "amanda";
            cust1.Password = Utils.Crypto.Sha256(cust1.Name);
            dbcontext.Add(cust1);

            Customer cust2 = new Customer();
            cust2.Id = "2";
            cust2.Id = Guid.NewGuid().ToString();
            cust2.Name = "joe";
            cust2.Password = Utils.Crypto.Sha256(cust2.Name);
            dbcontext.Add(cust2);

            Customer cust3 = new Customer();
            cust3.Id = Guid.NewGuid().ToString();
            cust3.Name = "esther";
            cust3.Password = Utils.Crypto.Sha256(cust3.Name);
            dbcontext.Add(cust3);

            Category cat1 = new Category();
            cat1.Id = Guid.NewGuid().ToString();
            cat1.catName = ".NET";
            dbcontext.Add(cat1);

            Category cat2 = new Category();
            cat2.Id = Guid.NewGuid().ToString();
            cat2.catName = "MS";
            dbcontext.Add(cat2);

            
           Product product1 = new Product();
           product1.Id = Guid.NewGuid().ToString();
           product1.productName = ".NetCharts";
           product1.description = "Brings powerful charting capabilities to your .Net applications";
           product1.unitPrice = (double)99.00;
           product1.CategoryId = cat1.Id;
           product1.Image = "https://localhost:44331/lib/images/Charts.JPG";
           product1.URL = "https://localhost:44331/Product/View/product1";
           dbcontext.Add(product1);

           Product product2 = new Product();
           product2.Id = Guid.NewGuid().ToString();
           product2.productName = ".NetPaypal";
           product2.description = "Integrate your .Net apps with Paypal the easy way";
           product2.unitPrice = (double)69.00;
           product2.CategoryId = cat1.Id;
           product2.Image = "https://localhost:44331/lib/images/Paypal.JPG";
           product2.URL = "https://localhost:44331/Product/View/product2";
           dbcontext.Add(product2);

            Product product3 = new Product();
             product3.Id = Guid.NewGuid().ToString();
             product3.productName = ".NetML";
             product3.description = "Supercharges .NET machine learning libraries";
             product3.unitPrice = 299.00;
             product3.CategoryId = cat1.Id;
             product3.Image = "https://localhost:44331/lib/images/ML.JPG";
             product3.URL = "https://localhost:44331/Product/View/product3";
            dbcontext.Add(product3);

             Product product4 = new Product();
             product4.Id = Guid.NewGuid().ToString();
             product4.productName = ".NetAnalytics";
             product4.description = "Perform data mining and analytics easily in .NET";
             product4.unitPrice = 299.00;
             product4.CategoryId = cat1.Id;
             product4.Image = "https://localhost:44331/lib/images/Analytics.JPG";
             product4.URL = "https://localhost:44331/Product/View/product4";
             dbcontext.Add(product4);

             Product product5 = new Product();
             product5.Id = Guid.NewGuid().ToString();
             product5.productName = ".NetLogger";
             product5.description = "Logs and aggregates events easily in your .NET apps";
             product5.unitPrice = 49.00;
             product5.CategoryId = cat1.Id;
             product5.Image = "https://localhost:44331/lib/images/Logger.JPG";
             product5.URL = "https://localhost:44331/Product/View/product5";
             dbcontext.Add(product5);

             Product product6 = new Product();
             product6.Id = Guid.NewGuid().ToString();
             product6.productName = ".NetNumerics";
             product6.description = "Powerful numerical methods for your .NET simulations";
             product6.unitPrice = 199.00;
             product6.CategoryId = cat1.Id;
             product6.Image = "https://localhost:44331/lib/images/numerics.JPG";
             product6.URL = "https://localhost:44331/Product/View/product6";
             dbcontext.Add(product6);

             Product product7 = new Product();
             product7.Id = Guid.NewGuid().ToString();
             product7.productName = "MS Access";
             product7.description = "Powerful Access methods to store data.";
             product7.unitPrice = 600.00;
             product7.CategoryId = cat2.Id;
             product7.Image = "https://localhost:44331/lib/images/MA.JPG";
            dbcontext.Add(product7);

             Product product8 = new Product();
             product8.Id = Guid.NewGuid().ToString();
             product8.productName = "MS Outlook";
             product8.description = "Powerful application to access your emails.";
             product8.unitPrice = 999.00;
             product8.CategoryId = cat2.Id;
             product8.Image = "https://localhost:44331/lib/images/Outlook.JPG";
            dbcontext.Add(product8);

             Product product9 = new Product();
             product9.Id = Guid.NewGuid().ToString();
             product9.productName = "MS Visual Studio";
             product9.description = "Powerful tool for you to program more programs";
             product9.unitPrice = 900.00;
             product9.CategoryId = cat2.Id;
             product9.Image = "https://localhost:44331/lib/images/vs.png";
            dbcontext.Add(product9);

              Purchase purchase1 = new Purchase();
              purchase1.Id = Guid.NewGuid().ToString();
              purchase1.CustomerId = cust2.Id;
              purchase1.ProductId = product1.Id;
              purchase1.PurchaseQty = 1;
              purchase1.UTCPurchaseDate = new DateTime(2015, 12, 25);
              purchase1.ActivationKey = "12345";
              dbcontext.Add(purchase1);


            Review review1 = new Review();
            review1.Id = Guid.NewGuid().ToString();
            review1.CustomerId = cust1.Id;
            review1.ProductId = product1.Id;
            review1.Stars = 5;
            review1.Comments = "Good product";
            review1.CreationTime = DateTime.Now;
            dbcontext.Add(review1);

            Review review2 = new Review();
            review2.Id = Guid.NewGuid().ToString();
            review2.CustomerId = cust2.Id;
            review2.ProductId = product2.Id;
            review2.Stars = 3;
            review2.Comments = "I do not like this product";
            review2.CreationTime = DateTime.Now;
            dbcontext.Add(review2);

            Review review3 = new Review();
            review3.Id = Guid.NewGuid().ToString();
            review3.CustomerId = cust3.Id;
            review3.ProductId = product3.Id;
            review3.Stars = 4;
            review3.Comments = "Not bad";
            review3.CreationTime = DateTime.Now;
            dbcontext.Add(review3);

            Review review4 = new Review();
            review4.Id = Guid.NewGuid().ToString();
            review4.CustomerId = cust3.Id;
            review4.ProductId = product4.Id;
            review4.Stars = 5;
            review4.Comments = "Good product";
            review4.CreationTime = DateTime.Now;
            dbcontext.Add(review4);

            Review review5 = new Review();
            review5.Id = Guid.NewGuid().ToString();
            review5.CustomerId = cust2.Id;
            review5.ProductId = product5.Id;
            review5.Stars = 5;
            review5.Comments = "Good product";
            review5.CreationTime = DateTime.Now;
            dbcontext.Add(review5);

            Review review6 = new Review();
            review6.Id = Guid.NewGuid().ToString();
            review6.CustomerId = cust1.Id;
            review6.ProductId = product6.Id;
            review1.Stars = 5;
            review6.Comments = "Good product";
            review6.CreationTime = DateTime.Now;
            dbcontext.Add(review6);

            Review review7 = new Review();
            review7.Id = Guid.NewGuid().ToString();
            review7.CustomerId = cust2.Id;
            review7.ProductId = product7.Id;
            review7.Stars = 5;
            review7.Comments = "Good product";
            review7.CreationTime = DateTime.Now;
            dbcontext.Add(review7);

            Review review8 = new Review();
            review8.Id = Guid.NewGuid().ToString();
            review8.CustomerId = cust2.Id;
            review8.ProductId = product8.Id;
            review8.Stars = 5;
            review8.Comments = "Good product";
            review8.CreationTime = DateTime.Now;
            dbcontext.Add(review8);

            Review review9 = new Review();
            review9.Id = Guid.NewGuid().ToString();
            review9.CustomerId = cust1.Id;
            review9.ProductId = product9.Id;
            review9.Stars = 5;
            review9.Comments = "Good product";
            review9.CreationTime = DateTime.Now;
            dbcontext.Add(review9);

            dbcontext.SaveChanges();
        }
    }
}

