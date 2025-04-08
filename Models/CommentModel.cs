using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombustionCarGuideWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;


        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
