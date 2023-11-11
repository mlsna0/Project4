using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly NorthwindContext _context;

    public EmployeeController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
{
    var employees = await _context.Employees.ToListAsync();
    return employees;
}

[HttpGet("{id}")]
public async Task<ActionResult<Employee>> GetEmployee(int id)
{
    var employee = await _context.Employees.FindAsync(id);

    if (employee == null)
    {
        return NotFound();
    }

    return employee;
}

[HttpPost("{id}")]
public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
{
    if (employee == null)
    {
        return BadRequest("Invalid employee data.");
    }


    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();


    return CreatedAtAction("GetEmployees", new { id = employee.EmployeeId }, employee);
}

        [HttpPut("{id}")]
public async Task<ActionResult> PutEmployee(int id, Employee employee)
{
if (id != employee.EmployeeId)
{
return BadRequest();
}

_context.Entry(employee).State = EntityState.Modified;

try
{
await _context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
if (!EmployeeExists(id))
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
private bool EmployeeExists(int id)
{
return _context.Employees.Any(e => e.EmployeeId == id);
}


[HttpDelete("{id}")]
public async Task<ActionResult> DeleteEmployee(int id)
{
    var employees = await _context.Employees.FindAsync(id);
    if (employees == null)
    {
        return NotFound();
    }

    _context.Employees.Remove(employees);
    await _context.SaveChangesAsync();

    return NoContent();
}
}