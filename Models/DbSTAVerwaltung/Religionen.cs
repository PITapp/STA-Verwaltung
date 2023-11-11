using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Religionen")]
    public partial class Religionen
    {
        [Key]
        [Required]
        public string ReligionCode { get; set; }

        [Required]
        public string Titel { get; set; }

        public ICollection<AdressenEreignisse> AdressenEreignisse { get; set; }

    }
}