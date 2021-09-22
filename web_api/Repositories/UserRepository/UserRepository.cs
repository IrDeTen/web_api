using Models.Users;
using Models.ApplicationContext;
using System.Linq;

namespace web_api.Repositories.UserRepository
{
  public class UserRepository : IUserRepository
  {
    private readonly AppDBContext _context;

    public UserRepository(AppDBContext context)
    {
      _context = context;
    }

    public int CreateUser(User user)
    {
      _context.Add<User>(user);
      _context.SaveChanges();
      return GetUser(user.Login).ID;
    }

    public User GetUser(string login)
    {
      return _context.Users.FirstOrDefault(u => u.Login == login);
    }
  }
}