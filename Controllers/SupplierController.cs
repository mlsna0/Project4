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
    public class SupplierController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public SupplierController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .ToListAsync();
        
            return suppliers;
        }

                [HttpGet("{id}")]
            public async Task<ActionResult<Supplier>> GetSuppliers(int id)
            {
                var suppliers = await _context.Suppliers.FindAsync(id);

                if (suppliers == null)
                {
                    return NotFound();
                }

                return suppliers;
            }
[HttpPost("{id}")]
public async Task<ActionResult<Supplier>> CreateSupplier([FromBody] Supplier supplier)
{
    if (supplier == null)
    {
        return BadRequest("Invalid supplier data.");
    }


    _context.Suppliers.Add(supplier);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetSuppliers", new { id = supplier.SupplierId }, supplier);
}

            [HttpPut("{id}")]
    public async Task<ActionResult> PutSupplier(int id, Supplier supplier)
    {
    if (id != supplier.SupplierId)
    {
    return BadRequest();
    }

    _context.Entry(supplier).State = EntityState.Modified;

    try
    {
    await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
    if (!SupplierExists(id))
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
    private bool SupplierExists(int id)
    {
    return _context.Suppliers.Any(e => e.SupplierId == id);
    }

[HttpDelete("{id}")]
public async Task<ActionResult> DeleteSupplier(int id)
{
    var supplier = await _context.Suppliers.FindAsync(id);
    if (supplier == null)
    {
        return NotFound();
    }

    var productsLinkedToSupplier = await _context.Products
        .Where(p => p.SupplierId == id)
        .ToListAsync();

    if (productsLinkedToSupplier.Any())
    {

        return BadRequest("Cannot delete Supplier with linked Products.");
    }

    _context.Suppliers.Remove(supplier);
    await _context.SaveChangesAsync();

    return NoContent();
}



    }