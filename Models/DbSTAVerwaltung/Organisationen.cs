using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Organisationen")]
    public partial class Organisationen
    {
        [Key]
        [Required]
        public string OrganisationCode { get; set; }

        [Required]
        public string Art { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public string Kurzzeichen { get; set; }

        public string Vereinigung { get; set; }

        [Required]
        public string Verband { get; set; }

        [Required]
        public string VerbandKurzzeichen { get; set; }

        [Required]
        public string Division { get; set; }

        public string Straße { get; set; }

        public string PLZ { get; set; }

        public string Ort { get; set; }

        public string LKZ { get; set; }

        public string BundeslandCode { get; set; }

        public string Telefon { get; set; }

        public string EMail { get; set; }

        public string Internet { get; set; }

        public string DVRNummer { get; set; }

        [Required]
        public int Sortierung { get; set; }

        public string Info { get; set; }

        public ICollection<BenutzerBerechtigungOrganisationen> BenutzerBerechtigungOrganisationen { get; set; }

        public ICollection<Gemeinden> Gemeinden { get; set; }

    }
}