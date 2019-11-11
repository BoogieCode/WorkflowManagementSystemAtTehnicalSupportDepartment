namespace licenta.DatabaseConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        public int requestId { get; set; }

        public int createdBy { get; set; }

        [Required]
        [StringLength(50)]
        public string title { get; set; }

        [Required]
        [StringLength(500)]
        public string description { get; set; }

        public bool type { get; set; }

        [StringLength(50)]
        public string departmentAssigned { get; set; }

        public string employeeAssigned { get; set; }

        public string image { get; set; }

        public int priority { get; set; }

        public virtual User User { get; set; }
    }
}
