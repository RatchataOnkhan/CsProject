namespace CombustionCarGuideWeb.Models
{

    public class Rating
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public Car? Car { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int Star { get; set; }
    }

}
