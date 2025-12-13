namespace ReservasTucson.Domain.Support.Helpers
{
    public class BackendException : Exception
    {
        public BackendException(
            int statusCode,
            string title,
            List<string> errors,
            string error) : base(error)
        {
            StatusCode = statusCode;
            Errors = errors;
            Title = title;
        }

        public int StatusCode { get; private set; }
        public string Title { get; private set; }
        public List<string> Errors { get; private set; }
    }
}
