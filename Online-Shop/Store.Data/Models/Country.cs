namespace Store.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static ModelConstants;

    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CountryNameMinLength)]
        [MaxLength(CountryNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Town> Towns { get; set; } = new HashSet<Town>();
    }
}
