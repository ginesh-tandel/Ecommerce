namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemResponse> Items { get; set; }
        public ShoppingCartResponse()
        {
            UserName = string.Empty;
            Items = [];
        }
        public ShoppingCartResponse(string userName) : this(userName, [])
        {
            UserName = userName;
            Items = [];
        }
        public ShoppingCartResponse(string userName, List<ShoppingCartItemResponse> items)
        {
            UserName = userName ?? string.Empty;
            Items = items ?? [];
        }
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}
