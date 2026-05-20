using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_17_KosmeticheskayaLaja
{
    public partial class Products
    {
        public bool IsHighDiscount => Discount > 15;

        public int QuantityInCart
        {
            get
            {
                return Core.Cart.Count(p => p.ID_Product == this.ID_Product);
            }
        }
    }
}
