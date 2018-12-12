using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.AddressViewModels
{
    public class CreateAddressViewModel
    {
        public string State { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string ReturnUrl { get; set; }
    }
}
