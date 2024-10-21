using OnlineShopping.CartService.Domain.Entities;
using Shared.Domain.Exceptions;

namespace OnlineShopping.CartService.Tests.UnitTests
{
    [TestFixture]
    public class CartTests
    {
        private Cart _cart;

        [SetUp]
        public void SetUp()
        {
            _cart = new Cart();
        }

        [Test]
        public void AddItem_ItemNotInCart_AddsItem()
        {
            var item = new Item { Id = 1, Quantity = 1 };

            _cart.AddItem(item);

            Assert.That(_cart.Items.Count, Is.EqualTo(1));
            Assert.That(_cart.Items.Single(), Is.EqualTo(item));
        }

        [Test]
        public void AddItem_ItemAlreadyInCart_IncreasesQuantity()
        {
            var item1 = new Item { Id = 1, Quantity = 1 };
            _cart.AddItem(item1);

            var item2 = new Item { Id = 1, Quantity = 2 };
            _cart.AddItem(item2);

            Assert.That(_cart.Items.Count, Is.EqualTo(1));
            Assert.That(_cart.Items.Single().Quantity, Is.EqualTo(3));
        }

        [Test]
        public void RemoveItem_ItemInCart_RemovesItem()
        {
            var item = new Item { Id = 1, Quantity = 1 };
            _cart.AddItem(item);

            _cart.RemoveItem(item.Id);

            Assert.That(_cart.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveItem_ItemNotInCart_ThrowsException()
        {
            var ex = Assert.Throws<NotFoundException>(() => _cart.RemoveItem(1));
            Assert.That(ex.Message, Is.EqualTo("Item ID 1 was not found in the Cart"));
        }

        [Test]
        public void SetItemQuantity_ItemInCart_UpdatesQuantity()
        {
            var item = new Item { Id = 1, Quantity = 1 };
            _cart.AddItem(item);

            _cart.SetItemQuantity(1, 5);

            Assert.That(_cart.Items.Single().Quantity, Is.EqualTo(5));
        }

        [Test]
        public void SetItemQuantity_ItemNotInCart_ThrowsException()
        {
            var ex = Assert.Throws<NotFoundException>(() => _cart.SetItemQuantity(1, 5));
            Assert.That(ex.Message, Is.EqualTo("Item ID 1 was not found in the Cart"));
        }

        [Test]
        public void GetItems_ReturnsAllItems()
        {
            var item1 = new Item { Id = 1, Quantity = 1 };
            _cart.AddItem(item1);
            var item2 = new Item { Id = 2, Quantity = 2 };
            _cart.AddItem(item2);

            var items = _cart.GetItems();

            Assert.That(items.Count, Is.EqualTo(2));
            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }
    }
}