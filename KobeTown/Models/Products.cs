namespace KobeTown.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Category { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [StringLength(255)]
        public string Image { get; set; }
    }
}
