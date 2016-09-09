namespace CodeModelFromDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
