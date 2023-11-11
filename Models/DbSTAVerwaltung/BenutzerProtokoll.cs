using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("BenutzerProtokoll")]
    public partial class BenutzerProtokoll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenutzerProtokollNr { get; set; }

        [Required]
        public int BenutzerNr { get; set; }

        [Required]
        public string Art { get; set; }

        [Required]
        public DateTime Zeitstempel { get; set; }

        public Benutzer Benutzer { get; set; }

    }
}