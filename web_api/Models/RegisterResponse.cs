namespace Models.Response
{
  public class RegisterResponse
  {
    public string Token { get; set; }
    public string Status { get; set; }
    public int ID { get; set; }

    public RegisterResponse(string token, int id)
    {
      Status = "success";
      Token = token;
      ID = id;
    }
      
  }
}