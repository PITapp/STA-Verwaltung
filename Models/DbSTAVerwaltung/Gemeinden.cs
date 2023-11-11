using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Gemeinden")]
    public partial class Gemeinden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GemeindeNr { get; set; }

        [Required]
        public string GemeindeArt { get; set; }

        [Required]
        public string Titel { get; set; }

        public int? ZuGemeindeNr { get; set; }

        public string Kürzel { get; set; }

        public string Straße { get; set; }

        public string PLZ { get; set; }

        public string Ort { get; set; }

        public string LKZ { get; set; }

        public string BundeslandCode { get; set; }

        public string Telefon { get; set; }

        public string EMail { get; set; }

        public string Internet { get; set; }

        public DateTime? GegründetAm { get; set; }

        public DateTime? AufgelöstAm { get; set; }

        public string AuflösungInfo { get; set; }

        public string Bezirk { get; set; }

        public string Großbezirk { get; set; }

        [Required]
        public string OrganisationCode { get; set; }

        public string Sprachen { get; set; }

        public string Wegbeschreibung { get; set; }

        public string GesprächsleitervorbereitungTag { get; set; }

        public TimeOnly? GesprächsleitervorbereitungZeit { get; set; }

        public TimeOnly? GottesdienstZeit { get; set; }

        public string AndachtTag { get; set; }

        public TimeOnly? AndachtZeit { get; set; }

        public string Info { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

        public ICollection<BenutzerBerechtigungGemeinden> BenutzerBerechtigungGemeinden { get; set; }

        public Bundeslaender Bundeslaender { get; set; }

        public GemeindenArten GemeindenArten { get; set; }

        public LKZ LKZ1 { get; set; }

        public Organisationen Organisationen { get; set; }

    }
}