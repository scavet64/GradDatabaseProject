namespace Kinabalu.Models
{
    public class OrderProductViewModel
    {
        public Order Order { get; set; }
        public string ProductSource { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }
}