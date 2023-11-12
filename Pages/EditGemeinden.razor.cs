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
    public partial class EditGemeinden
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

        [Parameter]
        public int GemeindeNr { get; set; }

        protected override async Task OnInitializedAsync()
        {
            gemeinden = await dbSTAVerwaltungService.GetGemeindenByGemeindeNr(GemeindeNr);

            bundeslaenderForBundeslandCode = await dbSTAVerwaltungService.GetBundeslaender();

            gemeindenArtenForGemeindeArt = await dbSTAVerwaltungService.GetGemeindenArten();

            lKZForLKZ = await dbSTAVerwaltungService.GetLKZ();

            organisationenForOrganisationCode = await dbSTAVerwaltungService.GetOrganisationen();
        }
        protected bool errorVisible;
        protected STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden gemeinden;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Bundeslaender> bundeslaenderForBundeslandCode;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.GemeindenArten> gemeindenArtenForGemeindeArt;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.LKZ> lKZForLKZ;

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> organisationenForOrganisationCode;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await dbSTAVerwaltungService.UpdateGemeinden(GemeindeNr, gemeinden);
                DialogService.Close(gemeinden);
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