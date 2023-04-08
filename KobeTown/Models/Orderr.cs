namespace KobeTown.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Orderr")]
    public partial class Orderr
    {
        [Key]
        public int OrderNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public bool isComplete { get; set; }

        public bool isPaid { get; set; }
    }
}
