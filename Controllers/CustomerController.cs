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
public class CustomerController : ControllerBase
{
    private readonly NorthwindContext _context;

    public CustomerController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
    {
        var customers = await _context.Customers
            .ToListAsync();
       
        return customers;
    }
    
           [HttpGet("{id}")]
public async Task<ActionResult<Customer>> GetCustomer(string id)
{
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
    {
        return NotFound();
    }

    return customer;
}

[HttpPost("{id}")]
public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
{
    if (customer == null)
    {
        return BadRequest("Invalid customer data.");
    }


    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetCustomers", new { id = customer.CustomerId }, customer);
}

[HttpPut("{id}")]
public async Task<ActionResult> PutCustomer(string id, Customer customer)
{
    if (id != customer.CustomerId)
    {
        return BadRequest();
    }

    _context.Entry(customer).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!CustomerExists(id))
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

[HttpDelete("{id}")]
public async Task<ActionResult> DeleteCustomer(string id)
{
    var customer = await _context.Customers.FindAsync(id);
    if (customer == null)
    {
        return NotFound();
    }

    _context.Customers.Remove(customer);
    await _context.SaveChangesAsync();

    return NoContent();
}

private bool CustomerExists(string id)
{
    return _context.Customers.Any(e => e.CustomerId == id);
}

    

}