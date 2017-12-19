namespace Store.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CarMakeMinLength)]
        [MaxLength(CarMakeMaxLength)]
        public string Make { get; set; }

        [Required]
        [MinLength(CarModelMinLength)]
        [MaxLength(CarModelMaxLength)]
        public string Model { get; set; }

        public int? MinYear { get; set; }

        public int? MaxYear { get; set; }
    }
}
