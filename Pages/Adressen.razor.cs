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
    public partial class Adressen
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

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> adressen;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Adressen> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            adressen = await dbSTAVerwaltungService.GetAdressen(new Query { Expand = "AdressenArten,AdressenAnreden,Bundeslaender,AdressenFamilienstaende,Gemeinden,AdressenGeschlechter,LKZ1,AdressenSortierungFamilie" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAdressen>("Add Adressen", null);
            await grid0.Reload();
        }

        protected async Task EditRow(STAVerwaltung.Models.dbSTAVerwaltung.Adressen args)
        {
            await DialogService.OpenAsync<EditAdressen>("Edit Adressen", new Dictionary<string, object> { {"AdressNr", args.AdressNr} });
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
                    Detail = $"Unable to delete Adressen"
                });
            }
        }
    }
}