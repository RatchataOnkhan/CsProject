// ✅ Car.cs (รวมความสัมพันธ์ทั้งหมด: Brand, Owner, Ratings, Comments, Favorites)

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombustionCarGuideWeb.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Display(Name = "Model Name")]
        public string? ModelName { get; set; }

        public int BrandId { get; set; }  // Foreign key ไปยัง Brand
        public Brand? Brand { get; set; } // Navigation property ✅
        public string? CarBrand { get; set; } // ชื่อแบรนด์โดยตรง (สำหรับค้นหา)

        public string? Type { get; set; }
        public string? EngineType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MSRP { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CostToDrive { get; set; }

        [Column(TypeName = "decimal(2,1)")]

        public decimal CargoCapacity { get; set; }

        public int ReleaseYear { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? TrimLevels { get; set; }
        public string? FuelEconomy { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public string? TransmissionType { get; set; }
        public int SeatingCapacity { get; set; }
        public string? InfotainmentSystem { get; set; }
        public string? SafetyFeatures { get; set; }
        public string? ImageUrl { get; set; }

        // ⭐ ความสัมพันธ์กับเจ้าของรถ (One-to-Many)

        // Navigation → เจ้าของ

        // ⭐ ความสัมพันธ์ผู้กด Favorite (Many-to-Many)
        public virtual ICollection<User>? FavoritedByUsers { get; set; }

        // ⭐ ความสัมพันธ์กับความคิดเห็นและคะแนน
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating>? Ratings { get; set; }


    }
}