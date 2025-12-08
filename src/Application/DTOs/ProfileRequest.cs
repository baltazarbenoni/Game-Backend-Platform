namespace Application.DTOs
{
    public class ProfileRequest
    {
        public required string DisplayName { get; set; } = default!;
        public required string Email { get; set;  } = default!;
        public required string Message {get; set; } = default!;
    }
}