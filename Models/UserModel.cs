namespace CombustionCarGuideWeb.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; } // ใช้สำหรับเก็บ hash อย่างเดียว

        // ความสัมพันธ์
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Car> FavoriteCars { get; set; } = new List<Car>();
    }

}
