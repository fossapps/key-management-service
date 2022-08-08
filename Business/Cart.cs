using Storage.Cart;

namespace Business;

public interface ICart
{
    Task<IEnumerable<CartItem>> GetItems();
}

public class Cart : ICart
{
    private readonly ICartRepository _cartRepository;

    public Cart(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<IEnumerable<CartItem>> GetItems()
    {
        return await _cartRepository.GetItems();
    }
}
