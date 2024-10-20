﻿using OnlineShopping.Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.Domain.Entities;

public class Cart
{
    private readonly Dictionary<int, Item> _items = new();

    public Guid Id { get; private set; }

    private Cart() { }

    public Cart(Guid guid)
    {
        Id = guid;
    }

    public void AddItem(int id, string name, decimal price)
    {
        var item = _items[id];

        if (item == null)
        {
            _items[id] = new Item(id, name, price);
        }
        else
        {
            item.IncrementQuantity();
        }
    }

    public void RemoveItem(int id)
    {
        var item = _items[id];

        if (item == null)
        {
            throw new ItemNotFoundException($"Item ID {id} was not found in the {nameof(Cart)}");
        }
        else
        {
            _items.Remove(id);
        }
    }

    public void SetItemQuantity(int id, int quantity)
    {
        var item = _items[id];

        if (item == null)
        {
            throw new ItemNotFoundException($"Item ID {id} was not found in the {nameof(Cart)}");
        }
        else
        {
            item.SetQuantity(quantity);
        }
    }

    public List<Item> GetItems()
    {
        return _items.Values.ToList();
    }
}