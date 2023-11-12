using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenEreignisseArten")]
    public partial class AdressenEreignisseArten
    {
        [Key]
        [Required]
        public string EreignisCode { get; set; }

        [Required]
        public string Titel { get; set; }

        public string Verlauftext { get; set; }

        public string SicherheitsInfo { get; set; }

        public string Maske { get; set; }

        public int? Sortierung { get; set; }

        [Required]
        public int Sortierung2 { get; set; }

        public ICollection<AdressenEreignisse> AdressenEreignisse { get; set; }

    }
}