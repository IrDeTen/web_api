using Microsoft.EntityFrameworkCore;

namespace web_api.Models
{
  public class ApplicationContext : DbContext
  {
      public DbSet<User> Users { get; set; }
      public ApplicationContext(DbContextOptions<ApplicationContext> options)
          : base(options)
      {
          Database.EnsureCreated();   // создаем базу данных при первом обращении
      }
  }
}