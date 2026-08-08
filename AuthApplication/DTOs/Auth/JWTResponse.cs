namespace AuthApplication.DTOs
{
    public class JWTResponse<T>
    {
        public JWTResponse()
        {

        }
        public JWTResponse(T data, string message = null)
        {
            Succeeded = true;
            Data = data;
            Message = message;
        }

        public JWTResponse(string message = null)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
