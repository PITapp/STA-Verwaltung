using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("BenutzerBerechtigungGemeinden")]
    public partial class BenutzerBerechtigungGemeinden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenutzerBerechtigungGemeindenNr { get; set; }

        [Required]
        public int BenutzerNr { get; set; }

        [Required]
        public int GemeindeNr { get; set; }

        public Benutzer Benutzer { get; set; }

        public Gemeinden Gemeinden { get; set; }

    }
}