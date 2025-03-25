namespace ForumBE.Middlewares.ErrorHandling
{
    public class HandleException : Exception
    {
        public int Code { get; }

        public HandleException(string message, int code) : base(message)
        {
            Code = code;
        }
    }
}
