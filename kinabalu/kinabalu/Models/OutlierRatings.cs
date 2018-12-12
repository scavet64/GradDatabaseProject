namespace Kinabalu.Models
{
    public class OutlierRatings
    {
        public int CustomerId { get; set; }
        public string CustomerSource { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public bool Outlier { get; set; }
    }
}