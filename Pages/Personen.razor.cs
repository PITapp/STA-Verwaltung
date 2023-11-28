using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace STAVerwaltung.Pages
{
    public partial class Personen
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public dbSTAVerwaltungService dbSTAVerwaltungService { get; set; }

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> tabAdressen;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> gridAdressen;
        protected bool isEdit = true;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await gridAdressen.GoToPage(0);

            tabAdressen = await dbSTAVerwaltungService.GetAdressen(new Query { Filter = $@"i => i.AdressArt.Contains(@0) || i.AdressArtFrueher.Contains(@0) || i.AnredeCode.Contains(@0) || i.Name1.Contains(@0) || i.Name2.Contains(@0) || i.Name1Frueher.Contains(@0) || i.WeitereVornamen.Contains(@0) || i.Namenszusatz.Contains(@0) || i.Geschlecht.Contains(@0) || i.Titel.Contains(@0) || i.Staatsbuergerschaft1.Contains(@0) || i.Staatsbuergerschaft2.Contains(@0) || i.FamilienstandCode.Contains(@0) || i.Strasse.Contains(@0) || i.Zusatz.Contains(@0) || i.PLZ.Contains(@0) || i.Ort.Contains(@0) || i.LKZ.Contains(@0) || i.BundeslandCode.Contains(@0) || i.Telefon1.Contains(@0) || i.Telefon2.Contains(@0) || i.Mobil1.Contains(@0) || i.Mobil2.Contains(@0) || i.Skype.Contains(@0) || i.EMail1.Contains(@0) || i.EMail2.Contains(@0) || i.Fax.Contains(@0) || i.Internet.Contains(@0) || i.NotizKommunikation.Contains(@0) || i.Notiz.Contains(@0)", FilterParameters = new object[] { search }, Expand = "AdressenArten,AdressenAnreden,Bundeslaender,AdressenFamilienstaende,Gemeinden,AdressenGeschlechter,LKZ1,AdressenSortierungFamilie" });
        }
        protected override async Task OnInitializedAsync()
        {
            tabAdressen = await dbSTAVerwaltungService.GetAdressen(new Query { Filter = $@"i => i.AdressArt.Contains(@0) || i.AdressArtFrueher.Contains(@0) || i.AnredeCode.Contains(@0) || i.Name1.Contains(@0) || i.Name2.Contains(@0) || i.Name1Frueher.Contains(@0) || i.WeitereVornamen.Contains(@0) || i.Namenszusatz.Contains(@0) || i.Geschlecht.Contains(@0) || i.Titel.Contains(@0) || i.Staatsbuergerschaft1.Contains(@0) || i.Staatsbuergerschaft2.Contains(@0) || i.FamilienstandCode.Contains(@0) || i.Strasse.Contains(@0) || i.Zusatz.Contains(@0) || i.PLZ.Contains(@0) || i.Ort.Contains(@0) || i.LKZ.Contains(@0) || i.BundeslandCode.Contains(@0) || i.Telefon1.Contains(@0) || i.Telefon2.Contains(@0) || i.Mobil1.Contains(@0) || i.Mobil2.Contains(@0) || i.Skype.Contains(@0) || i.EMail1.Contains(@0) || i.EMail2.Contains(@0) || i.Fax.Contains(@0) || i.Internet.Contains(@0) || i.NotizKommunikation.Contains(@0) || i.Notiz.Contains(@0)", FilterParameters = new object[] { search }, Expand = "AdressenArten,AdressenAnreden,Bundeslaender,AdressenFamilienstaende,Gemeinden,AdressenGeschlechter,LKZ1,AdressenSortierungFamilie" });

            adressenArtenForAdressArt = await dbSTAVerwaltungService.GetAdressenArten();

            adressenAnredenForAnredeCode = await dbSTAVerwaltungService.GetAdressenAnreden();

            bundeslaenderForBundeslandCode = await dbSTAVerwaltungService.GetBundeslaender();

            adressenFamilienstaendeForFamilienstandCode = await dbSTAVerwaltungService.GetAdressenFamilienstaende();

            gemeindenForGemeindeNr = await dbSTAVerwaltungService.GetGemeinden();

            adressenGeschlechterForGeschlecht = await dbSTAVerwaltungService.GetAdressenGeschlechter();

            lKZForLKZ = await dbSTAVerwaltungService.GetLKZ();

            adressenSortierungFamilieForSortierungFamilie = await dbSTAVerwaltungService.GetAdressenSortierungFamilie();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            adressen = new STAVerwaltung.Models.dbSTAVerwaltung.Adressen();
        }

        protected async Task EditRow(STAVerwaltung.Models.dbSTAVerwaltung.Adressen args)
        {
            isEdit = true;
            adressen = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Adressen adressen)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await dbSTAVerwaltungService.DeleteAdressen(adressen.AdressNr);

                    if (deleteResult != null)
                    {
                        await gridAdressen.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Adressen"
                });
            }
        }
        protected bool errorVisible;
        protected STAVerwaltung.Models.dbSTAVerwaltung.Adressen adressen;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenArten> adressenArtenForAdressArt;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenAnreden> adressenAnredenForAnredeCode;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> bundeslaenderForBundeslandCode;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenFamilienstaende> adressenFamilienstaendeForFamilienstandCode;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> gemeindenForGemeindeNr;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenGeschlechter> adressenGeschlechterForGeschlecht;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> lKZForLKZ;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.AdressenSortierungFamilie> adressenSortierungFamilieForSortierungFamilie;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                var result = isEdit ? await dbSTAVerwaltungService.UpdateAdressen(adressen.AdressNr, adressen) : await dbSTAVerwaltungService.CreateAdressen(adressen);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}