using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.Views
{
    public class ProductShipmentView
    {
        public string Customer { get; set; }
        public string Product { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
