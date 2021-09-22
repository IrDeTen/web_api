using Models.Users;

namespace web_api.Repositories
{
  public interface IUserRepository
  {
    public User GetUser(string login);
    public int CreateUser(User user);
  }
}