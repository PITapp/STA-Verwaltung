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

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> gemeinden;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> grid0;

        string strCountGemeinden;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            gemeinden = await dbSTAVerwaltungService.GetGemeinden(new Query { Expand = "Bundeslaender,GemeindenArten,LKZ1,Organisationen" });
            SetCountGemeinden();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddGemeinden>("Add Gemeinden", null);
            await grid0.Reload();
        }

        protected async Task EditRow(STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden args)
        {
            await DialogService.OpenAsync<EditGemeinden>("Edit Gemeinden", new Dictionary<string, object> { {"GemeindeNr", args.GemeindeNr} });
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
                    Detail = $"Unable to delete Gemeinden"
                });
            }
        }

        protected async System.Threading.Tasks.Task DataGrid0Filter(Radzen.DataGridColumnFilterEventArgs<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> args)
        {
            SetCountGemeinden();
        }

        protected async System.Threading.Tasks.Task DataGrid0FilterCleared(Radzen.DataGridColumnFilterEventArgs<STAVerwaltung.Models.dbSTAVerwaltung.Gemeinden> args)
        {
            SetCountGemeinden();
        }

        protected void SetCountGemeinden()
        {
            if (grid0 == null)
            {
                strCountGemeinden = gemeinden.Count().ToString();
            } else {
                if (gemeinden.Count() == grid0.View.Count())
                {
                    strCountGemeinden = gemeinden.Count().ToString();
                } else {
                    strCountGemeinden = grid0.View.Count().ToString() + " (Filter)";
                }
            }
        }

        protected async System.Threading.Tasks.Task Button1Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<OrganisationenTest>("Organisationen Test", null, new DialogOptions { Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
        }
    }
}