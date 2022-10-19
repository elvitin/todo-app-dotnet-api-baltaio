using System.ComponentModel.DataAnnotations;

namespace MyTodo.VIewModels;

public class CreateTodoViewModel
{
  [Required]
  public string Title { get; set; }
}