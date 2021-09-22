namespace Models.Response
{
  public class ErrorResponse
  {
    public string Message { get; set; }
    public string Status { get; set; }

    public ErrorResponse(string msg)
    {
      Status = "error";
      Message = msg;
    }
      
  }
}