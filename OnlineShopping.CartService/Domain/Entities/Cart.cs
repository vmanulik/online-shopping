using LiteDB;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }

    public List<Item> Items { get; set; } = new();

    public Cart() { }

    public Cart(Guid id)
    {
        Id = id;
    }

    public void AddItem(Item item)
    {
        var storedItem = Items.SingleOrDefault(i => i.Id == item.Id);

        if (storedItem == null)
        {
            Items.Add(item);
        }
        else
        {
            storedItem.SetQuantity(storedItem.Quantity + item.Quantity);
        }
    }

    public void RemoveItem(int id)
    {
        var item = Items.SingleOrDefault(i => i.Id == id);

        if (item == null)
        {
            throw new NotFoundException($"Item ID {id} was not found in the {nameof(Cart)}");
        }
        else
        {
            Items.Remove(item);
        }
    }

    public void SetItemQuantity(int id, int quantity)
    {
        var item = Items.SingleOrDefault(i => i.Id == id);

        if (item == null)
        {
            throw new NotFoundException($"Item ID {id} was not found in the {nameof(Cart)}");
        }
        else
        {
            item.SetQuantity(quantity);
        }
    }

    public List<Item> GetItems()
    {
        return Items;
    }
}
