using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTodo.Data;
using MyTodo.Models;
using MyTodo.VIewModels;

namespace MyTodo.Controllers;

[ApiController]
[Route("v1")]
public class TodoController : ControllerBase
{
  [HttpGet]
  [Route("todos")]
  public async Task<IActionResult> GetAsyc([FromServices] AppDbContext context)
  {
    var todos = await context
      .Todos
      .AsNoTracking()
      .ToListAsync();
    return Ok(todos);
  }

  [HttpGet]
  [Route("todos/{id}")]
  public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] uint id)
  {
    var todo = await context
      .Todos
      .AsNoTracking()
      .FirstOrDefaultAsync(item => item.Id == id);

    return todo == null ? NotFound() : Ok(todo);
  }

  [HttpPost("todos")]
  public async Task<IActionResult> PostAsync([FromServices] AppDbContext context,
    [FromBody] CreateTodoViewModel todoViewModel)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    Todo todo = new()
    {
      Date = DateTime.Now,
      IsDone = false,
      Title = todoViewModel.Title
    };

    try
    {
      await context.Todos.AddAsync(todo);
      await context.SaveChangesAsync();
      return Created($"v1/todos/{todo.Id}", todo);
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

  [HttpPut("todos/{id}")]
  public async Task<IActionResult> PutAsync([FromServices] AppDbContext context,
    [FromBody] CreateTodoViewModel todoViewModel, [FromRoute] uint id)
  {
    //Console.WriteLine("Aqui!!!");
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    Todo todo = await context
      .Todos
      .FirstOrDefaultAsync(item => item.Id == id);

    if (todo == null)
      return NotFound();

    try
    {
      todo.Title = todoViewModel.Title;
      context.Todos.Update(todo);
      await context.SaveChangesAsync();
      return Ok(todo);
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

  [HttpDelete("todos/{id}")]
  public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] uint id)
  {
    Todo todo = await context
      .Todos
      .FirstOrDefaultAsync(item => item.Id == id);

    if (todo == null)
      return NotFound();

    try
    {
      context.Todos.Remove(todo);
      await context.SaveChangesAsync();
      return Ok();
    }
    catch (Exception e)
    {
      return BadRequest();
    }
  }
}