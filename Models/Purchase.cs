using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace NETCORE_CA_8A.Models
{
    public class Purchase
    {
        

        [MaxLength(36)] 
        public string Id { get; set; }

        
        [MaxLength(36)]
        public string CartId { get; set; }

        [Required]
        [MaxLength(36)]       
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }

        //Use navigational property for product name 
        /*
        [Required]
        [MaxLength(36)]
        public string ProductName { get; set; } */

        [Required]
        [MaxLength(3)]
        public double PurchaseQty { get; set; }

        [Required]
        [MaxLength(36)]
        
        public string ActivationKey { get; set; }

        [Required]
        [MaxLength(36)]
        public DateTime UTCPurchaseDate { get; set; }

        public virtual Product Product { get; set; }

    }
}
