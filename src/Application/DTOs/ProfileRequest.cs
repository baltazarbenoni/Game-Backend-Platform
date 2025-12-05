namespace Application.DTOs
{
    public class ProfileRequest
    {
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Message;
    }
}