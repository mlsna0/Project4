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
public class OrderController : ControllerBase
{
    private readonly NorthwindContext _context;

    public OrderController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
    {
        var orders = await _context.Orders
            .ToListAsync();
       
        return orders;
    }
    
           [HttpGet("{id}")]
public async Task<ActionResult<Order>> GetOrder(int id)
{
    var order = await _context.Orders.FindAsync(id);

    if (order == null)
    {
        return NotFound();
    }

    return order;
}

[HttpPost("{id}")]
public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
{
    if (order == null)
    {
        return BadRequest("Invalid order data.");
    }


    _context.Orders.Add(order);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetOrders", new { id = order.OrderId }, order);
}

[HttpPut("{id}")]
public async Task<ActionResult> PutOrder(int id, Order order)
{
    if (id != order.OrderId)
    {
        return BadRequest();
    }

    _context.Entry(order).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!OrderExists(id))
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
public async Task<ActionResult> DeleteOrder(int id)
{
    var order= await _context.Orders.FindAsync(id);
    if (order == null)
    {
        return NotFound();
    }

    _context.Orders.Remove(order);
    await _context.SaveChangesAsync();

    return NoContent();
}

private bool OrderExists(int id)
{
    return _context.Orders.Any(e => e.OrderId == id);
}

    

}