using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using STAVerwaltung.Pages;

namespace STAVerwaltung.Shared
{
    public partial class MainLayout
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

        // private bool sidebarExpanded = true;

        [Inject]
        protected SecurityService Security { get; set; }

        //void SidebarToggleClick()
        //{
        //    sidebarExpanded = !sidebarExpanded;
        //}

        protected void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }

        protected async System.Threading.Tasks.Task ButtonMenueClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<StartMenue>("Start Menue", null, new DialogOptions { CloseDialogOnOverlayClick = true, Top = "0px", Left = "0px" });
            //await DialogService.OpenSideAsync<StartMenue>("Start Menue", options: new SideDialogOptions { CloseDialogOnOverlayClick = true, Position = DialogPosition.Left, ShowMask = true});

        }
    }
}
