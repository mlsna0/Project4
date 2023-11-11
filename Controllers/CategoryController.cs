using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly NorthwindContext _context;

    public CategoryController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategorys()
    {
        var categories = await _context.Categories
            .ToListAsync();
       
        return categories;
    }
   
            [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategorys(long id)
        {
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return categories;
        }

[HttpPost("{id}")]
public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
{
    if (category == null)
    {
        return BadRequest("Invalid category data.");
    }


    _context.Categories.Add(category);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetCategories", new { id = category.CategoryId }, category);
}

        [HttpPut("{id}")]
public async Task<ActionResult> PutCategory(int id, Category category)
{
if (id != category.CategoryId)
{
return BadRequest();
}

_context.Entry(category).State = EntityState.Modified;

try
{
await _context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
if (!CategoryExists(id))
{
return NotFound();
}

else
{
throw;
}
}

return NoContent();
}
private bool CategoryExists(int id)
{
return _context.Categories.Any(e => e.CategoryId == id);
}


[HttpDelete("{id}")]
public async Task<ActionResult> DeleteCategory(int id)
{
    var categories = await _context.Categories.FindAsync(id);
    if (categories == null)
    {
        return NotFound();
    }

    _context.Categories.Remove(categories);
    await _context.SaveChangesAsync();

    return NoContent();
}


}