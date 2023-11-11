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
public class ShipperController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ShipperController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipper>>> GetShippers()
    {
        var shippers = await _context.Shippers
            .ToListAsync();
       
        return shippers;
    }
    
            [HttpGet("{id}")]
        public async Task<ActionResult<Shipper>> GetShippers(int id)
        {
            var shippers = await _context.Shippers.FindAsync(id);

            if (shippers == null)
            {
                return NotFound();
            }

            return shippers;
        }
[HttpPost("{id}")]
public async Task<ActionResult<Shipper>> CreateShipper([FromBody] Shipper shipper)
{
    if (shipper == null)
    {
        return BadRequest("Invalid shipper data.");
    }


    _context.Shippers.Add(shipper);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetShippers", new { id = shipper.ShipperId }, shipper);
}

        [HttpPut("{id}")]
public async Task<ActionResult> PutShipper(int id, Shipper shipper)
{
if (id != shipper.ShipperId)
{
return BadRequest();
}

_context.Entry(shipper).State = EntityState.Modified;

try
{
await _context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
if (!ShipperExists(id))
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
private bool ShipperExists(int id)
{
return _context.Shippers.Any(e => e.ShipperId == id);
}


[HttpDelete("{id}")]
public async Task<ActionResult> DeleteShipper(int id)
{
    var shippers = await _context.Shippers.FindAsync(id);
    if (shippers == null)
    {
        return NotFound();
    }

    _context.Shippers.Remove(shippers);
    await _context.SaveChangesAsync();

    return NoContent();
}


}