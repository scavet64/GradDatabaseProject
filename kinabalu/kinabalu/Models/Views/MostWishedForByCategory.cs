using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.Views
{
    public class MostWishedForByCategory
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public int Wishes { get; set; }
    }
}
