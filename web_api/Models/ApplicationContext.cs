using Microsoft.EntityFrameworkCore;
using Models.Users;

namespace Models.ApplicationContext
{
  public class AppDBContext : DbContext
  {
      public DbSet<User> Users { get; set; }
      public AppDBContext(DbContextOptions<AppDBContext> options)
          : base(options)
      {
          Database.EnsureCreated();   // создаем базу данных при первом обращении
      }
  }
}