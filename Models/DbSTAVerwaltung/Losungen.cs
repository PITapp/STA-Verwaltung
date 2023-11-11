using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Losungen")]
    public partial class Losungen
    {
        [Key]
        [Required]
        public DateTime LosungDatum { get; set; }

        [Required]
        public string ATStelle { get; set; }

        [Required]
        public string ATText { get; set; }

        [Required]
        public string NTStelle { get; set; }

        [Required]
        public string NTText { get; set; }

    }
}