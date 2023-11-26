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
    public partial class Gemeinden
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

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> tabGemeinden;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> gridGemeinden;
        protected bool isEdit = true;
        protected override async Task OnInitializedAsync()
        {
            tabGemeinden = await dbSTAVerwaltungService.GetGemeinden(new Query { Expand = "Bundeslaender,GemeindenArten,LKZ1,Organisationen" });

            bundeslaenderForBundeslandCode = await dbSTAVerwaltungService.GetBundeslaender();

            gemeindenArtenForGemeindeArt = await dbSTAVerwaltungService.GetGemeindenArten();

            lKZForLKZ = await dbSTAVerwaltungService.GetLKZ();

            organisationenForOrganisationCode = await dbSTAVerwaltungService.GetOrganisationen();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            gemeinden = new STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden();
        }

        protected async Task EditRow(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden args)
        {
            isEdit = true;
            gemeinden = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden gemeinden)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await dbSTAVerwaltungService.DeleteGemeinden(gemeinden.GemeindeNr);

                    if (deleteResult != null)
                    {
                        await gridGemeinden.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Gemeinden"
                });
            }
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
                var result = isEdit ? await dbSTAVerwaltungService.UpdateGemeinden(gemeinden.GemeindeNr, gemeinden) : await dbSTAVerwaltungService.CreateGemeinden(gemeinden);

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