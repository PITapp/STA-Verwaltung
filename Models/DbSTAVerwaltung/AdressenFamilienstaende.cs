using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenFamilienstaende")]
    public partial class AdressenFamilienstaende
    {
        [Key]
        [Required]
        public string FamilienstandCode { get; set; }

        [Required]
        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        [Required]
        public string Kurzzeichen { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

    }
}