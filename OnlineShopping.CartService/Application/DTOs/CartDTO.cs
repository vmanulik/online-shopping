﻿namespace OnlineShopping.CartService.Application.DTOs;

public record CartDTO
{
    public Guid Id { get; private set; }
    public int ItemsQuantity { get; set; }
}
