using API.types;
using Business;
using Storage.Cart;

namespace API;

public class Query
{
    public Task<User> Me()
    {
        return Task.FromResult(new User("user_id"));
    }

    public async Task<IEnumerable<CartItem>> Cart([Service]ICart cart)
    {
        return await cart.GetItems();
    }
}
