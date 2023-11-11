using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("GemeindenArten")]
    public partial class GemeindenArten
    {
        [Key]
        [Required]
        public string GemeindeArt { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public ICollection<Gemeinden> Gemeinden { get; set; }

    }
}