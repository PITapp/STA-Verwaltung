using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("AdressenEreignisse")]
    public partial class AdressenEreignisse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EreignissNr { get; set; }

        [Required]
        public int AdressNr { get; set; }

        [Required]
        public DateTime EreignisAm { get; set; }

        [Required]
        public string EreignisCode { get; set; }

        public string UnterrichtetVon { get; set; }

        public string GetauftVon { get; set; }

        public string Tauftext { get; set; }

        public string ReligionCodeFrüher { get; set; }

        public string Gemeinde { get; set; }

        public string NachGemeinde { get; set; }

        public int? Sortierung { get; set; }

        public string Info { get; set; }

        public Adressen Adressen { get; set; }

        public AdressenEreignisseArten AdressenEreignisseArten { get; set; }

        public Religionen Religionen { get; set; }

    }
}