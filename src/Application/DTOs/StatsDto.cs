namespace Application.DTOs
{
    public class StatsDto
    {
        public required int Level { get; set; }
        public required int GamesPlayed { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}