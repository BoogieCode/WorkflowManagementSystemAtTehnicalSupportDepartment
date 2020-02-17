namespace licenta.DatabaseConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestHistory")]
    public partial class RequestHistory
    {
        public int requestHistoryId { get; set; }

        public int requestId { get; set; }

        [Required]
        [StringLength(50)]
        public string from { get; set; }

        [Required]
        [StringLength(50)]
        public string to { get; set; }

        public DateTime data { get; set; }

        [StringLength(50)]
        public string approval { get; set; }

        [StringLength(50)]
        public string message { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        public int? attachmentsId { get; set; }

        public virtual File File { get; set; }
    }
}
