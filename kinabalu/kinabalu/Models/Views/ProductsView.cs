using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models
{
    public class ProductsView
    {
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Cost")]
        public double Cost { get; set; }

        [Display(Name = "Quantity")]
        public int? Quantity { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Average Rating")]
        public double? AverageRating { get; set; }

        [Display(Name = "Weighted Average")]
        public double? AverageReceivedRating { get; set; }
    }
}
