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
    public partial class Organisationen
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

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> organisationen0;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Organisationen> grid0;
        protected bool isEdit = true;
        protected override async Task OnInitializedAsync()
        {
            organisationen0 = await dbSTAVerwaltungService.GetOrganisationen();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            organisationen = new STAVerwaltung.Models.dbSTAVerwaltung.Organisationen();
        }

        protected async Task EditRow(STAVerwaltung.Models.dbSTAVerwaltung.Organisationen args)
        {
            isEdit = true;
            organisationen = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Organisationen organisationen)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await dbSTAVerwaltungService.DeleteOrganisationen(organisationen.OrganisationCode);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Organisationen"
                });
            }
        }
        protected bool errorVisible;
        protected STAVerwaltung.Models.dbSTAVerwaltung.Organisationen organisationen;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                var result = isEdit ? await dbSTAVerwaltungService.UpdateOrganisationen(organisationen.OrganisationCode, organisationen) : await dbSTAVerwaltungService.CreateOrganisationen(organisationen);

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