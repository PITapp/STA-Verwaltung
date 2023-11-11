using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("Bundeslaender")]
    public partial class Bundeslaender
    {
        [Key]
        [Required]
        public string BundeslandCode { get; set; }

        [Required]
        public string LKZ { get; set; }

        [Required]
        public string Titel { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

        public LKZ LKZ1 { get; set; }

        public ICollection<Gemeinden> Gemeinden { get; set; }

    }
}