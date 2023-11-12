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

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            gemeinden = await dbSTAVerwaltungService.GetGemeinden(new Query { Expand = "Bundeslaender,GemeindenArten,LKZ1,Organisationen" });
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
    }
}