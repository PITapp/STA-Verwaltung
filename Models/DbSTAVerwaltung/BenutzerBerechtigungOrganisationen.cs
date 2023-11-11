using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("BenutzerBerechtigungOrganisationen")]
    public partial class BenutzerBerechtigungOrganisationen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BenutzerBerechtigungOrganisationenNr { get; set; }

        [Required]
        public int BenutzerNr { get; set; }

        [Required]
        public string OrganisationCode { get; set; }

        public Benutzer Benutzer { get; set; }

        public Organisationen Organisationen { get; set; }

    }
}