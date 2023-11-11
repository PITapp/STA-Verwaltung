using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenAnreden")]
    public partial class AdressenAnreden
    {
        [Key]
        [Required]
        public string AnredeCode { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

    }
}