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
    public partial class Losungen
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

        protected IEnumerable<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> losungen;

        protected RadzenDataGrid<STAVerwaltung.Models.dbSTAVerwaltung.Losungen> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            losungen = await dbSTAVerwaltungService.GetLosungen();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await grid0.InsertRow(new STAVerwaltung.Models.dbSTAVerwaltung.Losungen());
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Losungen losungen)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await dbSTAVerwaltungService.DeleteLosungen(losungen.LosungDatum);

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
                    Detail = $"Unable to delete Losungen"
                });
            }
        }

        protected async Task GridRowUpdate(STAVerwaltung.Models.dbSTAVerwaltung.Losungen args)
        {
            await dbSTAVerwaltungService.UpdateLosungen(args.LosungDatum, args);
        }

        protected async Task GridRowCreate(STAVerwaltung.Models.dbSTAVerwaltung.Losungen args)
        {
            await dbSTAVerwaltungService.CreateLosungen(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Losungen data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Losungen data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, STAVerwaltung.Models.dbSTAVerwaltung.Losungen data)
        {
            grid0.CancelEditRow(data);
            await dbSTAVerwaltungService.CancelLosungenChanges(data);
        }
    }
}