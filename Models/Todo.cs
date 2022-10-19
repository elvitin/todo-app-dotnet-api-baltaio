using System.ComponentModel.DataAnnotations;

namespace MyTodo.Models;

public class Todo
{
  public uint Id { get; set; }
  
  [Required]
  public string Title { get; set; }
  public bool IsDone { get; set; }
  public DateTime Date { get; set; } = DateTime.Now;
}