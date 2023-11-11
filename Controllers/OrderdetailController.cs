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
public class OrderdetailController : ControllerBase
{
    private readonly NorthwindContext _context;

    public OrderdetailController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Orderdetail>>> GetOrderdetail()
    {
        var ordersdetail = await _context.Orderdetails
            .ToListAsync();
       
        return ordersdetail;
    }
    
           [HttpGet("{id}")]
public async Task<ActionResult<Orderdetail>> GetOrderdetail(int id)
{
    var orderdetail = await _context.Orderdetails.FindAsync(id);

    if (orderdetail == null)
    {
        return NotFound();
    }

    return orderdetail;
}

[HttpPost("{id}")]
public async Task<ActionResult<Orderdetail>> CreateOrderdetail([FromBody] Orderdetail orderdetail)
{
    if (orderdetail == null)
    {
        return BadRequest("Invalid orderdetail data.");
    }


    _context.Orderdetails.Add(orderdetail);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetOrderdetails", new { id = orderdetail.OrderDetailsId }, orderdetail);
}

[HttpPut("{id}")]
public async Task<ActionResult> PutOrderdetail(int id, Orderdetail orderdetail)
{
    if (id != orderdetail.OrderId)
    {
        return BadRequest();
    }

    _context.Entry(orderdetail).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!OrderdetailExists(id))
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
public async Task<ActionResult> DeleteOrderdetail(int id)
{
    var orderdetail= await _context.Orderdetails.FindAsync(id);
    if (orderdetail == null)
    {
        return NotFound();
    }

    _context.Orderdetails.Remove(orderdetail);
    await _context.SaveChangesAsync();

    return NoContent();
}

private bool OrderdetailExists(int id)
{
    return _context.Orderdetails.Any(e => e.OrderDetailsId == id);
}

    

}