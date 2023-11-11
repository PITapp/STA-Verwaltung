using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using STAVerwaltung.Models.dbSTAVerwaltung;

namespace STAVerwaltung.Data
{
    public partial class dbSTAVerwaltungContext : DbContext
    {
        public dbSTAVerwaltungContext()
        {
        }

        public dbSTAVerwaltungContext(DbContextOptions<dbSTAVerwaltungContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.AdressenArten)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.AdressArt)
              .HasPrincipalKey(i => i.AdressArt);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.AdressenAnreden)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.AnredeCode)
              .HasPrincipalKey(i => i.AnredeCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.Bundeslaender)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.BundeslandCode)
              .HasPrincipalKey(i => i.BundeslandCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.AdressenFamilienstaende)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.FamilienstandCode)
              .HasPrincipalKey(i => i.FamilienstandCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.Gemeinden)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.GemeindeNr)
              .HasPrincipalKey(i => i.GemeindeNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.AdressenGeschlechter)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.Geschlecht)
              .HasPrincipalKey(i => i.Geschlecht);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.LKZ1)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.LKZ)
              .HasPrincipalKey(i => i.LKZ1);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Adressen>()
              .HasOne(i => i.AdressenSortierungFamilie)
              .WithMany(i => i.Adressen)
              .HasForeignKey(i => i.SortierungFamilie)
              .HasPrincipalKey(i => i.SortierungFamilie);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse>()
              .HasOne(i => i.Adressen)
              .WithMany(i => i.AdressenEreignisse)
              .HasForeignKey(i => i.AdressNr)
              .HasPrincipalKey(i => i.AdressNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse>()
              .HasOne(i => i.AdressenEreignisseArten)
              .WithMany(i => i.AdressenEreignisse)
              .HasForeignKey(i => i.EreignisCode)
              .HasPrincipalKey(i => i.EreignisCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse>()
              .HasOne(i => i.Religionen)
              .WithMany(i => i.AdressenEreignisse)
              .HasForeignKey(i => i.ReligionCodeFrüher)
              .HasPrincipalKey(i => i.ReligionCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden>()
              .HasOne(i => i.Benutzer)
              .WithMany(i => i.BenutzerBerechtigungGemeinden)
              .HasForeignKey(i => i.BenutzerNr)
              .HasPrincipalKey(i => i.BenutzerNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden>()
              .HasOne(i => i.Gemeinden)
              .WithMany(i => i.BenutzerBerechtigungGemeinden)
              .HasForeignKey(i => i.GemeindeNr)
              .HasPrincipalKey(i => i.GemeindeNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen>()
              .HasOne(i => i.Benutzer)
              .WithMany(i => i.BenutzerBerechtigungOrganisationen)
              .HasForeignKey(i => i.BenutzerNr)
              .HasPrincipalKey(i => i.BenutzerNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen>()
              .HasOne(i => i.Organisationen)
              .WithMany(i => i.BenutzerBerechtigungOrganisationen)
              .HasForeignKey(i => i.OrganisationCode)
              .HasPrincipalKey(i => i.OrganisationCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll>()
              .HasOne(i => i.Benutzer)
              .WithMany(i => i.BenutzerProtokoll)
              .HasForeignKey(i => i.BenutzerNr)
              .HasPrincipalKey(i => i.BenutzerNr);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender>()
              .HasOne(i => i.LKZ1)
              .WithMany(i => i.Bundeslaender)
              .HasForeignKey(i => i.LKZ)
              .HasPrincipalKey(i => i.LKZ1);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden>()
              .HasOne(i => i.Bundeslaender)
              .WithMany(i => i.Gemeinden)
              .HasForeignKey(i => i.BundeslandCode)
              .HasPrincipalKey(i => i.BundeslandCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden>()
              .HasOne(i => i.GemeindenArten)
              .WithMany(i => i.Gemeinden)
              .HasForeignKey(i => i.GemeindeArt)
              .HasPrincipalKey(i => i.GemeindeArt);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden>()
              .HasOne(i => i.LKZ1)
              .WithMany(i => i.Gemeinden)
              .HasForeignKey(i => i.LKZ)
              .HasPrincipalKey(i => i.LKZ1);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden>()
              .HasOne(i => i.Organisationen)
              .WithMany(i => i.Gemeinden)
              .HasForeignKey(i => i.OrganisationCode)
              .HasPrincipalKey(i => i.OrganisationCode);

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer>()
              .Property(p => p.AspNetUsers_Id)
              .HasDefaultValueSql(@"'Unbekannt'");

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer>()
              .Property(p => p.Benutzername)
              .HasDefaultValueSql(@"'Unbekannt'");

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer>()
              .Property(p => p.Initialen)
              .HasDefaultValueSql(@"'UnBk'");

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer>()
              .Property(p => p.BenutzerEMail)
              .HasDefaultValueSql(@"'Unbekannt'");

            builder.Entity<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll>()
              .Property(p => p.Zeitstempel)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> Adressen { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> AdressenAnreden { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> AdressenArten { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisse> AdressenEreignisse { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenEreignisseArten> AdressenEreignisseArten { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> AdressenFamilienstaende { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> AdressenGeschlechter { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> AdressenSortierungFamilie { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Benutzer> Benutzer { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungGemeinden> BenutzerBerechtigungGemeinden { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerBerechtigungOrganisationen> BenutzerBerechtigungOrganisationen { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.BenutzerProtokoll> BenutzerProtokoll { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> Bundeslaender { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> Gemeinden { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> GemeindenArten { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> LKZ { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> Losungen { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> Organisationen { get; set; }

        public DbSet<STAVerwaltung.Models.dbSTAVerwaltung.Religionen> Religionen { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}