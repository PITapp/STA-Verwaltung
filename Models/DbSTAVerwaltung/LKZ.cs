using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAVerwaltung.Models.dbSTAVerwaltung
{
    [Table("LKZ")]
    public partial class LKZ
    {
        [Key]
        [Column("LKZ")]
        [Required]
        public string LKZ1 { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public string TitelEnglisch { get; set; }

        public ICollection<Adressen> Adressen { get; set; }

        public ICollection<Bundeslaender> Bundeslaender { get; set; }

        public ICollection<Gemeinden> Gemeinden { get; set; }

    }
}