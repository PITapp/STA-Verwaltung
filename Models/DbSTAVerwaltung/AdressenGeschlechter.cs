using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenGeschlechter")]
    public partial class AdressenGeschlechter
    {
        [Key]
        [Required]
        public string Geschlecht { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

    }
}