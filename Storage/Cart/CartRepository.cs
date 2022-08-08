using Microsoft.EntityFrameworkCore;

namespace Storage.Cart;

public class CartRepository : ICartRepository
{
    private readonly ApplicationContext _db;

    public CartRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CartItem>> GetCartItemsForUser(string userId)
    {
        return await _db.CartItems.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<CartItem>> GetItems()
    {
        return await _db.CartItems.ToListAsync();
    }

    public async Task<CartItem> AddItemToCart(string productId, string userId)
    {
        var item = new CartItem
        {
            ProductId = productId,
            UserId = userId
        };
        await _db.CartItems.AddAsync(item);
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task RemoveCartItem(string itemId)
    {
        var item = new CartItem {Id = itemId};
        _db.CartItems.Attach(item);
        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
    }
}
