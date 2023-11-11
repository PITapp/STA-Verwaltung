using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Benutzer")]
    public partial class Benutzer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenutzerNr { get; set; }

        public string AspNetUsers_Id { get; set; }

        [Required]
        public int AdressNr { get; set; }

        public string Benutzername { get; set; }

        public string Initialen { get; set; }

        public string BenutzerEMail { get; set; }

        public string LKZ { get; set; }

        public string Notiz { get; set; }

        public string FilterTextbausteinArtCode { get; set; }

        public int? FilterKursNr { get; set; }

        public string FilterTitelUndText { get; set; }

        public int? FilterAutorNr { get; set; }

        public int? FilterThemaNummer { get; set; }

        public string FilterAnrede { get; set; }

        public string FilterDokument { get; set; }

        public string FilterInfo { get; set; }

        public ICollection<BenutzerBerechtigungGemeinden> BenutzerBerechtigungGemeinden { get; set; }

        public ICollection<BenutzerBerechtigungOrganisationen> BenutzerBerechtigungOrganisationen { get; set; }

        public ICollection<BenutzerProtokoll> BenutzerProtokoll { get; set; }

    }
}