using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using static System.String;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeePostController : ControllerBase
{
    private static readonly List<EmployeePost> _data = [];

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_data);
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmployeePostDto postDto)
    {
        if (IsNullOrWhiteSpace(postDto.Name))
        {
            return BadRequest("Необходимо передать название");
        }

        var post = new EmployeePost(Guid.NewGuid(), postDto.Name);
        _data.Add(post);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] EmployeePostDto employeeDto)
    {
        if (IsNullOrWhiteSpace(employeeDto.Name))
        {
            return BadRequest("Необходимо передать запись");
        }

        var index = _data.FindIndex(s => s.Id == id);
        if (index == -1)
        {
            return NotFound($"Запись с Id: {id} не найдена");
        }

        _data[index].Name = employeeDto.Name;

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var index = _data.FindIndex(s => s.Id == id);
        if (index == -1)
        {
            return NotFound($"Запись с Id: {id} не найдена");
        }

        _data.RemoveAt(index);
        return NoContent();
    }
}
