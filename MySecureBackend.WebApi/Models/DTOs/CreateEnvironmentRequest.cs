namespace MySecureBackend.WebApi.Models.DTOs
{
    public class CreateEnvironmentRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
