namespace Storage.Cart;

public interface ICartRepository
{
    Task<IEnumerable<CartItem>> GetCartItemsForUser(string userId);
    Task<IEnumerable<CartItem>> GetItems();
    Task<CartItem> AddItemToCart(string productId, string userId);
    Task RemoveCartItem(string itemId);
}
