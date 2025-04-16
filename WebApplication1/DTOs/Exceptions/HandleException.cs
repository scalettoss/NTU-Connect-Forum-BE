namespace ForumBE.DTOs.Exception
{
    public class HandleException : System.Exception
    {
        public int Status { get; }
        public HandleException(string message, int status) : base(message)
        {
            Status = status;
        }
    }
}
