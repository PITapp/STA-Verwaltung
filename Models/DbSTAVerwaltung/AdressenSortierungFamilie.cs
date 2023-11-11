using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenSortierungFamilie")]
    public partial class AdressenSortierungFamilie
    {
        [Key]
        [Required]
        public int SortierungFamilie { get; set; }

        [Required]
        public string Titel { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

    }
}