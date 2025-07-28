﻿using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models;

/// <summary>
/// Represents an individual product for sale
/// </summary>
public class Product
{   
    /// <summary>
    /// The unique identifier for the product
    /// </summary>
    [Key]
    public int ProductID { get; set; }

    /// <summary>
    /// The user facing title of the product
    /// </summary>
    [StringLength(50, ErrorMessage = "Titles cannot be more than 50 characters")]
    public required string Title { get; set; }

    /// <summary>
    /// The current sales price of the product
    /// </summary>
    [Range(0, 10000)]
    public decimal Price { get; set; }
}
