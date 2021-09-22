namespace Models.Response
{
  public class AuthResponse
  {
    public string Token { get; set; }
    public string Status { get; set; }

    public AuthResponse(string token)
    {
      Status = "success";
      Token = token;
    }
      
  }
}