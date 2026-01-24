using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using static System.String;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeePostController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] bool includeDeleted = false)
    {
        try
        {
            var query = _db.EmployeePosts.AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(p => !p.IsDeprecated);
            }

            var list = await query.ToListAsync();
            var dtoList = list.Select(item => new EmployeePostReadDto(item.Id, item.Name, item.IsDeprecated)).ToList();

            return Ok(dtoList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _db.EmployeePosts.FindAsync(id);

        if (employee is null)
        {
            return NotFound($"Запись с Id: {id} не найдена");
        }

        return Ok(new EmployeePostReadDto(employee.Id, employee.Name, employee.IsDeprecated));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeePostDto employeeDto)
    {
        if (IsNullOrWhiteSpace(employeeDto.Name))
        {
            return BadRequest("Необходимо передать название");
        }

        var employee = new EmployeePost
        {
            Name = employeeDto.Name
        };

        _db.EmployeePosts.Add(employee);

        await _db.SaveChangesAsync();

        var resultDto = new EmployeePostReadDto(employee.Id, employee.Name, employee.IsDeprecated);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, resultDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EmployeePostDto employeeDto)
    {
        if (IsNullOrWhiteSpace(employeeDto.Name))
        {
            return BadRequest("Необходимо передать название");
        }

        var employee = await _db.EmployeePosts.FindAsync(id);

        if (employee is null)
        {
            return NotFound($"Запись с Id: {id} не найдена");
        }

        employee.Name = employeeDto.Name;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var employee = await _db.EmployeePosts.FindAsync(id);

        if (employee is null)
        {
            return NotFound($"Запись с Id: {id} не найдена");
        }

        if (employee.IsDeprecated)
        {
            return BadRequest($"Тип подразделения с Id: {id} уже упразднён");
        }

        employee.IsDeprecated = true;

        await _db.SaveChangesAsync();

        return NoContent();
    }
}
