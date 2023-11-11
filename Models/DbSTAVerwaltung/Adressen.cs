using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Adressen")]
    public partial class Adressen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdressNr { get; set; }

        [Required]
        public string AdressArt { get; set; }

        public string AdressArtFrueher { get; set; }

        [Required]
        public string AnredeCode { get; set; }

        [Required]
        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Name1Frueher { get; set; }

        public string WeitereVornamen { get; set; }

        public string Namenszusatz { get; set; }

        [Required]
        public int GemeindeNr { get; set; }

        public string Geschlecht { get; set; }

        public string Titel { get; set; }

        public string Staatsbuergerschaft1 { get; set; }

        public string Staatsbuergerschaft2 { get; set; }

        public string FamilienstandCode { get; set; }

        public string Strasse { get; set; }

        public string Zusatz { get; set; }

        public string PLZ { get; set; }

        public string Ort { get; set; }

        public string LKZ { get; set; }

        public string BundeslandCode { get; set; }

        public DateTime? Geburtsdatum { get; set; }

        public DateTime? Taufdatum { get; set; }

        public DateTime? Hochzeitsdatum { get; set; }

        public DateTime? Sterbedatum { get; set; }

        public int? SortierungFamilie { get; set; }

        public string Telefon1 { get; set; }

        public string Telefon2 { get; set; }

        public string Mobil1 { get; set; }

        public string Mobil2 { get; set; }

        public string Skype { get; set; }

        public string EMail1 { get; set; }

        public string EMail2 { get; set; }

        public string Fax { get; set; }

        public string Internet { get; set; }

        public string NotizKommunikation { get; set; }

        public string Notiz { get; set; }

        public AdressenArten AdressenArten { get; set; }

        public AdressenAnreden AdressenAnreden { get; set; }

        public Bundeslaender Bundeslaender { get; set; }

        public AdressenFamilienstaende AdressenFamilienstaende { get; set; }

        public Gemeinden Gemeinden { get; set; }

        public AdressenGeschlechter AdressenGeschlechter { get; set; }

        public LKZ LKZ1 { get; set; }

        public AdressenSortierungFamilie AdressenSortierungFamilie { get; set; }

        public ICollection<AdressenEreignisse> AdressenEreignisse { get; set; }

    }
}