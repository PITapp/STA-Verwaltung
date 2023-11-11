using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenArten")]
    public partial class AdressenArten
    {
        [Key]
        [Required]
        public string AdressArt { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public string TitelMehrzahl { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

    }
}