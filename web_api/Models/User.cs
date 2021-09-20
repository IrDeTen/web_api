using System.ComponentModel.DataAnnotations;
namespace web_api.Models{
  public class User
  {
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public bool Internal { get; set; }
  }
}