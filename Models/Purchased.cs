using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.Models
{
    public class Purchased
    {
        public int Id { get; set; }
        public int CartItemId { get; set; }
        public string ActivationCode { get; set; }

        public Purchased()
        {
        }
        public virtual CartItem CartItem { get; set; }
        public virtual Purchase Purchase { get; set; }
        public Purchased(int cartItemId, string activationCode)
        {
            this.CartItemId = cartItemId;
            this.ActivationCode = activationCode;
        }
    }
}
