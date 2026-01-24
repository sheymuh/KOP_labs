using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Data;
using EmployeeApi.Entities;
using static System.String;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    /// <summary>
    /// Получить список всех сотрудников (только не удаленные)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(bool includeDeleted = false)
    {
        try
        {
            var employees = includeDeleted ? await _db.Employees.ToListAsync() : await _db.Employees.Where(s => !s.IsDeleted).ToListAsync();

            var dtoList = employees.Select(e => new EmployeeReadDto(
                e.Id,
                e.FIO,
                e.EmployeePostId,
                e.Autobiography,
                e.PromotionDate,
                e.IsDeleted
            )).ToList();

            return Ok(dtoList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    /// <summary>
    /// Получить сотрудника по ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var employee = await _db.Employees
                .Include(e => e.EmployeePost)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (employee is null)
            {
                return NotFound($"Сотрудник с Id: {id} не найден");
            }

            var dto = new EmployeeReadDto(
                employee.Id,
                employee.FIO,
                employee.EmployeePostId,
                employee.Autobiography,
                employee.PromotionDate,
                employee.IsDeleted
            );

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    /// <summary>
    /// Создать нового сотрудника
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto createDto)
    {
        try
        {
            if (IsNullOrWhiteSpace(createDto.FIO))
            {
                return BadRequest("Необходимо передать ФИО сотрудника");
            }

            // if exists in local cache
            var employeePost = await _db.EmployeePostCache.FindAsync(createDto.EmployeePostId);
            if (employeePost is null)
            {
                return BadRequest($"Должность сотрудника с Id: {createDto.EmployeePostId} не найдена");
            }

            if (employeePost.IsDeprecated)
            {
                return BadRequest($"Должность сотрудника с Id: {createDto.EmployeePostId} устарела и не может быть использована");
            }

            var employee = new Entities.Employee
            {
                FIO = createDto.FIO,
                EmployeePostId = createDto.EmployeePostId,
                Autobiography = createDto.Autobiography,
                PromotionDate = createDto.PromotionDate,
                IsDeleted = false
            };

            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            var resultDto = new EmployeeReadDto(
                employee.Id,
                employee.FIO,
                employee.EmployeePostId,
                employee.Autobiography,
                employee.PromotionDate,
                employee.IsDeleted
            );

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, resultDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновить существующего сотрудника
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, EmployeeUpdateDto updateDto)
    {
        try
        {
            if (IsNullOrWhiteSpace(updateDto.FIO))
            {
                return BadRequest("Необходимо передать ФИО сотрудника");
            }

            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (employee is null)
            {
                return NotFound($"Сотрудник с Id: {id} не найден");
            }

            // Проверяем, что должность сотрудника существует в кэше
            var employeePost = await _db.EmployeePostCache.FindAsync(updateDto.EmployeePostId);
            if (employeePost is null)
            {
                return BadRequest($"Должность сотрудника с Id: {updateDto.EmployeePostId} не найдена");
            }

            if (employeePost.IsDeprecated)
            {
                return BadRequest($"Должность сотрудника с Id: {updateDto.EmployeePostId} устарела и не может быть использована");
            }

            employee.FIO = updateDto.FIO;
            employee.EmployeePostId = updateDto.EmployeePostId;
            employee.Autobiography = updateDto.Autobiography;
            employee.PromotionDate = updateDto.PromotionDate;

            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    /// <summary>
    /// Удалить сотрудника (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (employee is null)
            {
                return NotFound($"Сотрудник с Id: {id} не найден");
            }

            // Soft delete
            employee.IsDeleted = true;
            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }
}
