using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.Models
{
    public class Product
    {
        [MaxLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string productName { get; set; }

    
        [MaxLength(300)]
        public string productLongDesc { get; set; }

        [Required]
        [MaxLength(200)]
        public string description { get; set; }

        [Required]
        [MaxLength(3)]
        public double unitPrice { get; set; }

        [Required]
        [MaxLength(36)]
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public Product()
        {
        }

        //joe testing out
        public string URL { get; set; }

        public string Image { get; set; }

        public Product(string name, string desc, int quantity, double price)
        {
            this.productName = name;
            this.description = desc;
            this.unitPrice = price;
        }

        public string Recommendation { get; set; }
        public string Review { get; set; }
    }
}

