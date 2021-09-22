using Models.Users;

namespace web_api.Usecases
{
  public interface IAuthUC
  {
    public (int, string) SignUp(User user);
    public string SignIn(User user);
  }
}