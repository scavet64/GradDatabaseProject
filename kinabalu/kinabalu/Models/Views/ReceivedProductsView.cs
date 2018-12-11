using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.Views
{
    public class ReceivedProductsView
    {
        public int CustomerId { get; set; }
        public string CustomerSource { get; set; }
        public int ProductId { get; set; }
        public string ProductSource { get; set; }
    }
}
