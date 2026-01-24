using EmployeeApi.Data;
using EmployeeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db;

    /// <summary>
    /// Получить PDF-отчёт со списком сотрудников, прошедших квалификацию.
    /// </summary>
    /// <param name="includeDeleted">Включить ли сотрудников, помеченных как удалённые.</param>
    [HttpGet("pdf")]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdfReport([FromQuery] bool includeDeleted = false)
    {
        var employees = await _db.Employees
            .Include(e => e.EmployeePost)
            .ToListAsync();

        var hasData = employees.Any(e => e.PromotionDate != null && (includeDeleted || !e.IsDeleted));
        if (!hasData)
        {
            return NoContent();
        }

        var content = ReportService.GeneratePdfReport(employees, includeDeleted);
        var fileName = $"promoted-employees-{DateTime.UtcNow:yyyyMMdd}.pdf";
        return File(content, "application/pdf", fileName);
    }

    /// <summary>
    /// Получить DOCX-отчёт с диаграммой по сотрудникам, не прошедшим квалификацию.
    /// </summary>
    /// <param name="includeDeleted">Включить ли сотрудников, помеченных как удалённые.</param>
    [HttpGet("word")]
    [Produces("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetWordReport([FromQuery] bool includeDeleted = false)
    {
        var employees = await _db.Employees
            .Include(e => e.EmployeePost)
            .ToListAsync();

        var hasData = employees.Any(e => e.PromotionDate == null && e.EmployeePost != null && (includeDeleted || !e.IsDeleted));
        if (!hasData)
        {
            return NoContent();
        }

        var content = ReportService.GenerateWordReport(employees, includeDeleted);
        var fileName = $"not-promoted-employees-{DateTime.UtcNow:yyyyMMdd}.docx";
        return File(content, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
    }
}
