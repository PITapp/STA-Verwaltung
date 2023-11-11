using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using STAVerwaltung.Data;

namespace STAVerwaltung.Controllers
{
    public partial class ExportdbSTAVerwaltungController : ExportController
    {
        private readonly dbSTAVerwaltungContext context;
        private readonly dbSTAVerwaltungService service;

        public ExportdbSTAVerwaltungController(dbSTAVerwaltungContext context, dbSTAVerwaltungService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/dbSTAVerwaltung/adressen/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressen/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenanreden/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressenanreden/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenAnredenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenAnreden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenanreden/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressenanreden/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenAnredenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenAnreden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenarten/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressenarten/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenArtenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenarten/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressenarten/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenArtenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenereignisse/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressenereignisse/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenEreignisseToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenEreignisse(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenereignisse/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressenereignisse/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenEreignisseToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenEreignisse(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenereignissearten/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressenereignissearten/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenEreignisseArtenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenEreignisseArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenereignissearten/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressenereignissearten/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenEreignisseArtenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenEreignisseArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenfamilienstaende/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressenfamilienstaende/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenFamilienstaendeToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenFamilienstaende(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressenfamilienstaende/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressenfamilienstaende/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenFamilienstaendeToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenFamilienstaende(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressengeschlechter/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressengeschlechter/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenGeschlechterToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenGeschlechter(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressengeschlechter/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressengeschlechter/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenGeschlechterToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenGeschlechter(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressensortierungfamilie/csv")]
        [HttpGet("/export/dbSTAVerwaltung/adressensortierungfamilie/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenSortierungFamilieToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdressenSortierungFamilie(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/adressensortierungfamilie/excel")]
        [HttpGet("/export/dbSTAVerwaltung/adressensortierungfamilie/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdressenSortierungFamilieToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdressenSortierungFamilie(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzer/csv")]
        [HttpGet("/export/dbSTAVerwaltung/benutzer/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBenutzer(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzer/excel")]
        [HttpGet("/export/dbSTAVerwaltung/benutzer/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBenutzer(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigunggemeinden/csv")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigunggemeinden/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerBerechtigungGemeindenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBenutzerBerechtigungGemeinden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigunggemeinden/excel")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigunggemeinden/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerBerechtigungGemeindenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBenutzerBerechtigungGemeinden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigungorganisationen/csv")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigungorganisationen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerBerechtigungOrganisationenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBenutzerBerechtigungOrganisationen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigungorganisationen/excel")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerberechtigungorganisationen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerBerechtigungOrganisationenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBenutzerBerechtigungOrganisationen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerprotokoll/csv")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerprotokoll/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerProtokollToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBenutzerProtokoll(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/benutzerprotokoll/excel")]
        [HttpGet("/export/dbSTAVerwaltung/benutzerprotokoll/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBenutzerProtokollToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBenutzerProtokoll(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/bundeslaender/csv")]
        [HttpGet("/export/dbSTAVerwaltung/bundeslaender/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBundeslaenderToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBundeslaender(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/bundeslaender/excel")]
        [HttpGet("/export/dbSTAVerwaltung/bundeslaender/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBundeslaenderToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBundeslaender(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/gemeinden/csv")]
        [HttpGet("/export/dbSTAVerwaltung/gemeinden/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGemeindenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGemeinden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/gemeinden/excel")]
        [HttpGet("/export/dbSTAVerwaltung/gemeinden/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGemeindenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGemeinden(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/gemeindenarten/csv")]
        [HttpGet("/export/dbSTAVerwaltung/gemeindenarten/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGemeindenArtenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGemeindenArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/gemeindenarten/excel")]
        [HttpGet("/export/dbSTAVerwaltung/gemeindenarten/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGemeindenArtenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGemeindenArten(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/lkz/csv")]
        [HttpGet("/export/dbSTAVerwaltung/lkz/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLKZToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLKZ(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/lkz/excel")]
        [HttpGet("/export/dbSTAVerwaltung/lkz/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLKZToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLKZ(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/losungen/csv")]
        [HttpGet("/export/dbSTAVerwaltung/losungen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLosungenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLosungen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/losungen/excel")]
        [HttpGet("/export/dbSTAVerwaltung/losungen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLosungenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLosungen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/organisationen/csv")]
        [HttpGet("/export/dbSTAVerwaltung/organisationen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrganisationenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrganisationen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/organisationen/excel")]
        [HttpGet("/export/dbSTAVerwaltung/organisationen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrganisationenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrganisationen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/religionen/csv")]
        [HttpGet("/export/dbSTAVerwaltung/religionen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportReligionenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetReligionen(), Request.Query), fileName);
        }

        [HttpGet("/export/dbSTAVerwaltung/religionen/excel")]
        [HttpGet("/export/dbSTAVerwaltung/religionen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportReligionenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetReligionen(), Request.Query), fileName);
        }
    }
}
