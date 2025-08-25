using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Controllers;

/// <summary>
/// Controller for managing products in the eCommerce application.
/// </summary>
public class ProductController : Controller
{
    private readonly ProductDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProductController class with the specified database context.
    /// </summary>
    /// <param name="context">The product controller object</param>
    public ProductController(ProductDbContext context)
    {
        _context = context; // Assign the provided context to the private field
    }

    /// <summary>
    /// Displays a list of all products in the database.
    /// </summary>
    /// <returns>Redirects the user to the products index page with
    ///  all products populated from the database.</returns>
    public async Task<IActionResult> Index( string? searchTerm, decimal? minPrice, decimal? maxPrice, int page = 1)
    {
        const int ProductsPerPage = 3; // Products per page

        IQueryable<Product> query = _context.Products; // Start creating query, doesn't run yet

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowered = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Title.Contains(searchTerm)); // Filter by search term
        }
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value); // Filter by minimum price
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value); // Filter by maximum price
        }

        int totalProducts = await query.CountAsync(); // Get the total count of products after filtering
        int totalPagesNeeded = (int)Math.Ceiling(totalProducts / (double)ProductsPerPage); // Calculate total number of pages

        if (page < 1) // Ensure the page number is at least 1
            page = 1; 

        // If the user tries to navigate beyond the last page, set page to the last page
        if (totalPagesNeeded > 0 && page > totalPagesNeeded) page = totalPagesNeeded; 

        List<Product> allProducts = await query
            .OrderBy(p => p.Title) // Order products by ProductId
            .Skip((page - 1) * ProductsPerPage) // Skip products for previous pages
            .Take(ProductsPerPage) // Take only the products for the current page
            .ToListAsync(); // Retrieve all products from the database asynchronously

        ProductListViewModel productListViewModel = new()
        {
            Products = allProducts,
            CurrentPage = page,
            TotalPages = totalPagesNeeded,
            PageSize = ProductsPerPage,
            TotalItems = totalProducts,
            SearchTerm = searchTerm,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        return View(productListViewModel); // Return the index view with the list of products
    }

    /// <summary>
    /// Displays the create view for a new product.
    /// </summary>
    /// <returns>returns the create view</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View(); // Return the create view for a new product
    }

    /// <summary>
    /// Creates a new product and saves it to the database.
    /// </summary>
    /// <param name="p">The product in the database</param>
    /// <returns>User directed to the index page</returns>
    [HttpPost]
    public async Task<IActionResult> Create(Product p)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(p);           // Add the product to the context
            await _context.SaveChangesAsync();  // Save changes to the database

            // TempData is used to pass data and will persist over a redirect
            TempData["Message"] = $"{p.Title} has been added successfully!"; // Set a success message in TempData

            return RedirectToAction(nameof(Index));
        }
        return View(p); // If model state is invalid, return the view with the product data and validation errors

    }

    /// <summary>
    /// Displays the edit view for a product with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the product to edit</param>
    /// <returns>returns the edit view</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id) 
    {
        Product? product = await _context.Products.FindAsync(id); // Find the product by ID

        if (product == null) // Check if the product exists
        {
                return NotFound(); // If not found, return a 404 Not Found response
        }

            return View(product); // Return the edit view with the product data
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    /// <param name="product">The product in the database</param>
    /// <returns>User directed to the product index page</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Update(product); // Update the product in the context
            await _context.SaveChangesAsync(); // Save changes to the database
            
            TempData["Message"] = $"{product.Title} has been updated successfully!"; // Set a success message in TempData
            return RedirectToAction(nameof(Index));
        }
        return View(product); // If model state is invalid, return the view with the product data and validation errors
    }

    /// <summary>
    /// Displays the confirmation page for deleting a product.
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>returns the delete view</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Product? product = await _context.Products
            .Where(p => p.ProductId == id).FirstOrDefaultAsync(); // Find the product by ID

        if (product == null) // Check if the product exists
        {
            return NotFound(); // If not found, return a 404 Not Found response
        }
        return View(product); // Return the delete view with the product data
    }

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>User directed to products index page</returns>
    [ActionName(nameof(Delete))]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Product? product = _context.Products.Find(id); // Find the product by ID

        if (product == null) // Check if the product exists
        {
            return RedirectToAction(nameof(Index)); // If not found, redirect to the index page
        }

        _context.Remove(product); // Remove the product from the context
        await _context.SaveChangesAsync(); // Save changes to the database

        TempData["Message"] = $"{product.Title} has been deleted successfully!"; // Set a success message in TempData
        return RedirectToAction(nameof(Index)); // Redirect to the index page after deletion
    }

}
