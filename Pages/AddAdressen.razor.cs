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
    public partial class AddAdressen
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

        protected override async Task OnInitializedAsync()
        {
            adressen = new STAVerwaltung.Models.dbSTAVerwaltung.Adressen();

            adressenArtenForAdressArt = await dbSTAVerwaltungService.GetAdressenArten();

            adressenAnredenForAnredeCode = await dbSTAVerwaltungService.GetAdressenAnreden();

            bundeslaenderForBundeslandCode = await dbSTAVerwaltungService.GetBundeslaender();

            adressenFamilienstaendeForFamilienstandCode = await dbSTAVerwaltungService.GetAdressenFamilienstaende();

            gemeindenForGemeindeNr = await dbSTAVerwaltungService.GetGemeinden();

            adressenGeschlechterForGeschlecht = await dbSTAVerwaltungService.GetAdressenGeschlechter();

            lKZForLKZ = await dbSTAVerwaltungService.GetLKZ();

            adressenSortierungFamilieForSortierungFamilie = await dbSTAVerwaltungService.GetAdressenSortierungFamilie();
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
                await dbSTAVerwaltungService.CreateAdressen(adressen);
                DialogService.Close(adressen);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}