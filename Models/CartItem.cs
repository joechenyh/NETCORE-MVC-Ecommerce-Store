using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        

        [NotMapped]
        public virtual DateTime CheckoutTime { get; set; }

        [NotMapped]
        public virtual List<string> ActivationCodes { get; set; }
        public virtual Cart Cart { get; set; }

        public CartItem()
        {
        }

        public CartItem(int cartId, string productId)
        {
            this.CartId = cartId;
            this.ProductId = productId;
            this.Quantity = 1;
        }
    }
}
