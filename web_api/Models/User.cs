using System.ComponentModel.DataAnnotations;

namespace Models.Users
{
  public class User
  {
    public int ID { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    public bool Internal { get; set; }
  }
}